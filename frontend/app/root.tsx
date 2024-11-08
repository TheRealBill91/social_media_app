import {
  json,
  type LinksFunction,
  type LoaderFunctionArgs,
  type MetaFunction,
} from "@remix-run/cloudflare";
import styles from "./tailwind.css?url";
import {
  Links,
  Meta,
  Outlet,
  Scripts,
  ScrollRestoration,
  ShouldRevalidateFunctionArgs,
  isRouteErrorResponse,
  useLoaderData,
  useRouteError,
} from "@remix-run/react";

import {
  href as EyeOpen,
  href as EyeNone,
  href as TimerSandEmpty,
  href as CheckCircleOutline,
  href as GoogleLight,
  href as AlertCircle,
  href as List,
} from "./components/icons/icon.tsx";
import { BackButton } from "./components/ui/BackButton.tsx";
import { getToast } from "./utils/flash-session/flash-session.server.ts";
import { getProfileInfo } from "./utils/auth.server.ts";
import { Header } from "./components/ui/Header.tsx";
import { Toast } from "./components/ui/Toast.tsx";
import { useToast } from "./utils/hooks/useToast.ts";
import default_avatar from "../assets/default-avatar.png";

export const links: LinksFunction = () => {
  return [
    { rel: "stylesheet", href: styles, as: "style" },
    { rel: "preload", href: EyeOpen, as: "image" },
    { rel: "preload", href: EyeNone, as: "image" },
    { rel: "preload", href: TimerSandEmpty, as: "image" },
    { rel: "preload", href: CheckCircleOutline, as: "image" },
    { rel: "preload", href: GoogleLight, as: "image" },
    { rel: "preload", href: AlertCircle, as: "image" },
    { rel: "preload", href: List, as: "image" },
    { rel: "preload", href: default_avatar, as: "image" },
  ].filter(Boolean);
};

export const meta: MetaFunction = () => [
  {
    title: "Disengage",
    name: "description",
    content: "A social media web app designed to prevent long periods of use",
  },
];

export async function loader({ request, context }: LoaderFunctionArgs) {
  const { env } = context.cloudflare;

  const { toast, headers } = await getToast(request, env);

  const userInfo = await getProfileInfo(request, env);

  return json({ toast, userInfo }, { headers: headers });
}

// Only invoke the root loader if there was a form action in a root route
export function shouldRevalidate({ formAction }: ShouldRevalidateFunctionArgs) {
  return formAction;
}

export default function App() {
  const data = useLoaderData<typeof loader>();
  const toast = data.toast;

  useToast(toast);

  const userInfo = data.userInfo;

  return (
    <html lang="en">
      <head>
        <meta charSet="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <Meta />
        <Links />
      </head>
      <body>
        <div className="flex min-h-screen flex-col">
          <Header userInfo={userInfo} />
          <Toast />
          <Outlet />
          <ScrollRestoration />
          <Scripts />
        </div>
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
          <main className="flex min-h-screen flex-col justify-center gap-5 bg-gray-100 p-6 text-slate-900">
            <section className="mx-auto flex flex-col items-center justify-center gap-4 rounded-md bg-[#FFFFFF] p-8 shadow-md">
              <h1 className="text-center text-3xl font-bold md:text-4xl">
                Uh oh!
              </h1>
              {!error.statusText && (
                <p className="text-lg italic text-gray-500 md:text-xl">
                  Looks like we ran into an issue!
                </p>
              )}

              <div className="flex items-center gap-2">
                <p className="mr-2 text-lg lg:text-xl">{error.status} </p>
                <div className="h-8 border-r border-l-gray-800 text-lg"></div>
                <p className="ml-2 text-balance text-lg">{error.statusText}</p>
              </div>
              <hr className="my-2 h-[2px] w-full bg-gray-500" />
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
          <main className="flex min-h-screen flex-col items-center justify-center gap-4 text-slate-900">
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
