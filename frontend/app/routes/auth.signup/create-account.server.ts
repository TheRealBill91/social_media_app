import { AppLoadContext } from "@remix-run/cloudflare";

export async function createAccount(
  context: AppLoadContext,
  username: string,
  email: string,
  firstName: string,
  lastName: string,
  password: string,
  passwordConfirmation: string,
) {
  console.log(context.env.API_URL);

  const signUpResponse = await fetch(`https://localhost/api/auth/signup`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      username,
      email,
      firstName,
      lastName,
      password,
      passwordConfirmation,
    }),
  });

  console.log("response" + signUpResponse);

  return signUpResponse;
}
