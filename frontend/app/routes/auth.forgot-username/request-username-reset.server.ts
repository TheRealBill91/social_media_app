export async function requestUsername(env: Env, email: string) {
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
