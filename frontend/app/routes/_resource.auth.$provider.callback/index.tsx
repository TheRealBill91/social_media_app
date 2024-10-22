import { LoaderFunctionArgs, redirect } from "@remix-run/cloudflare";
import { parse } from "cookie";
import {
  lockedOutError,
  emailClaimError,
  createOrLinkError,
  externalSigninError,
  successCookie,
} from "./process-callback-responses.server";

export async function loader({ request, context }: LoaderFunctionArgs) {
  const { env } = context.cloudflare;
  const cookieHeaders = request.headers.get("Cookie");

  const cookieObj = parse(cookieHeaders || "");

  // Create `Record<string, string>` to model the cookie string above
  // Use this modeled data to run through the for loop and switch statement
  // to test each case

  for (const cookieName in cookieObj) {
    switch (cookieName) {
      case "LockedOutMessage": {
        const lockedOutMessage = cookieObj["LockedOutMessage"];

        return lockedOutError(lockedOutMessage, env);
      }
      case "EmailClaimError": {
        const emailClaimErrorMsg = cookieObj["EmailClaimError"];

        return emailClaimError(emailClaimErrorMsg);
      }
      case "CreateOrLinkError": {
        const createOrLinkErrorMsg = cookieObj["CreateOrLinkError"];

        return createOrLinkError(createOrLinkErrorMsg);
      }
      case "ExternalSigninError": {
        const externalSigninErrorMsg = cookieObj["ExternalSigninError"];

        return externalSigninError(externalSigninErrorMsg);
      }
      case "MessageCookie": {
        const successCookieMsg = cookieObj["MessageCookie"];
        return successCookie(successCookieMsg, env);
      }
      default: {
        // If the cookie headers obj contains more than just the toast-session cookie,
        // continue, otherwise redirect the user to the login page
        if (cookieObj["toast-session"] && Object.keys(cookieObj).length > 1) {
          continue;
        } else {
          throw redirect("/auth/login");
        }
      }
    }
  }
}

// This should not be visible to the user, we have this so that
// the errors thrown in the loader above will bubble up
// to the default error boundary in the root file
export default function Testing() {
  return (
    <>
      <div>Testing</div>
    </>
  );
}
