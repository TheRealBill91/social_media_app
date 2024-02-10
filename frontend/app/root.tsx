import {
  json,
  type LinksFunction,
  type LoaderFunctionArgs,
  type MetaFunction,
} from "@remix-run/cloudflare";
import styles from "./tailwind.css";
import {
  Links,
  LiveReload,
  Meta,
  Outlet,
  Scripts,
  ScrollRestoration,
  isRouteErrorResponse,
  useLoaderData,
  useRouteError,
} from "@remix-run/react";

import {
  href as Spinner,
  href as EyeOpen,
  href as EyeNone,
  href as TimerSandEmpty,
  href as CheckCircleOutline,
  href as GoogleLight,
  href as AlertCircle,
} from "./components/icons/icon.tsx";
import { BackButton } from "./components/ui/BackButton.tsx";
import { toast as showToast, Toaster } from "sonner";
import { getToast } from "./utils/flash-session/flash-session.server.ts";
import { useEffect } from "react";
import { getProfileInfo } from "./utils/auth.server.ts";

export const links: LinksFunction = () => [
  { rel: "stylesheet", href: styles },
  { rel: "preload", href: Spinner, as: "image" },
  { rel: "preload", href: EyeOpen, as: "image" },
  { rel: "preload", href: EyeNone, as: "image" },
  { rel: "preload", href: TimerSandEmpty, as: "image" },
  { rel: "preload", href: CheckCircleOutline, as: "image" },
  { rel: "preload", href: GoogleLight, as: "image" },
  { rel: "preload", href: AlertCircle, as: "image" },
];

export const meta: MetaFunction = () => [
  {
    title: "Disengage",
    name: "description",
    content: "A social media web app designed to prevent long periods of use",
  },
];

export async function loader({ request, context }: LoaderFunctionArgs) {
  const { toast, headers } = await getToast(request, context);

  const userInfo = await getProfileInfo(request, context);

  return json({ toast, userInfo }, { headers: headers });
}

export default function App() {
  const data = useLoaderData<typeof loader>();
  const toast = data.toast;

  useEffect(() => {
    if (toast?.type === "error") {
      showToast.error(toast.text, {
        id: "errorId",
      });
    } else if (toast?.type === "success") {
      showToast.success(toast.text, {
        id: "successId",
      });
    }
  }, [toast]);

  return (
    <html lang="en">
      <head>
        <meta charSet="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <Meta />
        <Links />
      </head>
      <body className="">
        <Toaster
          closeButton
          position="top-center"
          toastOptions={{
            unstyled: true,

            classNames: {
              toast:
                "bg-white border border-gray-50 shadow-md w-[350px] lg:w-[400px] rounded-md p-4 flex justify-center items-center ",
              title: "text-gray-700 ml-3",
            },
          }}
        />
        <Outlet />
        <ScrollRestoration />
        <Scripts />
        <LiveReload />
      </body>
    </html>
  );
}

export function ErrorBoundary() {
  const error = useRouteError();
  console.error(error);

  const navTo = "/";

  if (isRouteErrorResponse(error)) {
    return (
      <html lang="en">
        <head>
          <title>Oh no!</title>
          <meta charSet="utf-8" />
          <meta name="viewport" content="width=device-width, initial-scale=1" />
          <Meta />
          <Links />
        </head>
        <body>
          <main className="flex min-h-screen flex-col justify-center gap-5 bg-gray-100 p-6 text-slate-900 ">
            <section className="mx-auto flex flex-col items-center justify-center gap-4 rounded-md bg-[#FFFFFF] p-8 shadow-md">
              <h1 className="text-center text-3xl font-bold md:text-4xl">
                Uh oh!
              </h1>
              {!error.statusText && (
                <p className="text-lg italic text-gray-500 md:text-xl">
                  Looks like we ran into an issue!
                </p>
              )}

              <div className="flex items-center gap-2 ">
                <p className="mr-2 text-lg lg:text-xl">{error.status} </p>
                <div className=" h-8 border-r border-l-gray-800  text-lg"></div>
                <p className="ml-2 text-balance text-lg">{error.statusText}</p>
              </div>
              <hr className="my-2 h-[2px] w-full bg-gray-500 " />
              <BackButton navTo={navTo} />
            </section>
          </main>
          <Scripts />
        </body>
      </html>
    );
  } else if (error instanceof Error) {
    return (
      <html lang="en">
        <head>
          <title>Oh no!</title>
          <meta charSet="utf-8" />
          <meta name="viewport" content="width=device-width, initial-scale=1" />
          <Meta />
          <Links />
        </head>
        <body>
          <main className="flex min-h-screen flex-col items-center justify-center gap-4 text-slate-900 ">
            <h1 className="text-center text-3xl font-bold md:text-4xl">
              Uh oh!
            </h1>
            <p className="text-lg md:text-xl">
              Looks like we ran into an issue!
            </p>
            <BackButton navTo={navTo} />
          </main>
          <Scripts />
        </body>
      </html>
    );
  }
}
