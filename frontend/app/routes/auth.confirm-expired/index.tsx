import { default as TimerSandEmpty } from "~/components/icons/icon.tsx";
import { useFetcherWithReset } from "~/hooks/useFetcherWithReset";
import { ResendConfirmationEmailBtn } from "~/components/ui/ResendConfirmationEmail";
import { ActionFunctionArgs, MetaFunction, json } from "@remix-run/cloudflare";
/* import { resendEmailConfirmation } from "../resend-email-confirmation.server";
import { resendEmailErrorResponse } from "types/resend-email-error";
 */
export const meta: MetaFunction = () => {
  return [{ title: "Disengage | Email Confirmation Expired" }];
};

export async function action({ request }: ActionFunctionArgs) {
  const formData = await request.formData();

  const email = String(formData.get("email"));

  const state = String(formData.get("state"));

  if (state === "submitting") {
    return null;
  }

  if (!email) {
    return json({ error: "Email is missing" });
  }

  // const resendEmailResponse = await resendEmailConfirmation(context, email);

  /* if (!resendEmailResponse) {
    const serverError: resendEmailErrorResponse = await resendEmailResponse.json();
  } */
}

export default function ConfirmExpired() {
  const fetcher = useFetcherWithReset<typeof action>();

  const submitting = fetcher.state === "submitting";

  return (
    <main className="flex min-h-screen flex-col items-center justify-center bg-gray-50 p-4 ">
      <article className="mx-auto flex w-full max-w-md flex-col gap-3 rounded-md border border-gray-200 bg-white p-3 py-6 ">
        <div>
          <h3 className="text-center text-2xl font-bold">Email Confirmation</h3>
        </div>
        <div className="mx-3 space-y-4">
          <div className="flex flex-col items-center space-y-2">
            <TimerSandEmpty
              icon="timer-sand-empty"
              className="my-3 size-10 text-gray-700"
            />
            <p className="text-center text-gray-600 ">
              Your email confirmation link has{" "}
              <em className="font-[600]">expired</em>.
            </p>
          </div>
          <fetcher.Form method="post">
            <input type="hidden" name="email" value={""} />
            <input type="hidden" name="state" value={fetcher.state} />
            <ResendConfirmationEmailBtn submitting={submitting} />
          </fetcher.Form>

          <button className="w-full rounded-md bg-gray-700 px-4 py-2 text-white">
            Go Home
          </button>
        </div>
      </article>
    </main>
  );
}
