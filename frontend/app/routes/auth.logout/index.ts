import { ActionFunctionArgs } from "@remix-run/cloudflare";
import { parse, serialize } from "cookie";
import { logoutAndRedirect } from "~/utils/auth.server";

export async function action({ request, context }: ActionFunctionArgs) {
  const { env } = context.cloudflare;
  const cookieHeaders = request.headers.get("Cookie");
  const cookie = parse(cookieHeaders || "");

  const authCookieValue = cookie["auth"];

  const authCookie = serialize("auth", authCookieValue);

  return await logoutAndRedirect(authCookie, env);
}
