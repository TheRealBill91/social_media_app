export async function requestUsernameReset(env: Env, formData: FormData) {
  const email = String(formData.get("email"));

  const requestUsernameResponse = await fetch(
    `${env.API_URL}/api/auth/username-recovery-request`,
    {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        email,
      }),
    },
  );

  return requestUsernameResponse;
}
