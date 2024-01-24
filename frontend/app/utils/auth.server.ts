import { redirect } from "@remix-run/cloudflare";
import { postSignupEmail } from "./cookie.server";

export async function getUserId(request: Request) {
  const cookieHeader = request.headers.get("Cookie");
  const userIdCookie: string = await postSignupEmail.parse(cookieHeader);

  if (!userIdCookie) {
    console.log("eventually will throw logout here");
  }

  console.log("userId: " + userIdCookie);
  const userId = userIdCookie;

  return userId;
}

export async function requireAnonymous(request: Request) {
  const userId = await getUserId(request);

  if (userId) {
    throw redirect("/");
  }
}
