import {
  ActionFunctionArgs,
  LoaderFunctionArgs,
  json,
} from "@remix-run/cloudflare";
import { Link, useLoaderData } from "@remix-run/react";
import { useFetcher } from "@remix-run/react";
import { postSignupEmail } from "../../cookie.server.ts";
import { resendEmailConfirmation } from "./resend-email-confirmation.ts";
import { Toaster } from "sonner";

interface resendEmailErrorResponse {
  error?: string;
}

export async function action({ request, context }: ActionFunctionArgs) {
  const formData = await request.formData();

  const signupEmail = String(formData.get("signupEmail"));

  const resendEmailResponse = await resendEmailConfirmation(
    context,
    signupEmail,
  );

  if (!resendEmailResponse.ok) {
    const serverError: resendEmailErrorResponse =
      await resendEmailResponse.json();

    return json({ success: false, error: serverError });
  }

  return json({ success: true });
}

export async function loader({ request }: LoaderFunctionArgs) {
  const cookieHeader = request.headers.get("Cookie");
  const cookie = (await postSignupEmail.parse(cookieHeader)) || {};

  return json({ postSignupCookie: cookie });
}

export function SignupSuccess() {
  const { postSignupCookie } = useLoaderData<typeof loader>();

  const signupEmail = postSignupCookie.email;

  const fetcher = useFetcher<typeof action>();
  const actionResponse = fetcher.data;

  return (
    <main className="flex h-screen w-full items-center justify-center bg-[#ffffff]">
      {actionResponse?.success ? (
        <Toaster
          closeButton
          toastOptions={{
            unstyled: true,
            classNames: {
              toast: "bg-white",
              title: "text-gray-700",
            },
          }}
        />
      ) : !actionResponse?.success ? (
        <Toaster
          closeButton
          toastOptions={{
            unstyled: true,
            classNames: {
              toast: "bg-white",
              title: "text-gray-700",
            },
          }}
        />
      ) : null}
      <div className="mx-auto flex w-[350px] flex-col space-y-6 p-6">
        <div className="space-y-2 text-center">
          <h1 className="text-3xl font-bold">Account Created!</h1>
          <p className="text-gray-500 dark:text-gray-400">
            Please check your email to confirm your account before signing in.
          </p>
        </div>
        <div className="space-y-4">
          <fetcher.Form
            className="text-primary-foreground hover:bg-primary/90 w-full cursor-pointer  bg-gray-800 focus:ring focus:ring-gray-500 focus:ring-offset-1 "
            method="post"
          >
            Resend Confirmation Email
            <input type="hidden" name="signupEmail" value={signupEmail}></input>
          </fetcher.Form>

          <Link
            to="/auth"
            className="text-primary-foreground hover:bg-primary/90 w-full cursor-pointer bg-gray-800 focus:ring focus:ring-gray-500 focus:ring-offset-1"
            type="button"
          >
            Return to Home Page
          </Link>
          <p className="text-center text-sm text-gray-500 dark:text-gray-400">
            Didn&apos;t receive the email? Please check your spam folder
          </p>
        </div>
      </div>
    </main>
  );
}
