// Cookie that contains the email of the user who just signed up

import { createCookie } from "@remix-run/cloudflare";

// Used in allowing user to click 'resend email' after user signs up
export const postSignupEmail = createCookie("post-signup-email", {
  maxAge: 21_600, // 6 hours in seconds
  httpOnly: true,
});
