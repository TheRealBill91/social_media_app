/**
 * @param {Object} loginOptions - Values needed in order to log user in
 * @param {Env} loginOptions.env Used to access Cloudflare environment variables through Remix context
 * @param {string} loginOptions.username Represents the username used to login
 * @param {string} loginOptions.password the password entered by the user
 * @param {string} loginOptions.rememberMe represents whether or not the user wants to be remembered after logging in
 */

export async function login({
  env,
  username,
  password,
  rememberMe,
}: {
  env: Env;
  username: string;
  password: string;
  rememberMe?: boolean;
}) {
  const loginResponse = await fetch(`${env.API_URL}/api/auth/signin`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      username: username,
      password,
      rememberMe: rememberMe ?? false,
    }),
  });

  return loginResponse;
}
