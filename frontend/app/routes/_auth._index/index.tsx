import { MetaFunction } from "@remix-run/cloudflare";
import { Link } from "@remix-run/react";

export const meta: MetaFunction = () => [{ title: "Disengage" }];

// export async function loader({ request }: LoaderFunctionArgs) {
//   // console.log("auth cookie? " + request.headers.get("Cookie"));
//   console.log("auth cookie? " + request.headers.get("Set-Cookie"));

//   return null;
// }

export default function Index() {
  return (
    <>
      <main className="flex min-h-screen flex-col items-center justify-center gap-5 ">
        <h2 className="mt-6 text-xl ">Non auth landing page!</h2>
        <div className=" flex gap-6">
          <Link
            className="rounded-md bg-gray-700 px-4 py-2 text-lg font-medium capitalize text-white outline-none hover:bg-gray-700/90  focus:ring focus:ring-slate-600 focus:ring-offset-2"
            to="/auth/signup"
          >
            sign up
          </Link>
          <Link
            className="rounded-md bg-gray-700 px-4 py-2 text-lg font-medium capitalize text-white outline-none hover:bg-gray-700/90 focus:ring focus:ring-gray-600 focus:ring-offset-2"
            to="/auth/login"
          >
            sign in
          </Link>
        </div>
      </main>
    </>
  );
}
