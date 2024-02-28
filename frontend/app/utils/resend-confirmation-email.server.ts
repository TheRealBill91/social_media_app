export async function resendConfirmationEmail(env: Env, email: string) {
  const resendEmailResponse = await fetch(
    `${env.API_URL}/api/auth/resend-confirmation-email`,
    {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        email,
      }),
    },
  );

  return resendEmailResponse;
}
