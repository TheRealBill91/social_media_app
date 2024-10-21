import { default as TimerSandEmpty } from "~/components/icons/icon.tsx";
import { ResendConfirmationEmailBtn } from "~/components/ui/ResendConfirmationEmailBtn.tsx";
import {
  ActionFunctionArgs,
  LoaderFunctionArgs,
  MetaFunction,
  json,
  redirect,
} from "@remix-run/cloudflare";
import { resendConfirmationEmail } from "../../utils/resend-confirmation-email.server";
import { resendEmailErrorResponse } from "types/resend-email-error";
import {
  emailConfirmationFailure,
  postSignupEmail,
} from "~/utils/cookie.server";
import { Link, useFetcher, useLoaderData } from "@remix-run/react";
import {
  jsonWithError,
  jsonWithSuccess,
  redirectWithErrorToast,
} from "~/utils/flash-session/flash-session.server";
import { requireAnonymous } from "~/utils/auth.server";
import { combineHeaders } from "~/utils/misc";
import { ResendEmailSuccessResponse } from "../auth.signup_.success/types";

export const meta: MetaFunction = () => {
  return [{ title: "Disengage | Email Confirmation Expired" }];
};

export async function action({ request, context }: ActionFunctionArgs) {
  requireAnonymous(request);
  const { env } = context.cloudflare;

  const formData = await request.formData();

  const email = String(formData.get("email"));

  const state = String(formData.get("state"));

  if (state === "submitting") {
    return json({ status: null } as const);
  }

  if (!email) {
    return jsonWithError({ status: "error" } as const, "Email is missing", env);
  }

  const resendEmailResponse = await resendConfirmationEmail(env, email);

  if (!resendEmailResponse.ok) {
    const serverError: resendEmailErrorResponse =
      await resendEmailResponse.json();

    return redirectWithErrorToast(
      "/",
      serverError.ErrorMessage,
      env,
      {
        headers: combineHeaders(
          {
            "Set-Cookie": await postSignupEmail.serialize("", {
              maxAge: 1,
            }),
          },
          {
            "Set-Cookie": await emailConfirmationFailure.serialize("", {
              maxAge: 1,
            }),
          },
        ),
      },
      10000,
    );
  }

  const serverSuccessMessage: ResendEmailSuccessResponse =
    await resendEmailResponse.json();
  return jsonWithSuccess(
    { status: null } as const,
    serverSuccessMessage.successMessage,
    env,
  );
}

export async function loader({ request }: LoaderFunctionArgs) {
  const cookieHeader = request.headers.get("Cookie");

  // checking if user has recently signed up or accessed
  // the expired email confirmation link
  const emailConfirmationExpiredCookie =
    ((await postSignupEmail.parse(cookieHeader)) as Record<
      string,
      string
    > | null) ||
    ((await emailConfirmationFailure.parse(cookieHeader)) as Record<
      string,
      string
    > | null);

  // prevents user from accessing this page unless they come from
  // the email confirmation link
  if (!emailConfirmationExpiredCookie) {
    throw redirect("/auth/signup");
  }

  return json({ emailConfirmationExpiredCookie });
}

export default function ConfirmExpired() {
  const { emailConfirmationExpiredCookie } = useLoaderData<typeof loader>();

  const email = emailConfirmationExpiredCookie?.email;

  const emailMissing = email ? false : true;

  const confirmExpired = useFetcher<typeof action>();

  return (
    <main className="flex flex-1 flex-col items-center justify-center bg-gray-100 p-4 px-6">
      <article className="mx-auto flex w-[350px] flex-col gap-3 rounded-md border border-gray-200/80 bg-[#FFFFFF] p-3 py-8 shadow-md md:w-[400px]">
        <div>
          <h3 className="text-center text-2xl font-bold capitalize">
            email confirmation
          </h3>
        </div>
        <div className="mx-3 space-y-4 px-2 md:px-4">
          <div className="mb-2 flex flex-col items-center space-y-4">
            <TimerSandEmpty
              name="timer-sand-empty"
              className="my-3 size-10 text-gray-700"
            />
            <p className="text-center text-gray-600">
              Your email confirmation link has{" "}
              <em className="font-[600]">expired</em>.
            </p>
          </div>
          <confirmExpired.Form className="mt-2" method="post">
            <input type="hidden" name="email" value={email} />
            <input type="hidden" name="state" value={confirmExpired.state} />
            <ResendConfirmationEmailBtn
              emailMissing={emailMissing}
              state={confirmExpired.state}
              data={confirmExpired.data}
            />
          </confirmExpired.Form>

          <Link
            to="/"
            className="inline-block w-full rounded-md bg-gray-700 px-4 py-2 text-center text-white outline-0 transition-all hover:bg-gray-600 focus-visible:ring focus-visible:ring-gray-500 focus-visible:ring-offset-2"
          >
            Go Home
          </Link>
        </div>
      </article>
    </main>
  );
}
