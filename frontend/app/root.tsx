import type { LinksFunction } from "@remix-run/cloudflare";
import styles from "./tailwind.css";
import {
  Links,
  LiveReload,
  Meta,
  Outlet,
  Scripts,
  ScrollRestoration,
} from "@remix-run/react";

import {
  href as Spinner,
  href as Check,
  href as EyeOpen,
  href as EyeNone,
} from "./components/icons/icon.tsx";

export const links: LinksFunction = () => [
  { rel: "stylesheet", href: styles },
  { rel: "preload", href: Spinner, as: "image" },
  { rel: "preload", href: Check, as: "image" },
  { rel: "preload", href: EyeOpen, as: "image" },
  { rel: "preload", href: EyeNone, as: "image" },
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
