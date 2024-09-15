import { MetaFunction } from "@remix-run/cloudflare";
import { Link } from "@remix-run/react";
import { default as CheckCircleOutline } from "~/components/icons/icon.tsx";

export const meta: MetaFunction = () => {
  return [{ title: "Disengage | Email Confirmation Successful" }];
};

export default function ConfirmSuccess() {
  return (
    <main className="flex flex-1 flex-col items-center justify-center bg-gray-100 p-4 px-6 ">
      <article className="mx-auto flex w-full max-w-md flex-col items-center gap-3 rounded-md border border-gray-200/80 bg-[#FFFFFF] p-3  py-8 shadow-md ">
        <div>
          <h3 className="text-center text-2xl font-bold capitalize">
            email confirmation
          </h3>
        </div>
        <div className="mx-3 space-y-4">
          <div className="flex flex-col items-center space-y-3">
            <CheckCircleOutline
              name="check-circle-outline"
              className="my-3 size-10 text-gray-700"
            />
            <p className="text-center text-gray-600 ">
              Your email has been successfully
              <em className="font-[600]"> confirmed</em>.
            </p>
          </div>

          <div className="flex items-center gap-3">
            <Link
              to="/"
              className="inline-block flex-1 rounded-md bg-gray-800 px-4 py-2 text-center capitalize text-white outline-0 transition-all hover:bg-gray-800/90 focus-visible:ring focus-visible:ring-gray-500 focus-visible:ring-offset-2"
            >
              go home
            </Link>
            <Link
              to="/auth/login"
              className="inline-block flex-1 rounded-md bg-gray-800 px-4 py-2 text-center capitalize text-white outline-0 transition-all hover:bg-gray-800/90 focus-visible:ring focus-visible:ring-gray-500 focus-visible:ring-offset-2"
            >
              Log in
            </Link>
          </div>
        </div>
      </article>
    </main>
  );
}
