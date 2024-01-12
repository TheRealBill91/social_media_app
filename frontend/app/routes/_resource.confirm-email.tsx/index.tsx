import { ActionFunctionArgs, redirect } from "@remix-run/cloudflare";
import { confirmEmail } from "./confirm-email.server";
import { emailConfirmationFailure } from "~/utils/cookie.server.ts";

export async function loader({ request, context }: ActionFunctionArgs) {
  console.log("request URL" + request.url);
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
    throw redirect("/auth/confirm-expired", {
      status: 302,
      headers: {
        "Set-Cookie": await emailConfirmationFailure.serialize(cookie),
      },
    });
  }

  return null;
}
