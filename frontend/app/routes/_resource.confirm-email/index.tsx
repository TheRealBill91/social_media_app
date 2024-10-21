import { LoaderFunctionArgs, redirect } from "@remix-run/cloudflare";
import { confirmEmail } from "./confirm-email.server.ts";
import { emailConfirmationFailure } from "~/utils/cookie.server";
import { redirectWithErrorToast } from "~/utils/flash-session/flash-session.server.ts";
import { EmailConfirmationResponse } from "./types.ts";

export async function loader({ request, context }: LoaderFunctionArgs) {
  const { env } = context.cloudflare;
  const emailCallbackURL = new URL(request.url).searchParams;

  const userId = String(emailCallbackURL.get("userId"));

  // this is the token for the email callback URL
  const code = String(emailCallbackURL.get("code"));

  const emailConfirmationResponse = await confirmEmail(env, userId, code);

  const cookieHeader: string | null = request.headers.get("Cookie");

  const cookie = ((await emailConfirmationFailure.parse(cookieHeader)) ||
    {}) as Record<string, string | null> | Record<string, never>;

  if (!emailConfirmationResponse.ok) {
    const emailConfirmationErrors: EmailConfirmationResponse =
      await emailConfirmationResponse.json();

    // non expiration error
    if (!("Email" in emailConfirmationErrors)) {
      throw await redirectWithErrorToast(
        "/",
        emailConfirmationErrors.ErrorMessage,
        env,
      );
      // this is for the change informing whether or not the frontend can
      // request a new email confirmation based on the the daily limit
    } else if ("Email" in emailConfirmationErrors) {
      cookie.email = emailConfirmationErrors.CanRequestNewEmailConfirmation
        ? emailConfirmationErrors.Email
        : null;
      throw redirect("/auth/confirm-expired", {
        headers: {
          "Set-Cookie": await emailConfirmationFailure.serialize(cookie),
        },
      });
    }
  }

  throw redirect("/auth/confirm-success");
}
