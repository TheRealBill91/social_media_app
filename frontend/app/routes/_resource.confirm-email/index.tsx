import { ActionFunctionArgs, redirect } from "@remix-run/cloudflare";
import { confirmEmail } from "./confirm-email.server.ts";
import { emailConfirmationFailure } from "~/utils/cookie.server";
import { redirectWithErrorToast } from "~/utils/flash-session/flash-session.server.ts";
import { EmailConfirmationResponse } from "./types.ts";

export async function loader({ request, context }: ActionFunctionArgs) {
  console.log("request URL: " + request.url);
  const emailCallbackURL = new URL(request.url).searchParams;

  const userId = String(emailCallbackURL.get("userId"));
  console.log("userId: " + userId);

  // this is the token for the email callback URL
  const code = String(emailCallbackURL.get("code"));
  console.log("code: " + code);

  const emailConfirmationResponse = await confirmEmail(context, userId, code);

  const cookieHeader = request.headers.get("Cookie");
  const cookie = (await emailConfirmationFailure.parse(cookieHeader)) || {};

  if (!emailConfirmationResponse.ok) {
    const emailConfirmationErrors: EmailConfirmationResponse =
      await emailConfirmationResponse.json();

    // non expiration error
    if (!("email" in emailConfirmationErrors)) {
      throw await redirectWithErrorToast(
        "/",
        emailConfirmationErrors.error,
        context,
      );
      // expiration error
    } else if ("email" in emailConfirmationErrors) {
      cookie.email = emailConfirmationErrors.email;
      throw redirect("/auth/confirm-expired", {
        status: 302,
        headers: {
          "Set-Cookie": await emailConfirmationFailure.serialize(cookie),
        },
      });
    }
  }
}
