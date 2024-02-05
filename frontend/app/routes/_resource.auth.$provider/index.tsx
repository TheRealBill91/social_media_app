import { ActionFunctionArgs, redirect } from "@remix-run/cloudflare";

export async function action({ context }: ActionFunctionArgs) {
  return redirect(`${context.env.API_URL}/api/auth/google-sign-in`);
}
