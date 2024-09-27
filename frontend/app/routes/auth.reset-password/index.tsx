import {
  ActionFunctionArgs,
  LoaderFunctionArgs,
  MetaFunction,
  json,
} from "@remix-run/cloudflare";
import { parse as cookieParse } from "cookie";
import { expiredResetTokenResponse } from "./process-forgot-password-responses.server";
import {
  Form,
  useActionData,
  useLoaderData,
  useNavigation,
} from "@remix-run/react";
import { requireAnonymous } from "~/utils/auth.server";
import { PasswordAndConfirmPasswordSchema } from "./reset-password-schema";
import { useId } from "react";
import { useForm, conform } from "@conform-to/react";
import { parse } from "@conform-to/zod";
import { tw } from "~/utils/tw-identity-helper";
import { usePasswordReveal } from "~/utils/hooks/usePasswordReveal";
import { PasswordRevealBtn } from "~/components/ui/PasswordRevealBtn";
import { resetPassword } from "./reset-password-action.server";
import { Alert, AlertTitle } from "~/components/ui/Alert";
import { default as AlertCircle } from "~/components/icons/icon.tsx";
import { redirectWithSuccessToast } from "~/utils/flash-session/flash-session.server";

export const meta: MetaFunction = () => {
  return [{ title: "Disengage | Password Reset" }];
};

const ResetPasswordSchema = PasswordAndConfirmPasswordSchema;

export async function action({ request, context }: ActionFunctionArgs) {
  const { env } = context.cloudflare;
  const formData = await request.formData();

  const state = String(formData.get("state"));

  if (state === "submitting") return null;

  const submission = parse(formData, {
    schema: PasswordAndConfirmPasswordSchema,
  });

  if (submission.intent !== "submit" || !submission.value) {
    return json(submission);
  }

  const passwordResetUserId = String(formData.get("PasswordResetUserId"));
  const code = String(formData.get("Code"));
  const password = String(formData.get("password"));
  const passwordConfirmation = String(formData.get("passwordConfirmation"));

  const resetPasswordResult = await resetPassword(
    env,
    passwordResetUserId,
    code,
    password,
    passwordConfirmation,
  );

  if (!resetPasswordResult.ok && resetPasswordResult.status === 409) {
    const resetPasswordError: { Message: string } =
      await resetPasswordResult.json();

    return json(resetPasswordError);
  } else if (!resetPasswordResult.ok && resetPasswordResult.status === 400) {
    // temporarily throw a generic error, but need to look at the BadRequest response
    // from the ResetPassword endpoint so we can give the user more specific and better
    // error messages
    throw new Response("", {
      status: 500,
      statusText: "We ran into an issue logging you out",
    });
  }

  const resetPasswordSuccessMessage: { SuccessMessage: string } =
    await resetPasswordResult.json();

  return redirectWithSuccessToast(
    "/auth/login",
    resetPasswordSuccessMessage.SuccessMessage,
    env,
  );
}

export async function loader({ request, context }: LoaderFunctionArgs) {
  const { env } = context.cloudflare;
  requireAnonymous(request);
  const cookieHeaders = request.headers.get("Cookie");

  const cookieObj = cookieParse(cookieHeaders || "");

  // if either the password reset user id or the password reset code
  // is missing, redirect to the login page. This should only happen
  // if the user tries to access this route without going through
  // the password reset email link
  // if (!cookieObj["PasswordResetUserId"] || !cookieObj["Code"]) {
  //   throw redirect("/auth/login");
  // }

  const validTokenResponseValues: Record<string, string> = {};

  for (const cookieName in cookieObj) {
    if (cookieName === "ExpiredMessage") {
      const expiredTokenMessage = cookieObj["ExpiredMessage"];

      return expiredResetTokenResponse(expiredTokenMessage, env);
    } else if (cookieName === "PasswordResetUserId") {
      validTokenResponseValues[cookieName] = cookieObj[cookieName];
    } else if (cookieName === "Code") {
      validTokenResponseValues[cookieName] = cookieObj[cookieName];
    }
  }

  return json({ validTokenResponseValues });
}

