import { ActionFunctionArgs } from "@remix-run/cloudflare";

export async function loader({ request }: ActionFunctionArgs) {
  console.log("request URL" + request.url);
  const emailCallbackURL = new URL(request.url).searchParams;

  const userId = emailCallbackURL.get("userId");
  console.log("userId: " + userId);

  // this is the token for the email callback URL
  const code = emailCallbackURL.get("code");
  console.log("code: " + code);

  return null;
}
