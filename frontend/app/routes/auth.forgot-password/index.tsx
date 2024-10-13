import { useForm } from "@conform-to/react";
import { getFieldsetConstraint, parse } from "@conform-to/zod";
import { ActionFunctionArgs, MetaFunction, json } from "@remix-run/cloudflare";
import { useFetcher, useNavigation } from "@remix-run/react";
import { z } from "zod";
import { requestPasswordReset } from "./request-password-reset.server";
import { ErrorAlertBanner } from "./ErrorAlertBanner";
import { redirectWithSuccessToast } from "~/utils/flash-session/flash-session.server";
import { Field } from "~/components/Forms";
import { StatusButton } from "~/components/ui/StatusButton";
import { emailSchema } from "../auth.signup/signup-schema";

export const meta: MetaFunction = () => {
  return [{ title: "Disengage | Password Recovery" }];
};

export const forgotPasswordSchema = z.object({ email: emailSchema });

export async function action({ request, context }: ActionFunctionArgs) {
  const { env } = context.cloudflare;
  const formData = await request.formData();

  const state = String(formData.get("state"));

  if (state === "submitting") return null;

  const submission = parse(formData, { schema: forgotPasswordSchema });

  if (submission.intent !== "submit") {
    return json({ status: "idle", submission } as const);
  }

  if (!submission.value) {
    return json({ status: "error", submission } as const, { status: 400 });
  }

  const requestPasswordResetResult = await requestPasswordReset(env, formData);

  if (requestPasswordResetResult.status === 429) {
    const passwordResetRequestError: { DailyLimitMessage: string } =
      await requestPasswordResetResult.json();
    submission.error[""] = [passwordResetRequestError.DailyLimitMessage];

    return json({ status: "error", submission } as const, { status: 429 });
  } else if (requestPasswordResetResult.status === 404) {
    const passwordResetRequestError: { NoAccountFoundMessage: string } =
      await requestPasswordResetResult.json();
    submission.error[""] = [passwordResetRequestError.NoAccountFoundMessage];

    return json({ status: "error", submission } as const, { status: 404 });
  }

  const passwordResetRequestResult: { ResponseMessage: string } =
    await requestPasswordResetResult.json();

  return await redirectWithSuccessToast(
    "/auth/login",
    passwordResetRequestResult.ResponseMessage,
    env,
  );
}

export default function ForgotPassword() {
  const navigation = useNavigation();

  const forgotPassword = useFetcher<typeof action>();

  forgotPassword.data;

  const [form, fields] = useForm({
    id: "forgot-password-form",
    constraint: getFieldsetConstraint(forgotPasswordSchema),
    lastSubmission: forgotPassword.data?.submission,
    shouldValidate: "onBlur",

    onValidate({ formData }) {
      return parse(formData, { schema: forgotPasswordSchema });
    },
  });

  return (
    <main className="flex flex-1 flex-col items-center justify-center gap-5 bg-gray-100 px-8 py-12 md:p-12">
      {form.error ? (
        <ErrorAlertBanner requestPasswordResetError={form.error} />
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
