import { LoaderFunctionArgs, MetaFunction, json } from "@remix-run/cloudflare";
import { Link, useRouteLoaderData } from "@remix-run/react";
import { redirectLoggedInUser } from "~/utils/auth.server";

import { type loader as rootLoader } from "~/root.tsx";

export const meta: MetaFunction = () => [{ title: "Disengage" }];

export async function loader({ request }: LoaderFunctionArgs) {
  /* await redirectLoggedInUser(request); */
  return json({});
}

export default function Index() {
  const data = useRouteLoaderData<typeof rootLoader>("root");
  const userInfo = data?.userInfo;

  return (
    <>
      <main className="flex min-h-screen flex-col items-center justify-center gap-5">
        {userInfo ? (
          <h2 className="mb-2 px-4 py-2 text-2xl text-gray-600">
            Hi, {userInfo.UserName}
          </h2>
        ) : null}
        <div className="border-gray-200/4 0 mx-auto flex w-[300px] flex-col justify-center gap-6 rounded-lg  border bg-gray-100 p-6 shadow-md">
          <h2 className="mt-4 text-center text-3xl">Welcome!</h2>
          <div className="flex flex-col gap-4">
            {!userInfo ? (
              <Link
                className="rounded-md bg-gray-700 px-4 py-2 text-center text-xl font-medium capitalize text-white outline-none transition-all hover:bg-gray-700/90 focus:ring focus:ring-gray-600 focus:ring-offset-2"
                to="/auth/login"
              >
                login
              </Link>
            ) : (
              <form action="/auth/logout" method="POST">
                <button
                  type="submit"
                  className="w-full rounded-md bg-gray-700 px-4 py-2 text-center text-xl font-medium capitalize text-white outline-none transition-all hover:bg-gray-700/90 focus:ring focus:ring-gray-600 focus:ring-offset-2"
                >
                  logout
                </button>
              </form>
            )}

            <Link
              className="rounded-md bg-gray-700 px-4 py-2 text-center text-xl font-medium capitalize text-white outline-none transition-all hover:bg-gray-700/90 focus:ring focus:ring-slate-600 focus:ring-offset-2"
              to="/auth/signup"
            >
              sign up
            </Link>
          </div>
        </div>
      </main>
    </>
  );
}
