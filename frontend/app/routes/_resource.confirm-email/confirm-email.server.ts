import { AppLoadContext } from "@remix-run/cloudflare";

export async function confirmEmail(
  context: AppLoadContext,
  userId: string,
  code: string,
) {
  const confirmEmailResponse = await fetch(
    `${context.env.API_URL}/api/auth/confirm-email`,
    {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ userId, code }),
    },
  );

  return confirmEmailResponse;
}
