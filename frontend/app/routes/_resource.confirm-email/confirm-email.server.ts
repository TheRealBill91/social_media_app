export async function confirmEmail(env: Env, userId: string, code: string) {
  const confirmEmailResponse = await fetch(
    `${env.API_URL}/api/auth/confirm-email`,
    {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ userId, code }),
    },
  );

  return confirmEmailResponse;
}
