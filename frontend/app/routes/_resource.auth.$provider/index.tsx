import { ActionFunctionArgs, redirect } from "@remix-run/cloudflare";

export async function action({ context }: ActionFunctionArgs) {
  const { env } = context.cloudflare;
  return redirect(`${env.API_URL}/api/auth/google-sign-in`);
}
