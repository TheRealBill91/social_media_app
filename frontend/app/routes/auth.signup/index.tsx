import { Form, useActionData, useNavigation } from "@remix-run/react";
import { signUpSchema } from "../../../zod/signup-schema";
import { Submission, conform, useForm } from "@conform-to/react";
import { parse } from "@conform-to/zod";
import { ActionFunctionArgs, json } from "@remix-run/cloudflare";
import { BackButton } from "../../components/ui/BackButton";
import { tw } from "../../utils/tw-identity-helper";
import { AuthButton } from "../../components/ui/AuthButton";
import { useId } from "react";
import * as Checkbox from "@radix-ui/react-checkbox";
import { default as Check } from "../../components/icons/icon.tsx";
import { createAccount } from "./create-account.server.ts";

export async function action({ request, context }: ActionFunctionArgs) {
  const formData = await request.formData();
  const username = String(formData.get("username"));
  const email = String(formData.get("email"));
  const firstName = String(formData.get("firstName"));
  const lastName = String(formData.get("lastName"));
  const password = String(formData.get("password"));
  const confirmPassword = String(formData.get("confirmPassword"));

  const submission = parse(formData, { schema: signUpSchema });

  const signUpResponse = await createAccount(
    context,
    username,
    email,
    firstName,
    lastName,
    password,
    confirmPassword,
  );

  if (!signUpResponse.ok) {
    const serverErrors: Record<string, string[]> = await signUpResponse.json();

    // Create an object that matches the SubmissionResult type
    const submissionResult: Submission = {
      intent: submission.intent,
      payload: submission.payload,
      error: serverErrors,
    };

    return submissionResult;
  }

  if (submission.intent !== "submit" || !submission.value) {
    return json(submission);
  }
}

