import { createCookie } from "@remix-run/cloudflare";

// Used in allowing user to click 'resend email' after user signs up
export const postSignupEmail = createCookie("post-signup-email", {
  maxAge: 600, // 10 minutes in seconds
  httpOnly: true,
  path: "/auth",
});

// export const emailConfirmationFailuree = createCloudflareCookie(
//   context,
//   "email-confirmation-failure",

// )

export const emailConfirmationFailure = createCookie(
  "email-confirmation-failure",
  {
    maxAge: 86_400, // 7 days in seconds
    httpOnly: true,
    path: "/auth",
  },
);

/**
 * A custom implementation of the `createCookie` Remix util,
 * which provides the ability to pass in Cloudflare's context
 * in order to access env variables
 */
export function createCloudflareCookie(
  name: string,
  httpOnly: boolean,
  sameSite: boolean | "none" | "lax" | "strict",
  secure: boolean | undefined,
  secrets?: string[] | undefined,
  maxAge?: number | undefined,
) {
  return createCookie(name, {
    maxAge: maxAge,
    httpOnly: httpOnly,
    sameSite: sameSite,
    secure: secure,
    secrets: secrets,
  });
}
