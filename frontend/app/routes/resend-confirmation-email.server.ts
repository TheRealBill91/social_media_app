import { AppLoadContext } from "@remix-run/cloudflare";

export async function resendConfirmationEmail(
  context: AppLoadContext,
  email: string,
) {
  const resendEmailResponse = await fetch(
    `${context.env.API_URL}/api/auth/resend-email-confirmation`,
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
