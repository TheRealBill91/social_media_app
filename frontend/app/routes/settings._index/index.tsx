import { redirect } from "@remix-run/cloudflare";

export function loader() {
  return redirect("/settings/profile");
}
