import { AppLoadContext } from "@remix-run/cloudflare";

export async function logout(authCookie: string, context: AppLoadContext) {
  const logoutResult = await fetch(`${context.env.API_URL}/api/auth/signout`, {
    method: "POST",
    headers: {
      cookie: authCookie,
    },
  });

  return logoutResult;
}
