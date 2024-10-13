export async function createAccount(env: Env, formData: FormData) {
  const username = String(formData.get("username"));
  const email = String(formData.get("email"));
  const firstName = String(formData.get("firstName"));
  const lastName = String(formData.get("lastName"));
  const password = String(formData.get("password"));
  const passwordConfirmation = String(formData.get("passwordConfirmation"));

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
