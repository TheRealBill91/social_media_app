import { Form, useActionData, useNavigation } from "@remix-run/react";
import { signUpSchema } from "~/routes/auth.signup/signup-schema.ts";
import { useForm } from "@conform-to/react";
import { getFieldsetConstraint, parse } from "@conform-to/zod";
import {
  ActionFunctionArgs,
  MetaFunction,
  json,
  redirect,
} from "@remix-run/cloudflare";
import { StatusButton } from "~/components/ui/StatusButton.tsx";
import { createAccount } from "./create-account.server.ts";
import { transformSignupErrors } from "./transform-signup-errors.server.ts";
import { postSignupEmail } from "~/utils/cookie.server.ts";
import { Field, RevealInputField } from "~/components/Forms.tsx";
import { useIsPending } from "~/utils/misc.tsx";
import z from "zod";

export const meta: MetaFunction = () => {
  return [{ title: "Disengage | Signup" }];
};

export async function action({ request, context }: ActionFunctionArgs) {
  const { env } = context.cloudflare;
  const formData = await request.formData();

  const state = String(formData.get("state"));

  if (state === "submitting") return null;

  const submission = await parse(formData, {
    schema: (intent) =>
      signUpSchema.transform(async (data, ctx) => {
        if (intent !== "submit") return { ...data, signUpResponse: null };

        const signUpResponse = await createAccount(env, formData);

        if (!signUpResponse.ok) {
          const serverErrors: Record<string, string[]> =
            await signUpResponse.json();
          transformSignupErrors(serverErrors, ctx);

          return z.NEVER;
        }

        return { ...data, signUpResponse };
      }),
    async: true,
  });

  if (submission.intent !== "submit") {
    return json({ status: "idle", submission } as const);
  }

  if (!submission.value?.signUpResponse) {
    return json({ status: "error", submission } as const, { status: 400 });
  }

  const cookieHeader = request.headers.get("Cookie");
  const parsedCookie = (await postSignupEmail.parse(cookieHeader)) as Record<
    string,
    string
  > | null;

  const cookie: Record<string, string> | Record<string, never> = parsedCookie
    ? parsedCookie
    : {};

  cookie.email = String(formData.get("email"));

  return redirect("/auth/signup/success", {
    headers: {
      "Set-Cookie": await postSignupEmail.serialize(cookie),
    },
  });
}

