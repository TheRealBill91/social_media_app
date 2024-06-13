import { useForm, conform } from "@conform-to/react";
import { parse, getFieldsetConstraint } from "@conform-to/zod";
import {
  ActionFunctionArgs,
  LoaderFunctionArgs,
  MetaFunction,
  json,
} from "@remix-run/cloudflare";
import { Form, useActionData, useNavigation, Link } from "@remix-run/react";
import { loginSchema } from "./login-schema.ts";
import { tw } from "~/utils/tw-identity-helper";
import { AuthButton } from "~/components/ui/AuthButton";
import { ErrorList, RememberMeCheckbox } from "~/components/Forms.tsx";
import { login } from "./login.server.ts";
import { usePasswordReveal } from "~/utils/hooks/usePasswordReveal.ts";
import { PasswordRevealBtn } from "~/components/ui/PasswordRevealBtn.tsx";
import { LoginErrorResponse, LoginSuccessResponse } from "./types.ts";
import { redirectWithSuccessToast } from "~/utils/flash-session/flash-session.server.ts";
import { createCloudflareCookie } from "~/utils/cookie.server.ts";
import { requireAnonymous } from "~/utils/auth.server.ts";
import { AuthDivider } from "~/components/ui/AuthDivider.tsx";
import { ProviderConnectionForm } from "~/utils/connections.tsx";
import z from "zod";
import { FormError } from "./FormError.tsx";

export const meta: MetaFunction = () => {
  return [{ title: "Disengage | Log in" }];
};

export function loader({ request }: LoaderFunctionArgs) {
  requireAnonymous(request);
  return json({});
}

export async function action({ request, context }: ActionFunctionArgs) {
  const { env } = context.cloudflare;
  const formData = await request.formData();

  const state = String(formData.get("state"));

  if (state === "submitting") return null;

  const submission = await parse(formData, {
    schema: (intent) =>
      loginSchema.transform(async (data, ctx) => {
        if (intent !== "submit") return { ...data, loginResponse: null };

        const loginResponse = await login({ ...data, env });
        if (!loginResponse.ok) {
          const loginError: LoginErrorResponse = await loginResponse.json();
          ctx.addIssue({
            code: z.ZodIssueCode.custom,
            message: loginError.ErrorMessage,
          });
          return z.NEVER;
        }

        return { ...data, loginResponse };
      }),
    async: true,
  });
  delete submission.payload.password;

  // means form validation not triggered by form submission
  if (submission.intent !== "submit") {
    // @ts-expect-error - wont be needed when we upgrade to Conform v1
    delete submission.value?.password;
    return json({ status: "idle", submission } as const);
  }

  // means issue with login
  if (!submission.value?.loginResponse) {
    return json({ status: "error", submission } as const, { status: 400 });
  }

  const { loginResponse } = submission.value;

  const authCookie = loginResponse.headers.getSetCookie().at(-1) as string;

  console.log("----------------------");

  const loginSuccess: LoginSuccessResponse = await loginResponse.json();
  const userId = loginSuccess.UserId;

  const userIdCookie = createCloudflareCookie(
    "UserId",
    true,
    "lax",
    env.ENVIRONMENT === "production",
    [env.COOKIE_SECRET],
    259_200,
  );

  const userIdCookieHeader = await userIdCookie.serialize(userId);

  const loginCookieHeaders = new Headers();
  loginCookieHeaders.append("Set-Cookie", authCookie);
  loginCookieHeaders.append("Set-Cookie", userIdCookieHeader);

  return redirectWithSuccessToast("/home", "Successfully logged in", env, {
    headers: loginCookieHeaders,
  });
}

