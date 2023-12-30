import { Form, useActionData, useNavigation } from "@remix-run/react";
import { signUpSchema } from "../../../zod/signup-schema";
import { Submission, conform, useForm } from "@conform-to/react";
import { parse } from "@conform-to/zod";
import { ActionFunctionArgs, json } from "@remix-run/cloudflare";
import { BackButton } from "../../components/ui/BackButton";
import { tw } from "../../utils/tw-identity-helper";
import { AuthButton } from "../../components/ui/AuthButton";
import { useId, useState } from "react";
import * as Checkbox from "@radix-ui/react-checkbox";
import {
  default as Check,
  default as EyeOpen,
  default as EyeNone,
} from "../../components/icons/icon.tsx";
import { createAccount } from "./create-account.server.ts";
import { transformErrors } from "./transform-errors.server.ts";

export async function action({ request, context }: ActionFunctionArgs) {
  const formData = await request.formData();
  const username = String(formData.get("username"));
  const email = String(formData.get("email"));
  const firstName = String(formData.get("firstName"));
  const lastName = String(formData.get("lastName"));
  const password = String(formData.get("password"));
  const passwordConfirmation = String(formData.get("passwordConfirmation"));

  const submission = parse(formData, { schema: signUpSchema });

  const signUpResponse = await createAccount(
    context,
    username,
    email,
    firstName,
    lastName,
    password,
    passwordConfirmation,
  );

  if (!signUpResponse.ok) {
    const serverErrors: Record<string, string[]> = await signUpResponse.json();

    const transformedErrors = transformErrors(serverErrors);

    console.log("Server errors: " + JSON.stringify(serverErrors, null, 2));

    console.log(
      "Zod server errors: " + JSON.stringify(submission.error, null, 2),
    );

    // Create an object that matches the SubmissionResult type
    const submissionResult: Submission = {
      intent: submission.intent,
      payload: submission.payload,
      error: transformedErrors,
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

  const [showPassword, setShowPassword] = useState(false);
  const [showPasswordConfirmation, setShowPasswordConfirmation] =
    useState(false);

  const togglePasswordReveal = (passwordType: string) => {
    if (passwordType === "password") {
      setShowPassword(!showPassword);
    } else if (passwordType === "passwordConfirmation") {
      setShowPasswordConfirmation(!showPasswordConfirmation);
    }
  };

  const passwordInputType = showPassword ? "text" : "password";
  const passwordConfirmationInputType = showPasswordConfirmation
    ? "text"
    : "password";

  const signUpButtonName = submitting ? "Signing up..." : "Sign up";

  const lastSubmission = useActionData<typeof action>();
  lastSubmission ? console.log(JSON.stringify(lastSubmission, null, 2)) : null;
  lastSubmission ? console.log(lastSubmission?.error) : null;

  const id = useId();
  console.log("test");

  const [form, fields] = useForm({
    id,
    lastSubmission,
    shouldValidate: "onInput",

    onValidate({ formData }) {
      return parse(formData, { schema: signUpSchema });
    },
  });

  const navTo = "/auth";

  /*   const navigate = useNavigate();
  const location = useLocation();
 */

  return (
    <main className="flex min-h-screen flex-1 flex-col items-center   gap-10 bg-slate-50 px-8  py-16 dark:bg-gray-800">
      <BackButton navTo={navTo} />
      <div className="flex w-full flex-col justify-start rounded-lg border border-gray-400 bg-slate-50 p-6 sm:max-w-md md:px-10">
        <h1 className="my-3 text-center text-[1.7rem] font-bold capitalize text-gray-700 dark:text-slate-100 ">
          sign up
        </h1>
        <span className="my-4 self-center italic text-gray-700">
          All fields are required
        </span>
        <div className=" flex  flex-col items-center">
          <Form
            className="flex w-full flex-col gap-4"
            replace
            method="post"
            {...form.props}
          >
            <fieldset className="mt-5">
              <div className="mb-4 flex w-full flex-col items-center gap-[6px] px-3">
                <div className="relative w-full ">
                  <input
                    className={tw`${
                      fields.firstName.errors?.length
                        ? "border-red-700 focus:border-red-700  "
                        : ""
                    }   signupInputAutofill peer block w-full rounded-md  border border-gray-500 bg-slate-50 px-3 py-[14px]  text-gray-700 placeholder-transparent  focus:border-gray-700  focus:outline-none`}
                    {...conform.input(fields.firstName, { type: "text" })}
                    placeholder="john"
                  />
                  <label
                    className={tw`${
                      fields.firstName.errors?.length
                        ? "text-red-700 peer-focus:text-red-700  "
                        : ""
                    }absolute -top-2.5 left-2   bg-slate-50 px-1 text-sm text-gray-700 transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-500 peer-focus:-top-2.5 peer-focus:text-sm peer-focus:text-gray-700`}
                    htmlFor={fields.firstName.id}
                  >
                    First name
                  </label>
                </div>

                <span
                  className={tw`${
                    fields.firstName.errors?.length
                      ? "opacity-100"
                      : "opacity-0"
                  }    self-start  pl-1 text-sm text-red-700  transition-opacity duration-300 ease-in-out dark:text-slate-100 dark:underline dark:decoration-red-700 dark:underline-offset-[5px]`}
                  id={fields.firstName.errorId}
                >
                  {fields.firstName.errors}
                </span>
              </div>

              <div className=" mt-8 flex w-full flex-col items-center gap-[6px] px-3">
                <div className="relative w-full ">
                  <input
                    className={tw`${
                      fields.lastName.errors?.length
                        ? "border-red-700 focus:border-red-700  "
                        : ""
                    }   signupInputAutofill peer block w-full rounded-md border  border-gray-500 bg-slate-50 px-3 py-[14px] text-gray-700  placeholder-transparent  focus:border-gray-700  focus:outline-none`}
                    {...conform.input(fields.lastName, { type: "text" })}
                    placeholder="appleseed"
                  />
                  <label
                    className={tw`${
                      fields.lastName.errors?.length
                        ? "text-red-700 peer-focus:text-red-700  "
                        : ""
                    }absolute -top-2.5 left-2   bg-slate-50 px-1 text-sm text-gray-700 transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-500 peer-focus:-top-2.5 peer-focus:text-sm peer-focus:text-gray-700`}
                    htmlFor={fields.lastName.id}
                  >
                    Last name
                  </label>
                </div>

                <span
                  className={tw`${
                    fields.lastName.errors?.length ? "opacity-100" : "opacity-0"
                  }    self-start  pl-1 text-sm text-red-700  transition-opacity duration-300 ease-in-out dark:text-slate-100 dark:underline dark:decoration-red-700 dark:underline-offset-[5px]`}
                  id={fields.lastName.errorId}
                >
                  {fields.lastName.errors}
                </span>
              </div>

              <div className=" mt-8 flex w-full flex-col items-center gap-[6px] px-3">
                <div className="relative w-full ">
                  <input
                    className={tw`${
                      fields.username.errors?.length
                        ? "border-red-700 focus:border-red-700  "
                        : ""
                    }   signupInputAutofill peer block w-full rounded-md border  border-gray-500 bg-slate-50 px-3 py-[14px] text-gray-700  placeholder-transparent  focus:border-gray-700  focus:outline-none`}
                    {...conform.input(fields.username, { type: "text" })}
                    placeholder="bob1234"
                  />
                  <label
                    className={tw`${
                      fields.username.errors?.length
                        ? "text-red-700 peer-focus:text-red-700  "
                        : ""
                    }absolute -top-2.5 left-2 bg-slate-50   px-1 text-sm capitalize text-gray-700 transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-500 peer-focus:-top-2.5 peer-focus:text-sm peer-focus:text-gray-700`}
                    htmlFor={fields.username.id}
                  >
                    username
                  </label>
                </div>

                <span
                  className={tw`${
                    fields.username.errors?.length ? "opacity-100" : "opacity-0"
                  }    self-start  pl-1 text-sm text-red-700  transition-opacity duration-300 ease-in-out dark:text-slate-100 dark:underline dark:decoration-red-700 dark:underline-offset-[5px]`}
                  id={fields.username.errorId}
                >
                  {fields.username.errors}
                </span>
              </div>

              <div className=" mt-8 flex w-full flex-col items-center gap-[6px] px-3">
                <div className="relative w-full ">
                  <input
                    className={tw`${
                      fields.email.errors?.length
                        ? "border-red-700 focus:border-red-700  "
                        : ""
                    }   signupInputAutofill peer block w-full rounded-md border  border-gray-500 bg-slate-50 px-3 py-[14px] text-gray-700  placeholder-transparent  focus:border-gray-700  focus:outline-none`}
                    {...conform.input(fields.email, { type: "email" })}
                    placeholder="email@example.com"
                  />
                  <label
                    className={tw`${
                      fields.email.errors?.length
                        ? "text-red-700 peer-focus:text-red-700  "
                        : ""
                    }absolute -top-2.5 left-2 bg-slate-50   px-1 text-sm capitalize text-gray-700 transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-500 peer-focus:-top-2.5 peer-focus:text-sm peer-focus:text-gray-700`}
                    htmlFor={fields.email.id}
                  >
                    email
                  </label>
                </div>

                <span
                  className={tw`${
                    fields.email.errors?.length ? "opacity-100" : "opacity-0"
                  }    self-start  pl-1 text-sm text-red-700  transition-opacity duration-300 ease-in-out dark:text-slate-100 dark:underline dark:decoration-red-700 dark:underline-offset-[5px]`}
                  id={fields.email.errorId}
                >
                  {fields.email.errors}
                </span>
              </div>

              <div className=" mt-8 flex w-full flex-col items-center gap-[6px] px-3">
                <div className="relative w-full ">
                  <input
                    className={tw`${
                      fields.password.errors?.length
                        ? "border-red-700 focus:border-red-700  "
                        : ""
                    }   signupInputAutofill peer block w-full rounded-md border  border-gray-500 bg-slate-50 px-3 py-[14px] text-gray-700  placeholder-transparent  focus:border-gray-700  focus:outline-none`}
                    {...conform.input(fields.password, {
                      type: passwordInputType,
                    })}
                    placeholder="email@example.com"
                  />
                  {showPassword ? (
                    <button
                      onClick={() => togglePasswordReveal("password")}
                      type="button"
                      className="absolute right-4 top-4"
                    >
                      <EyeNone
                        icon="eye-none"
                        className="size-[22px] text-gray-800"
                      />
                    </button>
                  ) : (
                    <button
                      onClick={() => togglePasswordReveal("password")}
                      type="button"
                      className="absolute right-4 top-4"
                    >
                      <EyeOpen
                        icon="eye-open"
                        className="size-[22px] text-gray-800"
                      />
                    </button>
                  )}
                  <label
                    className={tw`${
                      fields.password.errors?.length
                        ? "text-red-700 peer-focus:text-red-700  "
                        : ""
                    }absolute -top-2.5 left-2 bg-slate-50   px-1 text-sm capitalize text-gray-700 transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-500 peer-focus:-top-2.5 peer-focus:text-sm peer-focus:text-gray-700`}
                    htmlFor={fields.password.id}
                  >
                    password
                  </label>
                </div>

                <span
                  className={tw`${
                    fields.password.errors?.length ? "opacity-100" : "opacity-0"
                  }    self-start  pl-1 text-sm text-red-700  transition-opacity duration-300 ease-in-out dark:text-slate-100 dark:underline dark:decoration-red-700 dark:underline-offset-[5px]`}
                  id={fields.password.errorId}
                >
                  {fields.password.errors}
                </span>
              </div>

              <div className=" mt-8 flex w-full flex-col items-center gap-[6px] px-3">
                <div className="relative w-full ">
                  <input
                    className={tw`${
                      fields.passwordConfirmation.errors?.length
                        ? "border-red-700 focus:border-red-700  "
                        : ""
                    }   peer block w-full rounded-md border  border-gray-500 bg-slate-50 px-3 py-[14px] text-gray-700  placeholder-transparent  focus:border-gray-700  focus:outline-none`}
                    {...conform.input(fields.passwordConfirmation, {
                      type: passwordConfirmationInputType,
                    })}
                    placeholder="email@example.com"
                  />
                  {showPasswordConfirmation ? (
                    <button
                      onClick={() =>
                        togglePasswordReveal("passwordConfirmation")
                      }
                      type="button"
                      className="absolute right-4 top-4"
                    >
                      <EyeNone
                        icon="eye-none"
                        className="size-[22px] text-gray-800"
                      />
                    </button>
                  ) : (
                    <button
                      onClick={() =>
                        togglePasswordReveal("passwordConfirmation")
                      }
                      type="button"
                      className="absolute right-4 top-4"
                    >
                      <EyeOpen
                        icon="eye-open"
                        className="size-[22px] text-gray-800"
                      />
                    </button>
                  )}
                  <label
                    className={tw`${
                      fields.passwordConfirmation.errors?.length
                        ? "text-red-700 peer-focus:text-red-700  "
                        : ""
                    }absolute -top-2.5 left-2 bg-slate-50 px-1 text-sm   capitalize text-gray-700 transition-all peer-placeholder-shown:top-3.5 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-base peer-placeholder-shown:text-gray-500 peer-focus:-top-2.5 peer-focus:text-sm peer-focus:text-gray-700 md:peer-placeholder-shown:top-3 md:peer-placeholder-shown:text-[1.1rem] md:peer-focus:-top-2.5 md:peer-focus:text-sm`}
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
                  }    self-start  pl-1 text-sm text-red-700  transition-opacity duration-300 ease-in-out dark:text-slate-100 dark:underline dark:decoration-red-700 dark:underline-offset-[5px]`}
                  id={fields.passwordConfirmation.errorId}
                >
                  {fields.passwordConfirmation.errors}
                </span>
              </div>
            </fieldset>
            <div className="flex flex-col justify-center gap-4 px-3 lg:gap-8 lg:px-2">
              <div className=" my-3 flex items-center px-1 md:my-2 md:px-2">
                <Checkbox.Root
                  className="flex h-[22px] w-[22px] appearance-none items-center justify-center rounded-[4px] border border-gray-400 bg-white shadow-[0_2px_10px] shadow-blackA4 outline-none hover:bg-violet3  [&:focus-visible]:shadow-[0_0_0_2px_black] "
                  id="c1"
                >
                  <Checkbox.Indicator className=" text-gray-700">
                    <Check icon="check" className="h-[18px] w-[18px]" />
                  </Checkbox.Indicator>
                </Checkbox.Root>
                <label
                  className="pl-[10px] text-sm leading-none text-black md:text-base "
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
