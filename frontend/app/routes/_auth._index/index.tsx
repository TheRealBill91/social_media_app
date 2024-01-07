import { MetaFunction } from "@remix-run/cloudflare";
import { Link } from "@remix-run/react";

export const meta: MetaFunction = () => [
  { title: "Disengage | Authentication" },
];

export default function Index() {
  return (
    <>
      <main className=" flex min-h-screen flex-col items-center gap-4 ">
        <h2 className="mt-10 text-lg ">Non auth landing page!</h2>
        <div className=" flex   gap-6">
          <Link
            className=" rounded-md bg-slate-200 p-3 capitalize"
            to="/auth/signup"
          >
            sign up
          </Link>
          <Link className="rounded-md bg-slate-200 p-3 capitalize" to="signin">
            sign in
          </Link>
        </div>
      </main>
    </>
  );
}
