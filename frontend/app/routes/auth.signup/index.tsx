import { Form, useActionData, useNavigation } from "@remix-run/react";
import { signUpSchema } from "~/routes/auth.signup/signup-schema.ts";
import { Submission, conform, useForm } from "@conform-to/react";
import { parse } from "@conform-to/zod";
import {
  ActionFunctionArgs,
  MetaFunction,
  json,
  redirect,
} from "@remix-run/cloudflare";
import { BackButton } from "~/components/ui/BackButton.tsx";
import { tw } from "~/utils/tw-identity-helper.ts";
import { AuthButton } from "~/components/ui/AuthButton.tsx";
import { useId, useState } from "react";
import {
  default as EyeOpen,
  default as EyeNone,
} from "~/components/icons/icon.tsx";
import { createAccount } from "./create-account.server.ts";
import { transformSignupErrors } from "./transform-signup-errors.server.ts";
import { postSignupEmail } from "~/utils/cookie.server.ts";

export const meta: MetaFunction = () => {
  return [{ title: "Disengage | Signup" }];
};

export async function action({ request, context }: ActionFunctionArgs) {
  const formData = await request.formData();
  const username = String(formData.get("username"));
  const email = String(formData.get("email"));
  const firstName = String(formData.get("firstName"));
  const lastName = String(formData.get("lastName"));
  const password = String(formData.get("password"));
  const passwordConfirmation = String(formData.get("passwordConfirmation"));

  const state = String(formData.get("state"));

  if (state === "submitting") return null;

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

    const transformedErrors = transformSignupErrors(serverErrors);

    //  Create an object that matches the SubmissionResult type
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

  const cookieHeader = request.headers.get("Cookie");
  const cookie = (await postSignupEmail.parse(cookieHeader)) || {};
  cookie.email = email;

  return redirect("/auth/signup/success", {
    headers: {
      "Set-Cookie": await postSignupEmail.serialize(cookie),
    },
  });
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

  const id = useId();

  const [form, fields] = useForm({
    id,
    lastSubmission,
    shouldValidate: "onInput",

    /*  onValidate({ formData }) {
      return parse(formData, { schema: signUpSchema });
    }, */
  });

  const navTo = "/";

  return (
    <main className="flex min-h-screen flex-col items-center gap-12 bg-[#ffffff] px-8 py-10 md:p-12">
      <BackButton navTo={navTo} twStyles="self-start" />
      <div className="flex w-full flex-col justify-start rounded-lg border border-gray-400 bg-[#ffffff] p-6 sm:max-w-md md:px-10">
        <h1 className="mt-3 text-center text-[2rem] font-bold capitalize text-gray-700">
          sign up
        </h1>
        <span className="my-2 self-center text-[1.1rem] italic text-gray-600">
          All fields are required
        </span>
        <div className="flex flex-col items-center">
          <Form
            className="flex w-full flex-col gap-4"
            replace
            method="post"
            {...form.props}
          >
            <input type="hidden" name="state" value={navigation.state}></input>
            <fieldset className="mt-5">
              <div className="mb-4 flex w-full flex-col items-center gap-[6px] px-3">
                <div className="relative w-full ">
                  <input
                    className={tw`${
                      fields.firstName.errors?.length
                        ? "border-red-700 focus:border-red-700  "
                        : ""
                    }   signupInputAutofill peer block w-full rounded-md  border border-gray-500 bg-[#ffffff] px-3 py-[14px]  text-gray-700 placeholder-transparent  focus:border-gray-700  focus:outline-none`}
                    {...conform.input(fields.firstName, { type: "text" })}
                    placeholder="john"
                  />
                  <label
                    className={tw`${
                      fields.firstName.errors?.length
                        ? "text-red-700 peer-focus:text-red-700  "
                        : ""
                    }absolute -top-2.5 left-2   bg-[#ffffff] px-1 text-sm text-gray-700 transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-500 peer-focus:-top-2.5 peer-focus:text-sm peer-focus:text-gray-700`}
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
                  }    self-start  pl-1 text-sm text-red-700  transition-opacity duration-300 ease-in-out `}
                  id={fields.firstName.errorId}
                >
                  {fields.firstName.errors}
                </span>
              </div>

              <div className="mt-8 flex w-full flex-col items-center gap-[6px] px-3">
                <div className="relative w-full ">
                  <input
                    className={tw`${
                      fields.lastName.errors?.length
                        ? "border-red-700 focus:border-red-700  "
                        : ""
                    }   signupInputAutofill peer block w-full rounded-md border  border-gray-500 bg-[#ffffff] px-3 py-[14px] text-gray-700  placeholder-transparent  focus:border-gray-700  focus:outline-none`}
                    {...conform.input(fields.lastName, { type: "text" })}
                    placeholder="appleseed"
                  />
                  <label
                    className={tw`${
                      fields.lastName.errors?.length
                        ? "text-red-700 peer-focus:text-red-700  "
                        : ""
                    }absolute -top-2.5 left-2   bg-[#ffffff] px-1 text-sm text-gray-700 transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-500 peer-focus:-top-2.5 peer-focus:text-sm peer-focus:text-gray-700`}
                    htmlFor={fields.lastName.id}
                  >
                    Last name
                  </label>
                </div>

                <span
                  className={tw`${
                    fields.lastName.errors?.length ? "opacity-100" : "opacity-0"
                  }    self-start  pl-1 text-sm text-red-700  transition-opacity duration-300 ease-in-out `}
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
                    }   signupInputAutofill peer block w-full rounded-md border  border-gray-500 bg-[#ffffff] px-3 py-[14px] text-gray-700  placeholder-transparent  focus:border-gray-700  focus:outline-none`}
                    {...conform.input(fields.username, { type: "text" })}
                    placeholder="bob1234"
                  />
                  <label
                    className={tw`${
                      fields.username.errors?.length
                        ? "text-red-700 peer-focus:text-red-700  "
                        : ""
                    }absolute -top-2.5 left-2 bg-[#ffffff]   px-1 text-sm capitalize text-gray-700 transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-500 peer-focus:-top-2.5 peer-focus:text-sm peer-focus:text-gray-700`}
                    htmlFor={fields.username.id}
                  >
                    username
                  </label>
                </div>

                <span
                  className={tw`${
                    fields.username.errors?.length ? "opacity-100" : "opacity-0"
                  }    self-start  pl-1 text-sm text-red-700  transition-opacity duration-300 ease-in-out `}
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
                    }   signupInputAutofill peer block w-full rounded-md border  border-gray-500 bg-[#ffffff] px-3 py-[14px] text-gray-700  placeholder-transparent  focus:border-gray-700  focus:outline-none`}
                    {...conform.input(fields.email, { type: "email" })}
                    placeholder="email@example.com"
                  />
                  <label
                    className={tw`${
                      fields.email.errors?.length
                        ? "text-red-700 peer-focus:text-red-700  "
                        : ""
                    }absolute -top-2.5 left-2 bg-[#ffffff]   px-1 text-sm capitalize text-gray-700 transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-500 peer-focus:-top-2.5 peer-focus:text-sm peer-focus:text-gray-700`}
                    htmlFor={fields.email.id}
                  >
                    email
                  </label>
                </div>

                <span
                  className={tw`${
                    fields.email.errors?.length ? "opacity-100" : "opacity-0"
                  }    self-start  pl-1 text-sm text-red-700  transition-opacity duration-300 ease-in-out `}
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
                    }   signupInputAutofill peer block w-full rounded-md border  border-gray-500 bg-[#ffffff] px-3 py-[14px] text-gray-700  placeholder-transparent  focus:border-gray-700  focus:outline-none`}
                    {...conform.input(fields.password, {
                      type: passwordInputType,
                    })}
                    placeholder="password"
                  />
                  {showPassword ? (
                    <button
                      onClick={() => togglePasswordReveal("password")}
                      type="button"
                      title="hide password icon"
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
                      title="reveal password icon"
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
                    }   peer block w-full rounded-md border  border-gray-500 bg-[#ffffff] px-3 py-[14px] text-gray-700  placeholder-transparent  focus:border-gray-700  focus:outline-none`}
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
                      title="password reveal icon"
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
                      title="password reveal icon"
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
                  {fields.passwordConfirmation.errors}
                </span>
              </div>
            </fieldset>
            <div className="flex flex-col justify-center gap-4 px-3 lg:gap-8 lg:px-2">
              <AuthButton name={signUpButtonName} submitting={submitting} />
            </div>
          </Form>
        </div>
      </div>
    </main>
  );
}
