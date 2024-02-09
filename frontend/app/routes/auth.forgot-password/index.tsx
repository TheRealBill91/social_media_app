import { conform, useForm } from "@conform-to/react";
import { parse } from "@conform-to/zod";
import { ActionFunctionArgs, MetaFunction, json } from "@remix-run/cloudflare";
import { useActionData, useFetcher, useNavigation } from "@remix-run/react";
import { z } from "zod";
import { tw } from "~/utils/tw-identity-helper";
import { useId } from "react";
import { requestPasswordReset } from "./request-password-reset.server";

export const meta: MetaFunction = () => {
  return [{ title: "Disengage | Password recovery" }];
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
  const formData = await request.formData();

  const email = String(formData.get("email"));
  const state = String(formData.get("state"));

  if (state === "submitting") return null;

  const submission = parse(formData, { schema: forgotPasswordSchema });

  if (submission.intent !== "submit" || !submission.value) {
    return json(submission);
  }

  const requestPasswordResetResult = await requestPasswordReset(context, email);

  if (!requestPasswordResetResult.ok) {
    const passwordResetRequestError: { ErrorMessage: string } =
      await requestPasswordResetResult.json();
    if (requestPasswordResetResult.status === 429) {
      // need to return this error (as json) and render it on the page,
      // as it tells the user they have hit the daily limit of the number of password
      // reset requests they're able to send in
    }

    // if error is not rate limit, it must be that not account
    // can be found
    return json(passwordResetRequestError);
  }
}

export default function ForgotPassword() {
  const navigation = useNavigation();

  const forgotPassword = useFetcher<typeof action>();

  const id = useId();

  const actionData = useActionData<typeof action>();

  const lastSubmission =
    actionData && "intent" in actionData ? actionData : null;

  //   const requestPasswordResetError =
  //     actionData && "ErrorMessage" in actionData ? actionData : null;

  const [form, fields] = useForm({
    id,
    lastSubmission,
    shouldValidate: "onBlur",

    onValidate({ formData }) {
      return parse(formData, { schema: forgotPasswordSchema });
    },
  });

  return (
    <main className="flex min-h-screen flex-col items-center justify-center gap-5 bg-gray-100 px-8 py-12 md:p-12">
      <div className="mx-auto flex w-full max-w-[24rem] flex-col justify-start gap-6 rounded-lg border border-white bg-white p-6 px-8 py-6 shadow-md md:px-12">
        <h2 className="mt-4 text-center text-2xl">Forgot Password?</h2>
        <div className="flex flex-col items-center">
          <forgotPassword.Form {...form.props} className="w-full">
            <input type="hidden" name="state" value={navigation.state} />
            <fieldset className="mt-5">
              <div className="mb-4 flex flex-col items-center gap-[6px]">
                <div className="relative w-full">
                  <input
                    className={tw`${
                      fields.email.errors?.length
                        ? "border-red-700 focus:border-red-700  "
                        : ""
                    }signupInputAutofill peer block w-full rounded-md  border border-gray-500 bg-white px-3 py-[14px]  text-gray-700 placeholder-transparent  focus:border-gray-700  focus:outline-none`}
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
                        ? "text-red-700 peer-focus:text-red-700  "
                        : ""
                    }absolute -top-2.5 left-2 bg-white px-1 text-sm capitalize text-gray-700 transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-600 peer-focus:-top-2.5 peer-focus:text-sm peer-focus:text-gray-700`}
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
              className="h-[54px] w-full rounded-md bg-gray-700 px-4 py-2 text-lg capitalize text-white outline-none hover:bg-gray-600 focus:outline-none focus:ring focus:ring-gray-500 focus:ring-offset-2"
              type="submit"
            >
              recover email
            </button>
          </forgotPassword.Form>
        </div>
      </div>
    </main>
  );
}
