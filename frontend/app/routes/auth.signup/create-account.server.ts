import { AppLoadContext } from "@remix-run/cloudflare";

export async function createAccount(
  context: AppLoadContext,
  username: string,
  email: string,
  firstName: string,
  lastName: string,
  password: string,
  confirmPassword: string,
) {
  const signUpResponse = await fetch(`${context.env.API_URL}/api/auth/signup`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      username,
      email,
      firstName,
      lastName,
      password,
      confirmPassword,
    }),
  });

  return signUpResponse;
}
