export async function createAccount(
  env: Env,
  username: string,
  email: string,
  firstName: string,
  lastName: string,
  password: string,
  passwordConfirmation: string,
) {
  const signUpResponse = await fetch(`${env.API_URL}/api/auth/signup`, {
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

  return signUpResponse;
}
