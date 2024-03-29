import { LoaderFunctionArgs, redirect } from "@remix-run/cloudflare";
import { confirmEmail } from "./confirm-email.server.ts";
import { emailConfirmationFailure } from "~/utils/cookie.server";
import { redirectWithErrorToast } from "~/utils/flash-session/flash-session.server.ts";
import { EmailConfirmationResponse } from "./types.ts";

export async function loader({ request, context }: LoaderFunctionArgs) {
  const emailCallbackURL = new URL(request.url).searchParams;

  const userId = String(emailCallbackURL.get("userId"));

  // this is the token for the email callback URL
  const code = String(emailCallbackURL.get("code"));

  const emailConfirmationResponse = await confirmEmail(context, userId, code);

  const cookieHeader = request.headers.get("Cookie");
  const cookie = (await emailConfirmationFailure.parse(cookieHeader)) || {};

  if (!emailConfirmationResponse.ok) {
    const emailConfirmationErrors: EmailConfirmationResponse =
      await emailConfirmationResponse.json();

    // non expiration error
    if (!("Email" in emailConfirmationErrors)) {
      throw await redirectWithErrorToast(
        "/",
        emailConfirmationErrors.ErrorMessage,
        context,
      );
      // expiration error
    } else if ("Email" in emailConfirmationErrors) {
      cookie.email = emailConfirmationErrors.Email;
      throw redirect("/auth/confirm-expired", {
        status: 302,
        headers: {
          "Set-Cookie": await emailConfirmationFailure.serialize(cookie),
        },
      });
    }
  }

  throw redirect("/auth/confirm-success");
}
