import {
  ActionFunctionArgs,
  LoaderFunctionArgs,
  json,
} from "@remix-run/cloudflare";
import {
  Link,
  useFetcher,
  useLoaderData,
  MetaFunction,
} from "@remix-run/react";
import { resendConfirmationEmail } from "~/utils/resend-confirmation-email.server.ts";

import { ResendConfirmationEmailBtn } from "~/components/ui/ResendConfirmationEmailBtn.tsx";
import { resendEmailErrorResponse } from "types/resend-email-error.ts";
import {
  getToast,
  jsonWithError,
  jsonWithSuccess,
} from "~/utils/flash-session/flash-session.server.ts";
import { postSignupEmail } from "~/utils/cookie.server.ts";
import { resendEmailSuccessResponse } from "./types.ts";

export const meta: MetaFunction = () => {
  return [{ title: "Disengage | Signup Success" }];
};

export async function action({ request, context }: ActionFunctionArgs) {
  const { env } = context.cloudflare;
  const formData = await request.formData();

  const email = String(formData.get("email"));

  const state = String(formData.get("state"));

  if (state === "submitting") {
    return null;
  }

  if (!email) {
    return jsonWithError(null, "Email is missing", env);
  }

  const resendEmailResponse = await resendConfirmationEmail(env, email);

  if (!resendEmailResponse.ok) {
    const serverError: resendEmailErrorResponse =
      await resendEmailResponse.json();

    return jsonWithError(null, serverError.ErrorMessage, env, {
      headers: {
        "Set-Cookie": await postSignupEmail.serialize("", {
          maxAge: 1,
        }),
      },
    });
  }
  const serverSuccessResponse: resendEmailSuccessResponse =
    await resendEmailResponse.json();

  return jsonWithSuccess(null, serverSuccessResponse.successMessage, env);
}

export async function loader({ request, context }: LoaderFunctionArgs) {
  const { env } = context.cloudflare;

  const cookieHeader = request.headers.get("Cookie");
  const cookie = (await postSignupEmail.parse(cookieHeader)) as Record<
    string,
    string
  >;

  const { toast, headers } = await getToast(request, env);

  return json({ toast, postSignUpCookie: cookie }, { headers });
}

export default function SignupSuccess() {
  const { postSignUpCookie } = useLoaderData<typeof loader>();

  const email: string = postSignUpCookie.email || "";

  const emailMissing = email ? false : true;

  const fetcher = useFetcher();

  const submitting = fetcher.state === "submitting";

  return (
    <main className="flex h-screen w-full flex-1  items-center justify-center bg-gray-100">
      <div className="mx-auto flex w-[350px] flex-col gap-2 space-y-6 rounded-md border border-gray-200/80 bg-[#FFFFFF] p-10 shadow-md md:w-[400px]">
        <div className="space-y-2 text-center">
          <h1 className="text-3xl font-bold">Account Created!</h1>
          <p className="text-gray-600">
            Please check your email to confirm your account before signing in.
          </p>
        </div>
        <div className="space-y-4">
          <fetcher.Form method="post">
            <input type="hidden" name="email" value={email}></input>
            <input type="hidden" name="state" value={fetcher.state}></input>
            <ResendConfirmationEmailBtn
              submitting={submitting}
              emailMissing={emailMissing}
            />
          </fetcher.Form>

          <Link
            to="/"
            className="inline-flex w-full cursor-pointer items-center justify-center rounded-md bg-gray-800 px-4 py-2 text-primary-foreground text-white outline-0 transition-all hover:bg-gray-800/90 focus-visible:ring focus-visible:ring-gray-500 focus-visible:ring-offset-2"
            type="button"
          >
            Return to Home Page
          </Link>
          <p className="text-center text-sm text-gray-800">
            Didn&apos;t receive the email? Please check your spam folder
          </p>
        </div>
      </div>
    </main>
  );
}
