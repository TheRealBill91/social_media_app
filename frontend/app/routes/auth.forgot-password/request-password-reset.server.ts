import { AppLoadContext } from "@remix-run/cloudflare";

export async function requestPasswordReset(
  context: AppLoadContext,
  email: string,
) {
  const requestPasswordResetResponse = await fetch(
    `${context.env.API_URL}api/auth/password-reset-request`,
    {
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        email,
      }),
    },
  );

  return requestPasswordResetResponse;
}