export default function Login() {
  const navigation = useNavigation();

  const submitting =
    navigation.state === "submitting" &&
    navigation.formAction === "/auth/login";

  const passwordReveal = usePasswordReveal();

  const passwordInputType = passwordReveal.showPassword ? "text" : "password";

  const loginButtonName = submitting ? "Authenticating..." : "Login";

  const actionData = useActionData<typeof action>();

  const [form, fields] = useForm({
    id: "login-form",
    lastSubmission: actionData?.submission,
    constraint: getFieldsetConstraint(loginSchema),
    shouldValidate: "onBlur",
    shouldRevalidate: "onBlur",

    onValidate({ formData }) {
      return parse(formData, { schema: loginSchema });
    },
  });

  return (
    <main className="flex flex-1 flex-col items-center justify-center gap-12 bg-[#ffffff] px-8 py-12 md:p-12">
      <div className="flex w-full max-w-[24rem] flex-col justify-start rounded-lg border border-gray-400 bg-[#ffffff] px-8 py-6 md:px-12">
        <h1 className="mt-3 text-center text-[2rem] font-bold capitalize text-gray-700">
          Log in
        </h1>
        <span className="my-2 self-center text-[1.1rem] italic text-gray-600">
          All fields are required
        </span>
        <FormError formError={form.error} />
        <div className="flex flex-col items-center">
          <Form replace method="post" className="w-full" {...form.props}>
            <input type="hidden" name="state" value={navigation.state} />
            <fieldset className="mt-5">
              <div className="mb-4 flex flex-col items-center gap-[6px]">
                <div className="relative w-full">
                  <input
                    className={tw`${
                      fields.username.errors?.length
                        ? "border-red-700 caret-red-700 focus-visible:border-red-700"
                        : ""
                    } signupInputAutofill peer block w-full rounded-md border border-gray-400 bg-[#ffffff] px-3 py-[14px] text-gray-700 placeholder-transparent focus-visible:border-gray-700 focus-visible:outline-none`}
                    {...conform.input(fields.username, {
                      type: "text",
                    })}
                    placeholder="john"
                    autoComplete="on"
                  />
                  <label
                    htmlFor={fields.username.id}
                    className={tw`${
                      fields.username.errors?.length
                        ? "text-red-700 peer-focus-visible:text-red-700"
                        : ""
                    } absolute -top-2.5 left-2 bg-[#ffffff] px-1 text-sm text-gray-700 transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-400 peer-focus-visible:-top-2.5 peer-focus-visible:text-sm peer-focus-visible:text-gray-700`}
                  >
                    Username
                  </label>
                </div>

                <ErrorList
                  className="self-start pl-1 text-sm text-red-700 transition-opacity duration-300 ease-in-out"
                  errors={fields.username.errors}
                />
              </div>

              <div className="mt-8 flex flex-col items-center gap-[6px]">
                <div className="relative w-full">
                  <input
                    className={tw`${
                      fields.password.errors?.length
                        ? "border-red-700 caret-red-700 focus-visible:border-red-700"
                        : ""
                    } signupInputAutofill peer block w-full rounded-md border border-gray-400 bg-[#ffffff] px-3 py-[14px] text-gray-700 placeholder-transparent focus-visible:border-gray-700 focus-visible:outline-none`}
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
                    htmlFor={fields.password.id}
                    className={tw`${
                      fields.password.errors?.length
                        ? "text-red-700 peer-focus-visible:text-red-700"
                        : ""
                    } absolute -top-2.5 left-2 bg-[#ffffff] px-1 text-sm capitalize text-gray-700 transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-400 peer-focus-visible:-top-2.5 peer-focus-visible:text-sm peer-focus-visible:text-gray-700`}
                  >
                    password
                  </label>
                </div>

                <ErrorList
                  className="self-start pl-1 text-red-700"
                  errors={fields.password.errors}
                />
              </div>
            </fieldset>
            <div className="my-2 mt-3 flex justify-between pt-1 *:text-gray-600">
              <Link
                to="/auth/forgot-password"
                className="transition-color text-sm capitalize hover:text-gray-800 hover:underline hover:decoration-gray-800 hover:underline-offset-2"
              >
                forgot password?
              </Link>
              <Link
                to="/auth/forgot-username"
                className="transition-color text-sm capitalize hover:text-gray-800 hover:underline hover:decoration-gray-800 hover:underline-offset-4"
              >
                forgot username?
              </Link>
            </div>
            <RememberMeCheckbox
              checkBoxProps={conform.input(fields.rememberMe, {
                type: "checkbox",
              })}
              className="size-5"
              labelProps={{
                htmlFor: fields.rememberMe.id,
                children: "Remember me",
              }}
            />
            <AuthButton
              name={loginButtonName}
              submitting={submitting}
              className="h-[54px] w-full"
            />
          </Form>
          <AuthDivider />
          <ProviderConnectionForm
            btnClassName="h-[54px]"
            iconClassName="size-10"
          />
        </div>
      </div>
    </main>
  );
}
