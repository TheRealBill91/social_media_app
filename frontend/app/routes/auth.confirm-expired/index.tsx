import { default as TimerSandEmpty } from "~/components/icons/icon.tsx";
import { ResendConfirmationEmailBtn } from "~/components/ui/ResendConfirmationEmailBtn";
import {
  ActionFunctionArgs,
  LoaderFunctionArgs,
  MetaFunction,
  json,
} from "@remix-run/cloudflare";
import { resendConfirmationEmail } from "../../utils/resend-confirmation-email.server";
import { resendEmailErrorResponse } from "types/resend-email-error";
import { postSignupEmail } from "~/utils/cookie.server";
import { Link, useFetcher, useLoaderData } from "@remix-run/react";
import {
  jsonWithError,
  jsonWithSuccess,
} from "~/utils/flash-session/flash-session.server";

export const meta: MetaFunction = () => {
  return [{ title: "Disengage | Email Confirmation Expired" }];
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

  const serverSuccessMessage: string = await resendEmailResponse.json();
  return jsonWithSuccess(null, serverSuccessMessage, env);
}

export async function loader({ request }: LoaderFunctionArgs) {
  const cookieHeader = request.headers.get("Cookie");
  const cookie = (await postSignupEmail.parse(cookieHeader)) as Record<
    string,
    string
  > | null;

  return json({ postSignupCookie: cookie });
}

export default function ConfirmExpired() {
  const { postSignupCookie } = useLoaderData<typeof loader>();

  const email = postSignupCookie?.email;

  const emailMissing = email ? false : true;

  const fetcher = useFetcher();

  const submitting = fetcher.state === "submitting";

  return (
    <main className="flex flex-1 flex-col items-center justify-center bg-gray-100 p-4 px-6 ">
      <article className="mx-auto flex w-full max-w-md flex-col gap-3 rounded-md border border-gray-200/80 bg-[#FFFFFF] p-3  py-8 shadow-md ">
        <div>
          <h3 className="text-center text-2xl font-bold capitalize">
            email confirmation
          </h3>
        </div>
        <div className="mx-3 space-y-4 px-2 md:px-4">
          <div className="my-2 flex flex-col items-center space-y-4">
            <TimerSandEmpty
              icon="timer-sand-empty"
              className="my-3 size-10 text-gray-700"
            />
            <p className="text-center text-gray-600 ">
              Your email confirmation link has{" "}
              <em className="font-[600]">expired</em>.
            </p>
          </div>
          <fetcher.Form className="mt-2" method="post">
            <input type="hidden" name="email" value={email} />
            <input type="hidden" name="state" value={fetcher.state} />
            <ResendConfirmationEmailBtn
              submitting={submitting}
              emailMissing={emailMissing}
            />
          </fetcher.Form>

          <Link
            to="/"
            className="inline-block w-full rounded-md bg-gray-800 px-4 py-2 text-center text-white outline-0 transition-all hover:bg-gray-800/90 focus-visible:ring focus-visible:ring-gray-500 focus-visible:ring-offset-2"
          >
            Go Home
          </Link>
        </div>
      </article>
    </main>
  );
}
