import {
  ActionFunctionArgs,
  LoaderFunctionArgs,
  SerializeFrom,
  json,
  redirect,
} from "@remix-run/cloudflare";
import { FetcherWithComponents, Link, useLoaderData } from "@remix-run/react";
import { useFetcher } from "@remix-run/react";
import { postSignupEmail } from "../../cookie.server.ts";
import { resendEmailConfirmation } from "./resend-email-confirmation.server.ts";
import { Toaster, toast } from "sonner";
import { useEffect, useState } from "react";
import { MetaFunction } from "@remix-run/cloudflare";

export const meta: MetaFunction = () => {
  return [{ title: "Disengage | Signup Success" }];
};

export type FetcherWithComponentsReset<T> = FetcherWithComponents<T> & {
  reset: () => void;
};

export function useFetcherWithReset<T>(): FetcherWithComponentsReset<
  SerializeFrom<T>
> {
  const fetcher = useFetcher<T>();
  const [data, setData] = useState(fetcher.data);
  useEffect(() => {
    if (fetcher.state === "idle") {
      setData(fetcher.data);
    }
  }, [fetcher.state, fetcher.data]);
  return {
    ...fetcher,
    data: data as SerializeFrom<T> | undefined,
    reset: () => setData(undefined),
  };
}

interface resendEmailErrorResponse {
  error?: string;
}

interface ActionSuccessReponse {
  success: true;
}

interface ActionErrorResponse {
  success: false;
  error: string;
}

type ActionResponse = ActionSuccessReponse | ActionErrorResponse;

export async function action({ request, context }: ActionFunctionArgs) {
  const formData = await request.formData();

  const signupEmail = String(formData.get("signupEmail"));

  const resendEmailResponse = await resendEmailConfirmation(
    context,
    signupEmail,
  );

  console.log(
    "email confirmation resend response status: " + resendEmailResponse.status,
  );

  if (!resendEmailResponse.ok) {
    const serverError: resendEmailErrorResponse =
      await resendEmailResponse.json();
    console.log("signup email error: " + JSON.stringify(serverError.error));

    return json(
      { success: false, error: serverError.error },
      {
        headers: {
          "Set-Cookie": await postSignupEmail.serialize("", {
            maxAge: 1,
          }),
        },
      },
    );
  }

  return json({ success: true });
}

export async function loader({ request }: LoaderFunctionArgs) {
  const cookieHeader = request.headers.get("Cookie");
  const cookie = (await postSignupEmail.parse(cookieHeader)) || {};

  /*  if (!cookie.email) {
    return redirect("/");
  } */

  return json({ postSignupCookie: cookie });
}

export default function SignupSuccess() {
  const { postSignupCookie } = useLoaderData<typeof loader>();

  const signupEmail = postSignupCookie.email || "";

  const fetcher = useFetcherWithReset<typeof action>();
  const actionResponse = fetcher.data as ActionResponse | undefined;

  console.log(actionResponse);

  useEffect(() => {
    if (actionResponse) {
      if (actionResponse?.success) {
        toast.success("Resent email confirmation", { duration: 20000 });
      } else if (!actionResponse?.success) {
        toast.error(actionResponse.error, {
          position: "top-center",
        });
      }
      fetcher.reset();
    }
  }, [fetcher, actionResponse]);

  return (
    <main className="flex h-screen w-full  items-center justify-center bg-[#ffffff]">
      <Toaster
        closeButton
        position="top-center"
        toastOptions={{
          unstyled: true,

          classNames: {
            toast:
              "bg-white border border-gray-50 shadow-md w-[350px] rounded-md p-4 flex justify-center items-center ",
            title: "text-gray-700 ml-3",
          },
        }}
      />
      <div className="mx-auto flex w-[350px] flex-col gap-2 space-y-6 rounded-md border border-gray-800/40 p-10 md:w-[400px]">
        <div className="space-y-2 text-center">
          <h1 className="text-3xl font-bold">Account Created!</h1>
          <p className="text-gray-800 dark:text-gray-400">
            Please check your email to confirm your account before signing in.
          </p>
        </div>
        <div className="space-y-4">
          <fetcher.Form className=" " method="post">
            <input
              type="hidden"
              name="signupEmail"
              value={signupEmail || "bc35786+3@gmail.com"}
            ></input>
            <button
              type="submit"
              className="text-primary-foreground    inline-flex h-10 w-full cursor-pointer items-center justify-center rounded-md bg-gray-800 px-4 py-2 text-center text-white  hover:bg-gray-800/90 focus:ring focus:ring-gray-500 focus:ring-offset-2"
            >
              {" "}
              Resend Confirmation Email
            </button>
          </fetcher.Form>

          <Link
            to="/"
            className="text-primary-foreground inline-flex w-full cursor-pointer items-center justify-center rounded-md bg-gray-800 px-4 py-2 text-white hover:bg-gray-800/90 focus:ring focus:ring-gray-500 focus:ring-offset-2"
            type="button"
          >
            Return to Home Page
          </Link>
          <p className="text-center text-sm text-gray-800 ">
            Didn&apos;t receive the email? Please check your spam folder
          </p>
        </div>
      </div>
    </main>
  );
}
