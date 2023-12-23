import { Link } from "@remix-run/react";

export default function Index() {
  return (
    <main className=" flex min-h-screen flex-col items-center ">
      <div className="mt-16 flex   gap-6">
        <Link className="rounded-md bg-slate-200 p-3 capitalize" to="/auth">
          sign up/signin
        </Link>
        <Link className="rounded-md bg-slate-200 p-3 capitalize" to="signin">
          browse as guest
        </Link>
      </div>
    </main>
  );
}
