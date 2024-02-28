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

  // for testing
  /*  const cookie = {
    UserId: "52e78afe-942d-40c2-8ad1-48ab4f6108ac",
    CreateOrLinkError: "We ran into an issue creating or linking your accounts",
    "toast-session": "e30=.vNF8ODtkB1iCuSh4vYIaV781DieGg5XAPdLj64Knzqk",
    auth: "CfDJ8EDggVymMgtJjxRENSEFTNE46fUNMuXkjyo-Ab2-ShC7qXe2J16sfvfsKedArHyjLksB5_wjH-D1hy6-c-t41_TLkMuRFnHJCo4UoO4jHtPz033Y3dFqThqmB4gsOURyoxUsavqO8NCUm7W05ZLls-gG7BaQtVwbEdSC7WBqwUNvaGxehQJkAsffb_uSpgS5mXK0iFp8ZbWy-qU3s6jvvL9JkeJEaA723sksCZcIzmZtuL1gCWMiTyyD555-HNvDpRGuRBjkVNKbuXOBfJ4ro9uo_JVzdZAub84MbSyskEKDN4p1EIInYvMZn7ypBTn_uHVcMejg4hdXoy8zfNtrN9-l4UGIJNznXy9TxcTVLfENLykkeVZc2d8cI4U5xQQ_Hwwg5dOvJvszsuZQ4EE0nW8-UYxll110vqVhLU3kKvxFprv5cdJbRICRSRQleQ4-pMQsb5AstWKtG42x_3tljAvTR6ULgpZ-9GGeUIJVUJN67bBsvKt8hs-8gOhcUgHB0MBCc2oBVng1HvUnlhOXxHqHJoCxPB0zFFbYntD4KP219Rnf5hAzESvFthLTvHJ4gDm3uccEpiNpZzojDRnO3WMkRAyQTL3y2wSV7omQgPxw7eKXa7tlyrRmIBfIRZU6yglDM1tVv4-hw4bGZo4hn23yQTYCfskmoY9t0pTLKlfifmEPOKmUd_bruBjjys5kiSHbrgVL0Epzp1AALn-YshEIbPdhxJieTBzoHFhbmdol9V_FjbCH_jBLUt1wtluM6VUUBfzYv-QmoMoebp6MYOLIq3wf0cTTXPE33stHB606-2epqhy23KfJxnyiNGl0VyN8Y5-wJg8tHcK26WKy1P7Eel_aNUz9EPHL8hYDrkSSHEtD_XkjVEK6DBDK3d1yFW00jR0hepn7xBlzpZYEbJfY1QEzTxeEdL2sY4J1FPYgQ4S3a-O-c4ZUADladQefT3bAHU1mXEpp91mb2ayzppGokLS3ntSpwWZLnokwSrJ17s9qI_YvNJtC62HpH5VqsQ",
  }; */

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
