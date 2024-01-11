import {
  ActionFunctionArgs,
  LoaderFunctionArgs,
  json,
} from "@remix-run/cloudflare";
import { Link, useLoaderData } from "@remix-run/react";
import { postSignupEmail } from "~/cookie.server.ts";
import { resendEmailConfirmation } from "../resend-email-confirmation.server.ts";
import { Toaster, toast } from "sonner";
import { useEffect } from "react";
import { MetaFunction } from "@remix-run/cloudflare";
import { ActionResponse } from "./types.ts";
import { useFetcherWithReset } from "~/hooks/useFetcherWithReset.ts";
import { ResendConfirmationEmailBtn } from "~/components/ui/ResendConfirmationEmail.tsx";
import { resendEmailErrorResponse } from "types/resend-email-error.ts";

export const meta: MetaFunction = () => {
  return [{ title: "Disengage | Signup Success" }];
};

export async function action({ request, context }: ActionFunctionArgs) {
  const formData = await request.formData();

  const signupEmail = String(formData.get("signupEmail"));

  const state = String(formData.get("state"));

  if (state === "submitting") {
    return null;
  }

  if (!signupEmail) {
    return json({ error: "Email is missing" });
  }

  const resendEmailResponse = await resendEmailConfirmation(
    context,
    signupEmail,
  );

  if (!resendEmailResponse.ok) {
    const serverError: resendEmailErrorResponse =
      await resendEmailResponse.json();

    return json(
      { success: false, error: serverError.error },
      {
        headers: {
          "Set-Cookie": await postSignupEmail.serialize("", {
            maxAge: 1,
          }),
        },
      },
    );
  }

  return json({ success: true });
}

export async function loader({ request }: LoaderFunctionArgs) {
  const cookieHeader = request.headers.get("Cookie");
  const cookie = (await postSignupEmail.parse(cookieHeader)) || {};

  return json({ postSignupCookie: cookie });
}

export default function SignupSuccess() {
  const { postSignupCookie } = useLoaderData<typeof loader>();

  const signupEmail: string = postSignupCookie.email || "";

  const fetcher = useFetcherWithReset<typeof action>();
  const actionResponse = fetcher.data as ActionResponse | undefined;

  const submitting = fetcher.state === "submitting";

  useEffect(() => {
    if (actionResponse) {
      if (actionResponse?.success) {
        toast.success("Resent email confirmation", { duration: 20000 });
      } else if (!actionResponse?.success) {
        toast.error(actionResponse.error, {
          position: "top-center",
        });
      }
      fetcher.reset();
    }
  }, [fetcher, actionResponse]);

  return (
    <main className="flex h-screen w-full  items-center justify-center bg-[#ffffff]">
      <Toaster
        closeButton
        position="top-center"
        toastOptions={{
          unstyled: true,

          classNames: {
            toast:
              "bg-white border border-gray-50 shadow-md w-[350px] rounded-md p-4 flex justify-center items-center ",
            title: "text-gray-700 ml-3",
          },
        }}
      />
      <div className="mx-auto flex w-[350px] flex-col gap-2 space-y-6 rounded-md border border-gray-800/40 p-10 md:w-[400px]">
        <div className="space-y-2 text-center">
          <h1 className="text-3xl font-bold">Account Created!</h1>
          <p className="text-gray-800 dark:text-gray-400">
            Please check your email to confirm your account before signing in.
          </p>
        </div>
        <div className="space-y-4">
          <fetcher.Form method="post">
            <input type="hidden" name="signupEmail" value={signupEmail}></input>
            <input type="hidden" name="state" value={fetcher.state}></input>
            <ResendConfirmationEmailBtn submitting={submitting} />
          </fetcher.Form>

          <Link
            to="/"
            className="text-primary-foreground inline-flex w-full  cursor-pointer items-center justify-center rounded-md bg-gray-800 px-4 py-2 text-white outline-0 transition-all hover:bg-gray-800/90 focus:ring focus:ring-gray-500 focus:ring-offset-2"
            type="button"
          >
            Return to Home Page
          </Link>
          <p className="text-center text-sm text-gray-800 ">
            Didn&apos;t receive the email? Please check your spam folder
          </p>
        </div>
      </div>
    </main>
  );
}
