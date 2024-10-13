import {
  ActionFunctionArgs,
  LoaderFunctionArgs,
  MetaFunction,
  json,
  redirect,
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
import { useForm } from "@conform-to/react";
import { getFieldsetConstraint, parse } from "@conform-to/zod";
import { resetPassword } from "./reset-password-action.server";
import { redirectWithSuccessToast } from "~/utils/flash-session/flash-session.server";
import { z } from "zod";
import { RevealInputField } from "~/components/Forms";
import { StatusButton } from "~/components/ui/StatusButton";
import { useIsPending } from "~/utils/misc";

export const meta: MetaFunction = () => {
  return [{ title: "Disengage | Password Reset" }];
};

const ResetPasswordSchema = PasswordAndConfirmPasswordSchema;

export async function action({ request, context }: ActionFunctionArgs) {
  requireAnonymous(request);
  const { env } = context.cloudflare;
  const formData = await request.formData();

  const state = String(formData.get("state"));

  if (state === "submitting") return null;

  const submission = await parse(formData, {
    schema: (intent) =>
      ResetPasswordSchema.transform(async (data, ctx) => {
        if (intent !== "submit")
          return { ...data, resetPasswordResponse: null };

        const resetPasswordResponse = await resetPassword(env, formData);

        if (!resetPasswordResponse.ok && resetPasswordResponse.status === 409) {
          // password already exists

          const resetPasswordError: { Message: string } =
            await resetPasswordResponse.json();

          ctx.addIssue({
            code: z.ZodIssueCode.custom,
            message: resetPasswordError.Message,
            path: ["password"],
          });
          return z.NEVER;
        }

        return { ...data, resetPasswordResponse };
      }),
    async: true,
  });

  if (submission.intent !== "submit") {
    return json({ status: "idle", submission } as const);
  }

  if (!submission.value?.resetPasswordResponse) {
    return json({ status: "error", submission } as const);
  }

  const { resetPasswordResponse } = submission.value;

  if (
    !submission.value?.resetPasswordResponse &&
    resetPasswordResponse.status === 400
  ) {
    // Because of the validate reset password token endpoint, and the new password
    // validation in this action, this point should not be reached
    throw new Response("", {
      status: 500,
      statusText: "We ran into an issue resetting the password",
    });
  }

  const resetPasswordSuccessMessage: { SuccessMessage: string } =
    await resetPasswordResponse.json();

  return redirectWithSuccessToast(
    "/auth/login",
    resetPasswordSuccessMessage.SuccessMessage,
    env,
  );
}

export async function loader({ request, context }: LoaderFunctionArgs) {
  requireAnonymous(request);
  const { env } = context.cloudflare;
  const cookieHeaders = request.headers.get("Cookie");

  const cookieObj = cookieParse(cookieHeaders || "");

  // if either the password reset user id or the password reset code
  // is missing, redirect to the login page. This should only happen
  // if the user tries to access this route without going through
  // the password reset email link
  if (!cookieObj["PasswordResetUserId"] || !cookieObj["Code"]) {
    throw redirect("/auth/login");
  }

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

  const actionData = useActionData<typeof action>();

  const navigation = useNavigation();

  const isPending = useIsPending();

  const [form, fields] = useForm({
    id: "reset-password-form",
    lastSubmission: actionData?.submission,
    constraint: getFieldsetConstraint(ResetPasswordSchema),
    shouldValidate: "onBlur",
    shouldRevalidate: "onSubmit",

    onValidate({ formData }) {
      return parse(formData, { schema: ResetPasswordSchema });
    },
  });

  return (
    <main className="flex flex-1 flex-col items-center justify-center gap-5 bg-gray-100 px-8 py-12 md:p-12">
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
