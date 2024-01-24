import { useForm, conform } from "@conform-to/react";
import { parse } from "@conform-to/zod";
import {
  ActionFunctionArgs,
  LoaderFunctionArgs,
  MetaFunction,
  json,
} from "@remix-run/cloudflare";
import { Form, useActionData, useNavigation } from "@remix-run/react";
import { useId } from "react";
import { BackButton } from "~/components/ui/BackButton";
import { loginSchema } from "./login-schema.ts";
import { tw } from "~/utils/tw-identity-helper";
import { AuthButton } from "~/components/ui/AuthButton";
import { RememberMeCheckbox } from "~/components/ui/RememberMeCheckbox.tsx";
import { login } from "./login.server.ts";
import { usePasswordReveal } from "~/utils/usePasswordReveal.ts";
import { PasswordRevealBtn } from "~/components/ui/PasswordRevealBtn.tsx";
import { LoginErrorResponse, LoginSuccessResponse } from "./types.ts";
import { redirectWithSuccessToast } from "~/utils/flash-session/flash-session.server.ts";
import { createCloudflareCookie } from "~/utils/cookie.server.ts";
import { requireAnonymous } from "~/utils/auth.server.ts";

export const meta: MetaFunction = () => {
  return [{ title: "Disengage | Log in" }];
};

export async function loader({ request }: LoaderFunctionArgs) {
  await requireAnonymous(request);
  return json({});
}

export async function action({ request, context }: ActionFunctionArgs) {
  const formData = await request.formData();
  const userIdentifier = String(formData.get("userIdentifier"));
  const password = String(formData.get("password"));
  const rememberMe = formData.get("rememberMe") as string | null;

  // Used in fetch request for logging in
  const persistLogin: boolean = rememberMe === "on" ? true : false;

  const state = String(formData.get("state"));

  if (state === "submitting") return null;

  const submission = parse(formData, { schema: loginSchema });

  if (submission.intent !== "submit" || !submission.value) {
    return json(submission);
  }

  const loginResponse = await login(
    context,
    userIdentifier,
    password,
    persistLogin,
  );

  if (!loginResponse.ok) {
    const loginError: LoginErrorResponse = await loginResponse.json();

    return json(loginError);
  }

  const authCookie = loginResponse.headers.getSetCookie().at(-1) as string;

  console.log("----------------------");

  const loginSuccess: LoginSuccessResponse = await loginResponse.json();
  const userId = loginSuccess.UserId;

  const userIdCookie = createCloudflareCookie(
    "user-id",
    false,
    "lax",
    context.env.ENVIRONMENT === "production",
  );

  const userIdCookieHeader = await userIdCookie.serialize(userId);

  const loginCookieHeaders = new Headers();
  loginCookieHeaders.append("Set-Cookie", authCookie);
  loginCookieHeaders.append("Set-Cookie", userIdCookieHeader);

  return redirectWithSuccessToast("/", "Successfully logged in", context, {
    headers: loginCookieHeaders,
  });
}

export default function Login() {
  const navigation = useNavigation();

  const submitting = navigation.state === "submitting";

  const passwordReveal = usePasswordReveal();

  const passwordInputType = passwordReveal.showPassword ? "text" : "password";

  const signUpButtonName = submitting ? "Signing up..." : "Sign up";

  const actionData = useActionData<typeof action>();

  const lastSubmission =
    actionData && "intent" in actionData ? actionData : null;

  const loginErrorMessage =
    actionData && "ErrorMessage" in actionData ? actionData : null;

  const id = useId();

  const [form, fields] = useForm({
    id,
    lastSubmission,
    shouldValidate: "onBlur",

    onValidate({ formData }) {
      return parse(formData, { schema: loginSchema });
    },
  });

  return (
    <main className="flex min-h-screen flex-col items-center justify-center gap-12 bg-[#ffffff] px-8 py-10  md:p-12">
      <BackButton navTo="/" twStyles="self-start" />

      <div className="flex w-full flex-1 flex-col justify-center sm:max-w-sm">
        <div className="flex   flex-col justify-start rounded-lg border border-gray-400 bg-[#ffffff] p-6 md:px-10">
          <h1 className="mt-3 text-center text-[2rem] font-bold capitalize text-gray-700">
            Log in
          </h1>
          <span className="my-2 self-center text-[1.1rem] italic text-gray-600">
            All fields are required
          </span>
          {loginErrorMessage?.ErrorMessage ? (
            <span
              className={tw`${
                loginErrorMessage.ErrorMessage ? "opacity-100" : "opacity-0"
              } mt-3  self-center px-4 pl-1 text-[1rem] text-red-700 transition-opacity duration-300 ease-in-out`}
            >
              {loginErrorMessage.ErrorMessage}
            </span>
          ) : null}
          <div className="flex flex-col items-center">
            <Form
              className="flex w-full flex-col gap-4"
              replace
              method="post"
              {...form.props}
            >
              <input type="hidden" name="state" value={navigation.state} />
              <fieldset className="mt-5">
                <div className="mb-4 flex w-full flex-col items-center gap-[6px] px-3">
                  <div className="relative w-full">
                    <input
                      className={tw`${
                        fields.userIdentifier.errors?.length
                          ? "border-red-700 focus:border-red-700  "
                          : ""
                      }   signupInputAutofill peer block w-full rounded-md  border border-gray-500 bg-[#ffffff] px-3 py-[14px]  text-gray-700 placeholder-transparent  focus:border-gray-700  focus:outline-none`}
                      {...conform.input(fields.userIdentifier, {
                        type: "text",
                      })}
                      placeholder="john"
                      autoComplete="on"
                    />
                    <label
                      htmlFor={fields.userIdentifier.id}
                      className={tw`${
                        fields.userIdentifier.errors?.length
                          ? "text-red-700 peer-focus:text-red-700  "
                          : ""
                      }absolute -top-2.5 left-2   bg-[#ffffff] px-1 text-sm text-gray-700 transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-500 peer-focus:-top-2.5 peer-focus:text-sm peer-focus:text-gray-700`}
                    >
                      Username or email
                    </label>
                  </div>

                  <span
                    className={tw`${
                      fields.userIdentifier.errors?.length
                        ? "opacity-100"
                        : "opacity-0"
                    }    self-start  pl-1 text-sm text-red-700  transition-opacity duration-300 ease-in-out`}
                    id={fields.userIdentifier.errorId}
                  >
                    {fields.userIdentifier.errors}
                  </span>
                </div>

                <div className="mt-8 flex w-full flex-col items-center gap-[6px] px-3">
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
                      fields.password.errors?.length
                        ? "opacity-100"
                        : "opacity-0"
                    }    self-start  pl-1 text-sm text-red-700  transition-opacity duration-300 ease-in-out `}
                    id={fields.password.errorId}
                  >
                    {fields.password.errors}
                  </span>
                </div>
              </fieldset>
              <RememberMeCheckbox
                checkBoxProps={conform.input(fields.rememberMe, {
                  type: "checkbox",
                })}
                fields={fields}
                className="size-5"
              />
              <div className="flex flex-col justify-center gap-4 px-3 lg:gap-8 lg:px-2">
                <AuthButton name={signUpButtonName} submitting={submitting} />
              </div>
            </Form>
          </div>
        </div>
      </div>
    </main>
  );
}