export default function Signup() {
  const navigation = useNavigation();
  const submitting = navigation.state === "submitting";

  const signUpButtonName = submitting ? "Signing up..." : "Sign up";

  const lastSubmission = useActionData<typeof action>();
  console.log("last submission: " + lastSubmission);
  console.log(JSON.stringify(lastSubmission, null, 2));

  const id = useId();

  const [form, fields] = useForm({
    id,
    lastSubmission,
    shouldValidate: "onBlur",

    onValidate({ formData }) {
      return parse(formData, { schema: signUpSchema });
    },
  });

  console.log(fields.password);

  const navTo = "/auth";

  /*   const navigate = useNavigate();
  const location = useLocation();
 */

  return (
    <main className="flex min-h-screen flex-1 flex-col items-center   gap-10 bg-slate-50 px-8  py-16 dark:bg-gray-800">
      <BackButton navTo={navTo} />
      <div className="flex w-full flex-col justify-start rounded-lg bg-gray-300 p-6 sm:max-w-md sm:px-10">
        <h1 className="my-3 text-center text-2xl font-bold capitalize text-gray-700 dark:text-slate-100 ">
          sign up
        </h1>
        <span className="my-4 self-center italic text-gray-700">
          All fields are required
        </span>
        <div className=" flex  flex-col items-center">
          <Form
            className="flex w-full flex-col gap-6"
            replace
            method="post"
            {...form.props}
          >
            <fieldset className="mt-4">
              <div className="mb-4 flex w-full flex-col items-center gap-[6px] px-3">
                <label className="self-start" htmlFor="firstName">
                  First name
                </label>

                <input
                  className="w-full rounded-md border border-gray-400 bg-gray-100 px-2  py-[6px]"
                  {...conform.input(fields.firstName, { type: "text" })}
                />
                <span
                  className={tw`${
                    fields.firstName.errors?.length
                      ? "opacity-100"
                      : "opacity-0"
                  }    self-start  text-red-700 transition-opacity duration-300 ease-in-out dark:text-slate-100 dark:underline dark:decoration-red-700 dark:underline-offset-[5px]`}
                  id={fields.firstName.errorId}
                >
                  {fields.firstName.errors}
                </span>
              </div>

              <div className="mb-5 flex flex-col items-center gap-[6px] px-3 ">
                <label className="flex-start self-start" htmlFor="lastName">
                  Last name
                </label>
                <input
                  className="w-full rounded-md border border-gray-400 bg-gray-100 px-2  py-[6px]"
                  {...conform.input(fields.lastName, { type: "text" })}
                />
                <span
                  className={tw`${
                    fields.lastName.errors?.length ? "opacity-100" : "opacity-0"
                  }    self-start  text-red-700 transition-opacity duration-300 ease-in-out dark:text-slate-100 dark:underline dark:decoration-red-700 dark:underline-offset-[5px]`}
                  id={fields.lastName.errorId}
                >
                  {fields.lastName.errors}
                </span>
              </div>
              <div className="mb-6 flex flex-col items-center gap-[6px] px-3">
                <label
                  className="flex-start self-start capitalize"
                  htmlFor="username"
                >
                  username
                </label>
                <input
                  className="w-full rounded-md border border-gray-400 bg-gray-100 px-2  py-[6px]"
                  {...conform.input(fields.username, { type: "text" })}
                />
                <span
                  className={tw`${
                    fields.username.errors?.length ? "opacity-100" : "opacity-0"
                  }    self-start  text-red-700 transition-opacity duration-300 ease-in-out dark:text-slate-100 dark:underline dark:decoration-red-700 dark:underline-offset-[5px]`}
                  id={fields.username.errorId}
                >
                  {fields.username.errors}
                </span>
              </div>

              <div className="mb-6 flex flex-col items-center gap-[6px] px-3">
                <label className="self-start capitalize" htmlFor="email">
                  email
                </label>
                <input
                  className="w-full rounded-md border border-gray-400 bg-gray-100 px-2  py-[6px]"
                  {...conform.input(fields.email, { type: "email" })}
                />
                <span
                  className={tw`${
                    fields.email.errors?.length ? "opacity-100" : "opacity-0"
                  }    self-start  text-red-700 transition-opacity duration-300 ease-in-out dark:text-slate-100 dark:underline dark:decoration-red-700 dark:underline-offset-[5px]`}
                  id={fields.email.errorId}
                >
                  {fields.email.errors}
                </span>
              </div>
              <div className="mb-6 flex flex-col items-center gap-[6px] px-3">
                <label className="self-start capitalize" htmlFor="password">
                  password
                </label>
                <input
                  className="w-full rounded-md border border-gray-400 bg-gray-100 px-2  py-[6px]"
                  {...conform.input(fields.password, { type: "password" })}
                />
                <span
                  className={tw`${
                    fields.password.errors?.length ? "opacity-100" : "opacity-0"
                  }    self-start  text-red-700 transition-opacity duration-300 ease-in-out dark:text-slate-100 dark:underline dark:decoration-red-700 dark:underline-offset-[5px]`}
                  id={fields.password.errorId}
                >
                  {fields.password.errors}
                </span>
              </div>

              <div className="mb-2 flex flex-col items-center gap-[6px] px-3">
                <label
                  className="self-start capitalize"
                  htmlFor="confirmPassword"
                >
                  Password confirmation
                </label>
                <input
                  className="w-full rounded-md border border-gray-400 bg-gray-100 px-2  py-[6px]"
                  {...conform.input(fields.confirmPassword, {
                    type: "password",
                  })}
                />
                <span
                  className={tw`${
                    fields.confirmPassword.errors?.length
                      ? "opacity-100"
                      : "opacity-0"
                  }    self-start  text-red-700 transition-opacity duration-300 ease-in-out dark:text-slate-100 dark:underline dark:decoration-red-700 dark:underline-offset-[5px]`}
                  id={fields.confirmPassword.errorId}
                >
                  {fields.confirmPassword.errors}
                </span>
              </div>
            </fieldset>
            <div className="flex justify-center gap-8 px-2">
              <div className="flex items-center">
                <Checkbox.Root
                  className="flex h-[22px] w-[22px] appearance-none items-center justify-center rounded-[4px] bg-white shadow-[0_2px_10px] shadow-blackA4 outline-none hover:bg-violet3  [&:focus-visible]:shadow-[0_0_0_2px_black] "
                  id="c1"
                >
                  <Checkbox.Indicator className=" text-black">
                    <Check icon="check" className="h-[18px] w-[18px]" />
                  </Checkbox.Indicator>
                </Checkbox.Root>
                <label
                  className="pl-[10px] text-[15px] leading-none text-black"
                  htmlFor="c1"
                >
                  Remember me?
                </label>
              </div>

              <AuthButton name={signUpButtonName} submitting={submitting} />
            </div>
          </Form>
        </div>
      </div>
    </main>
  );
}
