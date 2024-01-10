import type { LinksFunction, MetaFunction } from "@remix-run/cloudflare";
import styles from "./tailwind.css";
import {
  Links,
  LiveReload,
  Meta,
  Outlet,
  Scripts,
  ScrollRestoration,
  isRouteErrorResponse,
  useRouteError,
} from "@remix-run/react";

import {
  href as Spinner,
  href as EyeOpen,
  href as EyeNone,
  href as TimerSandEmpty,
} from "./components/icons/icon.tsx";
import { BackButton } from "./components/ui/BackButton.tsx";

export const links: LinksFunction = () => [
  { rel: "stylesheet", href: styles },
  { rel: "preload", href: Spinner, as: "image" },
  { rel: "preload", href: EyeOpen, as: "image" },
  { rel: "preload", href: EyeNone, as: "image" },
  { rel: "preload", href: TimerSandEmpty, as: "image" },
];

export const meta: MetaFunction = () => [
  {
    title: "Disengage",
    name: "description",
    content: "A social media web app designed to prevent long periods of use",
  },
];

export default function App() {
  return (
    <html lang="en">
      <head>
        <meta charSet="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <Meta />
        <Links />
      </head>
      <body className="">
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
          <main className="flex min-h-screen flex-col items-center justify-center gap-4 text-slate-900 ">
            <h1 className="text-center text-3xl font-bold md:text-4xl">
              Uh oh!
            </h1>
            <p className="text-lg md:text-xl">
              Looks like we ran into an issue!
            </p>
            <p className="text-lg italic lg:text-xl">
              {error.status || error.statusText}
            </p>
            <BackButton navTo={navTo} />
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