export default function ResetPassword() {
  const data = useLoaderData<typeof loader>();
  const passwordResetUserId =
    data.validTokenResponseValues["PasswordResetUserId"];

  const code = data.validTokenResponseValues["Code"];

  const passwordReveal = usePasswordReveal();

  const passwordInputType = passwordReveal.showPassword ? "text" : "password";
  const passwordConfirmationInputType = passwordReveal.showPassword
    ? "text"
    : "password";

  const actionData = useActionData<typeof action>();

  const lastSubmission =
    actionData && "intent" in actionData ? actionData : null;

  const passwordExistsError =
    actionData && "Message" in actionData ? actionData.Message : null;

  const navigation = useNavigation();

  const id = useId();

  const [form, fields] = useForm({
    id,
    lastSubmission,
    shouldValidate: "onBlur",

    onValidate({ formData }) {
      return parse(formData, { schema: ResetPasswordSchema });
    },
  });

  return (
    <main className="flex flex-1 flex-col items-center justify-center gap-5 bg-gray-100 px-8 py-12 md:p-12">
      {passwordExistsError ? (
        <Alert className="md-shadow max-w-[24rem] bg-white">
          <AlertCircle
            icon="alert-circle"
            className="size-[22px] fill-red-400 pt-[2px]"
          />
          <AlertTitle className="pb-0 text-lg font-normal">
            {passwordExistsError}
          </AlertTitle>
        </Alert>
      ) : null}
      <div className="mx-auto flex w-full max-w-[24rem] flex-col justify-start gap-4 rounded-lg border border-white bg-white p-6 px-8 py-6 shadow-md md:px-12">
        <h2 className="mt-4 text-center text-2xl capitalize">
          reset your password
        </h2>

        <div className="flex-col items-center">
          <Form
            method="post"
            {...form.props}
            className="flex w-full flex-col gap-4"
          >
            <input type="hidden" name="state" value={navigation.state} />
            <input
              type="hidden"
              name="PasswordResetUserId"
              value={passwordResetUserId}
            />
            <input type="hidden" name="Code" value={code} />
            <fieldset>
              <RevealInputField
                labelProps={{
                  children: "Password",
                  baseClass:
                    "absolute -top-2.5 font-normal left-2 bg-[#ffffff] text-sm px-1 capitalize transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-400 peer-focus-visible:-top-2.5 peer-focus-visible:text-sm peer-focus-visible:text-gray-700",
                  errorsClass: "text-red-700 peer-focus-visible:text-red-700",
                }}
                inputProps={{
                  passwordField: fields.password,
                  placeholder: "email@example.com",
                  baseClass:
                    "signupInputAutofill border-color-[unset] placeholder:text-transparent focus-visible:ring-color-[unset] peer block h-[unset] w-full rounded-md border border-gray-400 bg-[#ffffff] px-3 py-[14px] text-base text-[unset] text-gray-700 placeholder-transparent ring-[unset] focus-visible:border-gray-700 focus-visible:outline-none focus-visible:ring-0 focus-visible:ring-offset-0",
                  inputErrorsClass:
                    "border-red-700 caret-red-700 focus-visible:border-red-700",
                }}
                errorProps={{
                  errors: fields.password.errors,
                  errorClass:
                    "self-start pl-1 text-sm text-red-700 transition-opacity duration-300 ease-in-out",
                }}
              />

              <RevealInputField
                labelProps={{
                  children: "Password Confirmation",
                  baseClass:
                    "absolute -top-2.5 font-normal left-2 bg-[#ffffff] text-sm px-1 capitalize transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-400 peer-focus-visible:-top-2.5 peer-focus-visible:text-sm peer-focus-visible:text-gray-700",
                  errorsClass: "text-red-700 peer-focus-visible:text-red-700",
                }}
                inputProps={{
                  passwordField: fields.passwordConfirmation,
                  placeholder: "email@example.com",
                  baseClass:
                    "signupInputAutofill  border-color-[unset] placeholder:text-transparent focus-visible:ring-color-[unset] peer block h-[unset] w-full rounded-md border border-gray-400 bg-[#ffffff] px-3 py-[14px] text-base text-[unset] text-gray-700 placeholder-transparent ring-[unset] focus-visible:border-gray-700 focus-visible:outline-none focus-visible:ring-0 focus-visible:ring-offset-0",
                  inputErrorsClass:
                    "border-red-700 focus-visible:border-red-700",
                }}
                errorProps={{
                  errors: fields.passwordConfirmation.errors,
                  errorClass:
                    "self-start pl-1 text-sm text-red-700 transition-opacity duration-300 ease-in-out",
                }}
              />
            </fieldset>

            <div className="mt-4 flex w-full flex-1 items-center justify-between gap-6">
              <StatusButton
                className="h-[54px] w-full flex-1 gap-4 px-3 py-[14px] capitalize"
                status={isPending ? "pending" : actionData?.status ?? "idle"}
                type="submit"
                disabled={isPending}
              >
                reset password
              </StatusButton>
            </div>
          </Form>
        </div>
      </div>
    </main>
  );
}
