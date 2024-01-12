import { createCookie } from "@remix-run/cloudflare";

// Used in allowing user to click 'resend email' after user signs up
export const postSignupEmail = createCookie("post-signup-email", {
  maxAge: 600, // 10 minutes in seconds
  httpOnly: true,
});

export const emailConfirmationFailure = createCookie(
  "email-confirmation-failure",
  {
    maxAge: 86_400, // 7 days in seconds
    httpOnly: true,
  },
);
