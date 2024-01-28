import { ActionFunctionArgs, redirect } from "@remix-run/cloudflare";
import {
  redirectWithErrorToast,
  redirectWithSuccessToast,
} from "~/utils/flash-session/flash-session.server";

interface GoogleLoginErrorResponse {
  ErrorMessage: string;
}

/**
 * Represents the response from a Google Login operation.
 */
interface GoogleLoginSuccessResponse {
  /**
   * Contains a message detailing the result of the login operation.
   */
  Message: string;
}

export async function action({ context }: ActionFunctionArgs) {
  return redirect(`${context.env.API_URL}/api/auth/google-sign-in`);
  // const authWithGoogle = await fetch(
  //   `${context.env.API_URL}/api/auth/google-sign-in`,
  //   {
  //     method: "GET",
  //   },
  // );

  // if (!authWithGoogle.ok) {
  //   const loginError: GoogleLoginErrorResponse = await authWithGoogle.json();
  //   const errorMessage = loginError.ErrorMessage;

  //   throw redirectWithErrorToast("/auth/login", errorMessage, context);
  // }

  // if (authWithGoogle.ok) {
  //   const loginSuccessResponse: GoogleLoginSuccessResponse =
  //     await authWithGoogle.json();

  //   throw await redirectWithSuccessToast(
  //     "/home",
  //     loginSuccessResponse.Message,
  //     context,
  //   );
  // }
}
