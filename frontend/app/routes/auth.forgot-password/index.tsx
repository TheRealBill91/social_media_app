import { conform, useForm } from "@conform-to/react";
import { parse } from "@conform-to/zod";
import { ActionFunctionArgs, MetaFunction, json } from "@remix-run/cloudflare";
import { useFetcher, useNavigation } from "@remix-run/react";
import { z } from "zod";
import { tw } from "~/utils/tw-identity-helper";
import { useId } from "react";
import { requestPasswordReset } from "./request-password-reset.server";
import { ErrorAlertBanner } from "./ErrorAlertBanner";

export const meta: MetaFunction = () => {
  return [{ title: "Disengage | Password Recovery" }];
};

const email = z
  .string({ required_error: "Email is required" })
  .email({
    message:
      "Please enter a valid email address in the format: example@domain.com.",
  })
  .min(3, { message: "Email is too short " })
  .max(50, { message: "Email is too long" });

export const forgotPasswordSchema = z.object({ email: email });

export async function action({ request, context }: ActionFunctionArgs) {
  const { env } = context.cloudflare;
  const formData = await request.formData();

  const email = String(formData.get("email"));
  const state = String(formData.get("state"));

  if (state === "submitting") return null;

  const submission = parse(formData, { schema: forgotPasswordSchema });

  if (submission.intent !== "submit" || !submission.value) {
    return json(submission);
  }

  const requestPasswordResetResult = await requestPasswordReset(env, email);

  if (
    !requestPasswordResetResult.ok &&
    requestPasswordResetResult.status === 429
  ) {
    const passwordResetRequestError: { DailyLimitMessage: string } =
      await requestPasswordResetResult.json();

    return json(passwordResetRequestError);
  } else if (
    !requestPasswordResetResult.ok &&
    requestPasswordResetResult.status === 404
  ) {
    const passwordResetRequestError: { NoAccountFoundMessage: string } =
      await requestPasswordResetResult.json();

    return json(passwordResetRequestError);
  }

  const passwordResetRequestResult: { ResponseMessage: string } =
    await requestPasswordResetResult.json();
  return json(passwordResetRequestResult);
}

export default function ForgotPassword() {
  const navigation = useNavigation();

  const forgotPassword = useFetcher<typeof action>();

  const id = useId();

  const submissionData = forgotPassword.data;

  const lastSubmission =
    submissionData && "intent" in submissionData ? submissionData : null;

  const dailyLimitMessage =
    submissionData && "DailyLimitMessage" in submissionData
      ? submissionData
      : null;

  const noAccountFoundMessage =
    submissionData && "NoAccountFoundMessage" in submissionData
      ? submissionData
      : null;

  const requestPasswordResetSuccess =
    submissionData && "ResponseMessage" in submissionData
      ? submissionData
      : null;

  const requestPasswordResetError =
    dailyLimitMessage?.DailyLimitMessage ||
    noAccountFoundMessage?.NoAccountFoundMessage;

  // used to dynamically change the error alert callout font size
  const alertTitleFontSize = dailyLimitMessage
    ? tw`text-sm`
    : noAccountFoundMessage
      ? tw`text-lg`
      : null;

  const [form, fields] = useForm({
    id,
    lastSubmission,
    shouldValidate: "onBlur",

    onValidate({ formData }) {
      return parse(formData, { schema: forgotPasswordSchema });
    },
  });

  return (
    <main className="flex flex-1 flex-col items-center justify-center gap-5 bg-gray-100 px-8 py-12 md:p-12">
      {requestPasswordResetError ? (
        <ErrorAlertBanner
          alertTitleFontSize={alertTitleFontSize}
          requestPasswordResetError={requestPasswordResetError}
        />
      ) : requestPasswordResetSuccess ? (
        <div className="w-full max-w-[24rem] p-3 pb-0">
          <p className="text-sm ">
            {requestPasswordResetSuccess.ResponseMessage}
          </p>
        </div>
      ) : null}
      <div className="mx-auto flex w-full max-w-[24rem] flex-col justify-start gap-6 rounded-lg border border-white bg-white p-6 px-8 py-6 shadow-md md:px-12">
        <h2 className="mt-4 text-center text-2xl capitalize">
          forgot password?
        </h2>

        <div className="flex flex-col items-center">
          <forgotPassword.Form method="post" {...form.props} className="w-full">
            <input type="hidden" name="state" value={navigation.state} />
            <fieldset>
              <Field
                labelProps={{
                  children: "email",
                  baseClass:
                    "absolute -top-2.5 font-normal left-2 bg-[#ffffff] text-sm px-1 capitalize transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-400 peer-focus-visible:-top-2.5 peer-focus-visible:text-sm peer-focus-visible:text-gray-700",
                  errorsClass: "text-red-700 peer-focus-visible:text-red-700",
                }}
                inputProps={{
                  field: fields.email,
                  placeholder: "placeholder",
                  baseClass:
                    "signupInputAutofill border-color-[unset] placeholder:text-transparent focus-visible:ring-color-[unset] peer block h-[unset] w-full rounded-md border border-gray-400 bg-[#ffffff] px-3 py-[14px] text-base text-[unset] text-gray-700 placeholder-transparent ring-[unset] focus-visible:border-gray-700 focus-visible:outline-none focus-visible:ring-0 focus-visible:ring-offset-0",
                  inputErrorsClass:
                    "border-red-700 caret-red-700 focus-visible:border-red-700",
                }}
                errorProps={{
                  errors: fields.email.errors,
                  errorClass:
                    "self-start pl-1 text-sm text-red-700 transition-opacity duration-300 ease-in-out",
                }}
                className="mb-4 mt-2"
              />
            </fieldset>
            <div className="flex flex-1 items-center justify-between gap-6">
              <StatusButton
                className="h-[54px] w-full flex-1 gap-4 px-3 py-[14px] text-lg capitalize"
                status={
                  forgotPassword.state === "submitting"
                    ? "pending"
                    : forgotPassword.data?.status ?? "idle"
                }
                type="submit"
                disabled={forgotPassword.state !== "idle"}
              >
                recover password
              </StatusButton>
            </div>
          </forgotPassword.Form>
        </div>
      </div>
    </main>
  );
}
