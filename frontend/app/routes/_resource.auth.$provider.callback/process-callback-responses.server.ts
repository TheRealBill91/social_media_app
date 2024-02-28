import { serialize } from "cookie";
import {
  redirectWithErrorToast,
  redirectWithSuccessToast,
} from "~/utils/flash-session/flash-session.server";

export function lockedOutError(lockedOutErrorMsg: string, env: Env) {
  const newHeaders = new Headers();
  newHeaders.append(
    "Set-Cookie",
    serialize("LockedOutMessage", "", {
      maxAge: 0,
      sameSite: "lax",
      priority: "medium",
      secure: true,
      httpOnly: true,
      path: "/",
    }),
  );

  return redirectWithErrorToast("/auth/login", lockedOutErrorMsg, env);
}

export function emailClaimError(emailClaimErrorMsg: string) {
  const newHeaders = new Headers();
  newHeaders.append(
    "Set-Cookie",
    serialize("EmailClaimError", "", {
      maxAge: 0,
      sameSite: "lax",
      priority: "medium",
      secure: true,
      httpOnly: true,
      path: "/",
    }),
  );

  throw new Response(emailClaimErrorMsg, {
    status: 404,
    statusText: emailClaimErrorMsg,
  });
}

export function createOrLinkError(createOrLinkErrorMsg: string) {
  const newHeaders = new Headers();
  newHeaders.append(
    "Set-Cookie",
    serialize("CreateOrLinkError", "", {
      maxAge: 0,
      sameSite: "lax",
      priority: "medium",
      secure: true,
      httpOnly: true,
      path: "/",
    }),
  );

  throw new Response(createOrLinkErrorMsg, {
    status: 404,
    statusText: createOrLinkErrorMsg,
  });
}

export function externalSigninError(externalSigninErrorMsg: string) {
  const newHeaders = new Headers();
  newHeaders.append(
    "Set-Cookie",
    serialize("ExternalSigninError", "", {
      maxAge: 0,
      sameSite: "lax",
      priority: "medium",
      secure: true,
      httpOnly: true,
      path: "/",
    }),
  );

  throw new Response("", {
    status: 404,
    statusText: externalSigninErrorMsg,
  });
}

// If the account is successfuly created or linked, then
// logged in, this function will be used
export function successCookie(successCookieMsg: string, env: Env) {
  const newHeaders = new Headers();

  newHeaders.append(
    "Set-Cookie",
    serialize("MessageCookie", "", {
      maxAge: 0,
      sameSite: "lax",
      priority: "medium",
      secure: true,
      httpOnly: true,
      path: "/",
    }),
  );

  return redirectWithSuccessToast("/home", successCookieMsg, env, {
    headers: newHeaders,
  });
}
