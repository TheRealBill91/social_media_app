import {
  createCookieSessionStorage,
  json,
  redirect,
} from "@remix-run/cloudflare";
import {
  FlashSessionValues,
  ToastMessage,
  flashSessionValuesSchema,
} from "./schema";

export function createFlashSessionStorage(env: Env) {
  return createCookieSessionStorage({
    cookie: {
      name: "toast-session",
      sameSite: "lax",
      path: "/",
      httpOnly: true,
      secrets: [env.COOKIE_SECRET],
      secure: env.ENVIRONMENT === "production",
    },
  });
}

const flashSessionStorage = (env: Env) => createFlashSessionStorage(env);

/**
 * This helper method is used to retrieve the cookie from the request,
 * parse it, and then gets the session used to store flash information
 */
function getSessionFromRequest(request: Request, env: Env) {
  const sessionStorage = flashSessionStorage(env);

  // Gets the cookie header from the request
  const cookie = request.headers.get("Cookie");
  // Gets our session using the instance we defined above to get the session
  // information
  return sessionStorage.getSession(cookie);
}

export async function flashMessage(
  flash: FlashSessionValues,
  env: Env,
  headers?: ResponseInit["headers"],
) {
  // Creates cookie session storage with custom function that passes in Cloudflare
  // env object (so we can access the env secrets)

  const flashSessionStorage = createFlashSessionStorage(env);

  // Gets session defined above

  const session = await flashSessionStorage.getSession();

  // stores flash information into it that we pass to this function

  session.flash("toast-session", flash);

  // Creates a cookie header to store the response

  const cookie = await flashSessionStorage.commitSession(session);

  // If you've already passed in some custom headers this will create headers
  // and allow us to append additional properties
  const newHeaders = new Headers(headers);

  // Unlike JS objects headers can have multiple properties of the same name
  // So if you already passed in a Set-Cookie it will work fine and it will
  // set both cookies at the same time

  newHeaders.append("Set-Cookie", cookie);

  // returns the headers to be given to the response object
  return newHeaders;
}

/**
 * Helper method used to redirect the user to a new page with flash session values
 * @param url Url to redirect to
 * @param flash Flash session values
 * @param env Used to access Cloudflare environment variables
 * @param init Response options
 * @returns Redirect response
 */

export async function redirectWithFlash(
  url: string,
  flash: FlashSessionValues,
  env: Env,
  init?: ResponseInit,
) {
  return redirect(url, {
    ...init,
    // Remember the utility we implemented above? Well here it is!
    // It combines the headers you provided, stores the flash session info
    // and gives that back to the redirect which stores this in your web
    // browser into a cookie which will be sent with the next requests
    // automatically!
    headers: await flashMessage(flash, env, init?.headers),
  });
}

async function jsonWithFlash<T>(
  data: T,
  flash: FlashSessionValues,
  env: Env,
  init?: ResponseInit,
) {
  return json(data, {
    ...init,
    headers: await flashMessage(flash, env, init?.headers),
  });
}

/**
 * Helper method used to display a toast notification without redirection
 *
 * @param data Generic object containing the data
 * @param toast Toast message and it's type
 * @param init Additional response options (status code, additional headers etc)
 * @returns Returns data with toast cookie set
 */

export function jsonWithToast<T>(
  data: T,
  toast: ToastMessage,
  env: Env,
  init?: ResponseInit,
) {
  return jsonWithFlash(data, { toast }, env, init);
}

/**
 * Helper method used to generate a JSON response object with a success toast message.
 *
 * @param data The data to be included in the response.
 * @param text The text for the success toast notification.
 * @param init Additional response options (status code, additional headers etc)
 * @returns Returns a JSON response object with the specified success toast message.
 */

export function jsonWithSuccess<T>(
  data: T,
  text: string,
  env: Env,
  init?: ResponseInit,
  duration?: number,
) {
  return jsonWithToast(data, { text, type: "success", duration }, env, init);
}

/**
 * Helper method used to generate a JSON response object with an error toast message.
 *
 * @param data The data to be included in the response.
 * @param text The text for the error toast notification.
 * @param init Additional response options (status code, additional headers etc)
 * @returns Returns a JSON response object with the specified error toast message.
 */

export function jsonWithError<T>(
  data: T,
  text: string,
  env: Env,
  init?: ResponseInit,
  duration?: number,
) {
  return jsonWithToast(data, { text, type: "error", duration }, env, init);
}

/**
 * Helper method used to generate a JSON response object with an info toast message.
 *
 * @param data The data to be included in the response.
 * @param text The message for the info toast notification.
 * @param init Additional response options (status code, additional headers etc)
 * @returns Returns a JSON response object with the specified info toast message.
 */
export function jsonWithInfo<T>(
  data: T,
  text: string,
  env: Env,
  init?: ResponseInit,
  duration?: number,
) {
  return jsonWithToast(data, { text, type: "info", duration }, env, init);
}

/**
 * Helper method used to redirect the user to a new page with a toast notification
 * If thrown it needs to be awaited
 * @param url Redirect URL
 * @param env Used to access Cloudflare environment variables
 * @param init Additional response options
 * @returns Returns a redirect response with toast stored in the session
 */

export function redirectWithToast(
  url: string,
  toast: ToastMessage,
  env: Env,
  init?: ResponseInit,
) {
  // We provide a simple utility around redirectWithFlash to make it
  // less generic and toast specific, add more methods if you need them!
  return redirectWithFlash(url, { toast }, env, init);
}

/** Helper utility to redirect with an error toast shown to the user */
export function redirectWithErrorToast(
  redirectUrl: string,
  text: string,
  env: Env,
  init?: ResponseInit,
  duration?: number,
) {
  return redirectWithToast(
    redirectUrl,
    { text, type: "error", duration },
    env,
    init,
  );
}

/** Helper utility to redirect with success toast shown to the user */
export function redirectWithSuccessToast(
  redirectUrl: string,
  text: string,
  env: Env,
  init?: ResponseInit,
  duration?: number,
) {
  return redirectWithToast(
    redirectUrl,
    { text, type: "success", duration },
    env,
    init,
  );
}

/**
 * Helper method used to get the toast data from the current request and purge the flash storage from the session
 * @param request Current request
 * @returns Returns the the toast notification if exists, undefined otherwise and the headers needed to purge it from the session
 */

export async function getToast(
  request: Request,
  env: Env,
): Promise<{ toast: ToastMessage | undefined; headers: Headers }> {
  const flashSession = flashSessionStorage(env);
  const session = await getSessionFromRequest(request, env);
  const result = flashSessionValuesSchema.safeParse(
    session.get("toast-session"),
  );
  const flash = result.success ? result.data : undefined;
  const headers = new Headers({
    "Set-Cookie": await flashSession.commitSession(session),
  });
  const toast = flash?.toast;
  return { toast, headers };
}
