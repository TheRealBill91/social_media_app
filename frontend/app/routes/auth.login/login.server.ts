import { AppLoadContext } from "@remix-run/cloudflare";

/**
 *
 * @param context Used to access Cloudflare environment variables through Remix context
 * @param userIdentifier Represents either the username or email
 * @param password the password entered by the user
 * @param persistLogin represents whether or not the user wants to be remembered after logging in
 */

export async function login(
  context: AppLoadContext,
  userIdentifier: string, // the username or password
  password: string,
  persistLogin: boolean,
) {
  const loginResponse = await fetch(`${context.env.API_URL}/api/auth/signin`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      emailOrUsername: userIdentifier,
      password,
      rememberMe: persistLogin,
    }),
  });

  return loginResponse;
}
