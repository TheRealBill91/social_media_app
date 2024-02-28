export async function requestPasswordReset(env: Env, email: string) {
  const requestPasswordResetResponse = await fetch(
    `${env.API_URL}/api/auth/password-reset-request`,
    {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        email,
      }),
    },
  );

  return requestPasswordResetResponse;
}
