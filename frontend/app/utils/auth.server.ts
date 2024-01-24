import { AppLoadContext, redirect } from "@remix-run/cloudflare";
import { createCloudflareCookie } from "./cookie.server";

export async function getUserId(request: Request, context: AppLoadContext) {
  const userIdCookie = createCloudflareCookie(
    "user-id",
    true,
    "lax",
    context.env.ENVIRONMENT === "production",
    [context.env.COOKIE_SECRET],
  );

  const cookieHeader = request.headers.get("Cookie");
  const userId: string = await userIdCookie.parse(cookieHeader);

  if (!userIdCookie) {
    console.log("eventually will throw logout here");
  }

  console.log("userId: " + userIdCookie);

  return userId;
}

export async function requireAnonymous(
  request: Request,
  context: AppLoadContext,
) {
  const userId = await getUserId(request, context);

  if (userId) {
    throw redirect("/");
  }
}
