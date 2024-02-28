import { serialize } from "cookie";
import { redirectWithErrorToast } from "~/utils/flash-session/flash-session.server";

export function expiredResetTokenResponse(
  expiredTokenMessage: string,
  env: Env,
) {
  const newHeaders = new Headers();
  newHeaders.append(
    "Set-Cookie",
    serialize("ExpiredMessage", "", {
      maxAge: 0,
      sameSite: "lax",
      priority: "medium",
      secure: true,
      httpOnly: true,
      path: "/",
    }),
  );

  return redirectWithErrorToast("/auth/login", expiredTokenMessage, env);
}
