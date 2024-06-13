import { redirect } from "@remix-run/cloudflare";
import { parse, serialize } from "cookie";
import { logout } from "~/routes/auth.logout/logout.server";
import { redirectWithSuccessToast } from "./flash-session/flash-session.server";
import { getAuthCookie } from "./cookie.server";

export function getUserId(request: Request) {
  const cookieHeader = request.headers.get("Cookie");
  const cookieObj = parse(cookieHeader || "");

  const userIdValue: string = cookieObj["UserId"];

  return userIdValue;
}

export async function getProfileInfo(request: Request, env: Env) {
  const authCookie = getAuthCookie(request);

  if (!authCookie) return undefined;

  const getProfileInfoResponse = await fetch(`${env.API_URL}/api/userprofile`, {
    method: "GET",
    headers: {
      cookie: authCookie,
    },
  });

  const profileInfoResponse: ProfileInfoResponse =
    await getProfileInfoResponse.json();

  if ("ErrorMessage" in profileInfoResponse) {
    return undefined;
  }

  return profileInfoResponse;
}

interface ProfileErrorResponse {
  ErrorMessage: "No user id available" | "Can't find the user";
}

export interface ProfileSuccessResponse {
  FirstName: string;
  LastName: string;
  UserName: string;
  CreatedAt: string;
  Photo_url?: string;
  Bio?: string;
  Location?: null;
  Url?: null;
}

type ProfileInfoResponse = ProfileErrorResponse | ProfileSuccessResponse;

/**
 * A route that requires user to be unauthenticated
 */
export function requireAnonymous(request: Request) {
  const userId = getUserId(request);

  if (userId) {
    throw redirect("/");
  }
}

export function requireAuthUser(request: Request) {
  const authCookie = getAuthCookie(request);

  if (!authCookie) {
    throw redirect("/auth/login");
  }
}

export function redirectLoggedInUser(request: Request) {
  const userId = getUserId(request);

  if (userId) {
    throw redirect("/home");
  }
}

export async function logoutAndRedirect(authCookie: string, env: Env) {
  const logoutResponse = await logout(authCookie, env);

  const logoutCookieResponseHeaders = new Headers();

  const expiredAuthCookie = serialize("auth", "", {
    maxAge: 0,
    sameSite: "lax",
    secure: true,
    priority: "medium",
    httpOnly: true,
    path: "/",
  });

  const expiredUserIdCookie = serialize("UserId", "", {
    maxAge: 0,
    sameSite: "lax",
    secure: true,
    priority: "medium",
    httpOnly: true,
    path: "/",
  });

  logoutCookieResponseHeaders.append("Set-Cookie", expiredAuthCookie);
  logoutCookieResponseHeaders.append("Set-Cookie", expiredUserIdCookie);

  if (logoutResponse.ok) {
    return redirectWithSuccessToast("/", "Logout successful", env, {
      headers: logoutCookieResponseHeaders,
    });
  }

  throw new Response("", {
    status: 500,
    statusText: "We ran into an issue logging you out",
  });
}