export default function Signup() {
  const navigation = useNavigation();

  const isPending = useIsPending();

  const actionData = useActionData<typeof action>();

  const [form, fields] = useForm({
    id: "signup-form",
    lastSubmission: actionData?.submission,
    constraint: getFieldsetConstraint(signUpSchema),
    shouldValidate: "onBlur",

    onValidate({ formData }) {
      return parse(formData, { schema: signUpSchema });
    },
  });

  return (
    <main className="flex flex-1 flex-col items-center gap-12 bg-[#ffffff] px-8 py-10 md:p-12">
      <div className="mt-6 flex w-full flex-col justify-start rounded-lg border border-gray-400 bg-[#ffffff] p-6 sm:max-w-md md:px-10">
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
              <Field
                labelProps={{
                  children: "first name",
                  baseClass:
                    "absolute -top-2.5 font-normal left-2 bg-[#ffffff] text-sm px-1 capitalize  transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-400 peer-focus-visible:-top-2.5 peer-focus-visible:text-sm peer-focus-visible:text-gray-700",
                  errorsClass: "text-red-700 peer-focus-visible:text-red-700",
                }}
                inputProps={{
                  field: fields.firstName,
                  placeholder: "john",
                  baseClass:
                    "signupInputAutofill border-color-[unset] placeholder:text-transparent focus-visible:ring-color-[unset] peer block h-[unset] w-full rounded-md border border-gray-400 bg-[#ffffff] px-3 py-[14px] text-base text-[unset] text-gray-700 placeholder-transparent ring-[unset] focus-visible:border-gray-700 focus-visible:outline-none focus-visible:ring-0 focus-visible:ring-offset-0",
                  inputErrorsClass:
                    "border-red-700 caret-red-700 focus-visible:border-red-700",
                }}
                errorProps={{
                  errors: fields.firstName.errors,
                  errorClass:
                    "self-start pl-1 text-sm text-red-700 transition-opacity duration-300 ease-in-out",
                }}
                className="mb-4 px-3"
              />
              <Field
                labelProps={{
                  children: "last name",
                  baseClass:
                    "absolute -top-2.5 font-normal left-2 bg-[#ffffff] text-sm px-1 capitalize transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-400 peer-focus-visible:-top-2.5 peer-focus-visible:text-sm peer-focus-visible:text-gray-700",
                  errorsClass: "text-red-700 peer-focus-visible:text-red-700",
                }}
                inputProps={{
                  field: fields.lastName,
                  placeholder: "appleseed",
                  baseClass:
                    "signupInputAutofill border-color-[unset] placeholder:text-transparent focus-visible:ring-color-[unset] peer block h-[unset] w-full rounded-md border border-gray-400 bg-[#ffffff] px-3 py-[14px] text-base text-[unset] text-gray-700 placeholder-transparent ring-[unset] focus-visible:border-gray-700 focus-visible:outline-none focus-visible:ring-0 focus-visible:ring-offset-0",
                  inputErrorsClass:
                    "border-red-700 caret-red-700 focus-visible:border-red-700",
                }}
                errorProps={{
                  errors: fields.lastName.errors,
                  errorClass:
                    "self-start pl-1 text-sm text-red-700 transition-opacity duration-300 ease-in-out",
                }}
                className="mt-8 px-3"
              />

              <Field
                labelProps={{
                  children: "username",
                  baseClass:
                    "absolute -top-2.5 font-normal left-2 bg-[#ffffff] text-sm px-1 capitalize transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-400 peer-focus-visible:-top-2.5 peer-focus-visible:text-sm peer-focus-visible:text-gray-700",
                  errorsClass: "text-red-700 peer-focus-visible:text-red-700",
                }}
                inputProps={{
                  field: fields.username,
                  placeholder: "bob1234",
                  baseClass:
                    "signupInputAutofill border-color-[unset] placeholder:text-transparent focus-visible:ring-color-[unset] peer block h-[unset] w-full rounded-md border border-gray-400 bg-[#ffffff] px-3 py-[14px] text-base text-[unset] text-gray-700 placeholder-transparent ring-[unset] focus-visible:border-gray-700 focus-visible:outline-none focus-visible:ring-0 focus-visible:ring-offset-0",
                  inputErrorsClass:
                    "border-red-700 caret-red-700 focus-visible:border-red-700",
                }}
                errorProps={{
                  errors: fields.username.errors,
                  errorClass:
                    "self-start pl-1 text-sm text-red-700 transition-opacity duration-300 ease-in-out",
                }}
                className="mt-8 px-3"
              />
              <Field
                labelProps={{
                  children: "email",
                  baseClass:
                    "absolute -top-2.5 font-normal left-2 bg-[#ffffff] text-sm px-1  capitalize transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-400 peer-focus-visible:-top-2.5 peer-focus-visible:text-sm peer-focus-visible:text-gray-700",
                  errorsClass: "text-red-700 peer-focus-visible:text-red-700",
                }}
                inputProps={{
                  field: fields.email,
                  placeholder: "email@example.com",
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
                className="mt-8 px-3"
              />
              <RevealInputField
                labelProps={{
                  children: "password",
                  baseClass:
                    "absolute -top-2.5 font-normal left-2 bg-[#ffffff] text-sm px-1  capitalize  transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-400 peer-focus-visible:-top-2.5 peer-focus-visible:text-sm peer-focus-visible:text-gray-700",
                  errorsClass: "text-red-700 peer-focus-visible:text-red-700",
                }}
                inputProps={{
                  passwordField: fields.password,
                  placeholder: "password",
                  baseClass:
                    "signupInputAutofill  border-color-[unset] placeholder:text-transparent focus-visible:ring-color-[unset] peer block h-[unset] w-full rounded-md border border-gray-400 bg-[#ffffff] px-3 py-[14px] text-base text-[unset] text-gray-700 placeholder-transparent ring-[unset] focus-visible:border-gray-700 focus-visible:outline-none focus-visible:ring-0 focus-visible:ring-offset-0",
                  inputErrorsClass:
                    "border-red-700 caret-red-700 focus-visible:border-red-700",
                }}
                errorProps={{
                  errors: fields.password.errors,
                  errorClass:
                    "self-start pl-1 text-sm text-red-700 transition-opacity duration-300 ease-in-out",
                }}
                className="px-3"
              />
              <RevealInputField
                labelProps={{
                  children: "password confirmation",
                  baseClass:
                    "absolute -top-2.5 font-normal left-2 bg-[#ffffff] text-sm px-1 capitalize transition-all peer-placeholder-shown:top-4 peer-placeholder-shown:align-baseline peer-placeholder-shown:text-[1.1rem] peer-placeholder-shown:text-gray-400 peer-focus-visible:-top-2.5 peer-focus-visible:text-sm peer-focus-visible:text-gray-700",
                  errorsClass: "text-red-700 peer-focus-visible:text-red-700",
                }}
                inputProps={{
                  passwordField: fields.passwordConfirmation,
                  placeholder: "password",
                  baseClass:
                    "signupInputAutofill border-color-[unset] placeholder:text-transparent focus-visible:ring-color-[unset] peer block h-[unset] w-full rounded-md border border-gray-400 bg-[#ffffff] px-3 py-[14px] text-base text-[unset] text-gray-700 placeholder-transparent ring-[unset] focus-visible:border-gray-700 focus-visible:outline-none focus-visible:ring-0 focus-visible:ring-offset-0",
                  inputErrorsClass:
                    "border-red-700 caret-red-700 focus-visible:border-red-700",
                }}
                errorProps={{
                  errors: fields.passwordConfirmation.errors,
                  errorClass:
                    "self-start pl-1 text-sm text-red-700 transition-opacity duration-300 ease-in-out",
                }}
                className="px-3"
              />
            </fieldset>
            <div className="mt-4 flex flex-1 items-center justify-between gap-6 px-3">
              <StatusButton
                className="h-[54px] w-full flex-1 gap-4 rounded-lg px-3 py-[14px]"
                status={isPending ? "pending" : actionData?.status ?? "idle"}
                type="submit"
                disabled={isPending}
              >
                Sign up
              </StatusButton>
            </div>
          </Form>
        </div>
      </div>
    </main>
  );
}
