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
import { usePasswordReveal } from "~/utils/usePasswordReveal";
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
  await requireAnonymous(request);
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
  const passwordConfirmationInputType = passwordReveal.showPasswordConfirmation
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
    <main className="flex min-h-screen flex-col items-center justify-center gap-5 bg-gray-100 px-8 py-12 md:p-12">
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

        <Form method="post" {...form.props} className="w-full">
          <input type="hidden" name="state" value={navigation.state} />
          <input
            type="hidden"
            name="PasswordResetUserId"
            value={passwordResetUserId}
          />
          <input type="hidden" name="Code" value={code} />
          <fieldset>
            <div className="mt-8 flex w-full flex-col items-center gap-[6px]">
              <div className="relative w-full">
                <input
                  className={tw`${
                    fields.password.errors?.length
                      ? "border-red-700 focus:border-red-700  "
                      : ""
                  }   signupInputAutofill peer block w-full rounded-md border  border-gray-500 bg-[#ffffff] px-3 py-[14px] text-gray-700  placeholder-transparent  focus:border-gray-700  focus:outline-none`}
                  {...conform.input(fields.password, {
                    type: passwordInputType,
                  })}
                  placeholder="password"
                />
                <PasswordRevealBtn
                  showPassword={passwordReveal.showPassword}
                  togglePassword={passwordReveal.togglePassword}
                />

                <label
                  className={tw`${
                    fields.password.errors?.length
                      ? "text-red-700 peer-focus:text-red-700  "
                      : ""
                  }absolute -top-2.5 left-2 bg-[#ffffff]   px-1 text-sm capitalize text-gray-700 transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-500 peer-focus:-top-2.5 peer-focus:text-sm peer-focus:text-gray-700`}
                  htmlFor={fields.password.id}
                >
                  password
                </label>
              </div>

              <span
                className={tw`${
                  fields.password.errors?.length ? "opacity-100" : "opacity-0"
                }    self-start  pl-1 text-sm text-red-700  transition-opacity duration-300 ease-in-out `}
                id={fields.password.errorId}
              >
                {fields.password.error}
              </span>
            </div>

            <div className=" mt-8 flex w-full flex-col items-center gap-[6px]">
              <div className="relative w-full ">
                <input
                  className={tw`${
                    fields.passwordConfirmation.errors?.length
                      ? "border-red-700 focus:border-red-700  "
                      : ""
                  }   peer block w-full rounded-md border  border-gray-500 bg-[#ffffff] px-3 py-[14px] text-gray-700  placeholder-transparent  focus:border-gray-700  focus:outline-none`}
                  {...conform.input(fields.passwordConfirmation, {
                    type: passwordConfirmationInputType,
                  })}
                  placeholder="email@example.com"
                />
                <PasswordRevealBtn
                  togglePasswordConfirmation={
                    passwordReveal.togglePasswordConfirmation
                  }
                  showPasswordConfirmation={
                    passwordReveal.showPasswordConfirmation
                  }
                />
                <label
                  className={tw`${
                    fields.passwordConfirmation.errors?.length
                      ? "text-red-700 peer-focus:text-red-700  "
                      : ""
                  }absolute -top-2.5 left-2 bg-[#ffffff] px-1 text-sm   capitalize text-gray-700 transition-all peer-placeholder-shown:top-3.5 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-base peer-placeholder-shown:text-gray-500 peer-focus:-top-2.5 peer-focus:text-sm peer-focus:text-gray-700 md:peer-placeholder-shown:top-3.5 md:peer-placeholder-shown:text-[1.1rem] md:peer-focus:-top-2.5 md:peer-focus:text-sm`}
                  htmlFor={fields.passwordConfirmation.id}
                >
                  Password confirmation
                </label>
              </div>

              <span
                className={tw`${
                  fields.passwordConfirmation.errors?.length
                    ? "opacity-100"
                    : "opacity-0"
                }    self-start  pl-1 text-sm text-red-700  transition-opacity duration-300 ease-in-out `}
                id={fields.passwordConfirmation.errorId}
              >
                {fields.passwordConfirmation.error}
              </span>
            </div>
          </fieldset>
          <button
            className="mt-6 h-[54px] w-full rounded-md bg-gray-700 px-4 py-2 text-lg capitalize text-white outline-none hover:bg-gray-600 focus:outline-none focus:ring focus:ring-gray-500 focus:ring-offset-2"
            type="submit"
          >
            reset password
          </button>
        </Form>
      </div>
    </main>
  );
}
