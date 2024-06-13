import { ActionFunctionArgs, MetaFunction, json } from "@remix-run/cloudflare";
import { useFetcher, useNavigation } from "@remix-run/react";
import { useId } from "react";
import { z } from "zod";
import { parse } from "@conform-to/zod";
import { tw } from "~/utils/tw-identity-helper";
import { conform, useForm } from "@conform-to/react";
import { requestUsername } from "./request-username-reset.server";
import { redirectWithSuccessToast } from "~/utils/flash-session/flash-session.server";
import { default as AlertCircle } from "~/components/icons/icon.tsx";
import { Alert, AlertTitle } from "~/components/ui/Alert";

export const meta: MetaFunction = () => {
  return [{ title: "Disengage | Username Recovery" }];
};

const email = z
  .string({ required_error: "Email is required" })
  .email({
    message:
      "Please enter a valid email address in the format: example@domain.com.",
  })
  .min(3, { message: "Email is too short " })
  .max(50, { message: "Email is too long" });

const requestUsernameSchema = z.object({ email: email });

export async function action({ request, context }: ActionFunctionArgs) {
  const { env } = context.cloudflare;
  const formData = await request.formData();

  const email = String(formData.get("email"));
  const state = String(formData.get("state"));

  if (state === "submitting") return null;

  const submission = parse(formData, { schema: requestUsernameSchema });

  if (submission.intent !== "submit" || !submission.value) {
    return json(submission);
  }

  const requestUsernameResult = await requestUsername(env, email);

  if (!requestUsernameResult.ok && requestUsernameResult.status === 429) {
    const requestUsernameError: { DailyLimitMessage: string } =
      await requestUsernameResult.json();

    return json(requestUsernameError);
  } else if (
    !requestUsernameResult.ok &&
    requestUsernameResult.status === 404
  ) {
    const requestUsernameError: { NoAccountFoundMessage: string } =
      await requestUsernameResult.json();
    return json(requestUsernameError);
  }

  const requestUsernameResultMessage: { ResponseMessage: string } =
    await requestUsernameResult.json();

  return redirectWithSuccessToast(
    "/auth/login",
    requestUsernameResultMessage.ResponseMessage,
    env,
    undefined,
    8000,
  );
}

export default function ForgotUsername() {
  const navigation = useNavigation();

  const forgotUsername = useFetcher<typeof action>();

  const id = useId();

  const submissionData = forgotUsername.data;

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

  const requestUsernameError =
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
      return parse(formData, { schema: requestUsernameSchema });
    },
  });

  return (
    <main className="flex flex-1 flex-col items-center justify-center gap-5 bg-gray-100 px-8 py-12 md:p-12">
      {requestUsernameError ? (
        <Alert className="md-shadow max-w-[24rem] bg-white">
          <AlertCircle
            icon="alert-circle"
            className="size-[22px] fill-red-400"
          />
          <AlertTitle className={tw`pb-0 ${alertTitleFontSize}`}>
            {requestUsernameError}
          </AlertTitle>
        </Alert>
      ) : null}

      <div className="mx-auto flex w-full max-w-[24rem] flex-col justify-start gap-6 rounded-lg border border-white bg-white p-6 px-8 py-6 shadow-md md:px-12">
        <h2 className="mt-4 text-center text-2xl capitalize">
          forgot username?
        </h2>

        <div className="flex flex-col items-center">
          <forgotUsername.Form method="post" {...form.props} className="w-full">
            <input type="hidden" name="state" value={navigation.state} />
            <fieldset className="mt-5">
              <div className="mb-4 flex flex-col items-center gap-[6px]">
                <div className="relative w-full">
                  <input
                    className={tw`${
                      fields.email.errors?.length
                        ? "border-red-700 focus-visible:border-red-700"
                        : ""
                    } signupInputAutofill peer block w-full rounded-md  border border-gray-500 bg-white px-3 py-[14px]  text-gray-700 placeholder-transparent  focus-visible:border-gray-700  focus-visible:outline-none`}
                    {...conform.input(fields.email, {
                      type: "email",
                    })}
                    placeholder="placeholder"
                    autoComplete="on"
                  />
                  <label
                    htmlFor={fields.email.id}
                    className={tw`${
                      fields.email.errors?.length
                        ? "text-red-700 peer-focus-visible:text-red-700"
                        : ""
                    } absolute -top-2.5 left-2 bg-white px-1 text-sm capitalize text-gray-700 transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-600 peer-focus-visible:-top-2.5 peer-focus-visible:text-sm peer-focus-visible:text-gray-700`}
                  >
                    email
                  </label>
                </div>

                <span
                  className={tw`${
                    fields.email.errors?.length ? "opacity-100" : "opacity-0"
                  }    mb-2  self-start pl-1 text-sm text-red-700  transition-opacity duration-300 ease-in-out`}
                  id={fields.email.errorId}
                >
                  {fields.email.error}
                </span>
              </div>
            </fieldset>
            <button
              className="h-[54px] w-full rounded-md bg-gray-700 px-4 py-2 text-lg capitalize text-white outline-none hover:bg-gray-600 focus-visible:outline-none focus-visible:ring focus-visible:ring-gray-500 focus-visible:ring-offset-2"
              type="submit"
            >
              recover username
            </button>
          </forgotUsername.Form>
        </div>
      </div>
    </main>
  );
}
