/* import { createCookieSessionStorage } from "@remix-run/cloudflare-pages";
import { AppLoadContext } from "@remix-run/cloudflare"; */
import { z } from "zod";

type ToastTypes =
  | "normal"
  | "action"
  | "success"
  | "info"
  | "warning"
  | "error"
  | "loading"
  | "default";

// const FLASH_SESSSION = "flash";

/* export function createFlashSessionStorage(context) {
  return createCookieSessionStorage({
    cookie: {
      name: FLASH_SESSSION,
      sameSite: "lax",
      path: "/",
      httpOnly: true,
      secrets: [context.env.COOKIE_SECRET],
      secure: [context.env.ENVIRONMENT === "production"],
    },
  });
} */

const toastMessageSchema = z.object({
  // message to diplay to the user
  text: z.string(),

  // type of notification
  type: z.custom<ToastTypes>(),
});

// Infers the type for later usage and type safety
// type ToastMessage = z.infer<typeof toastMessageSchema>;

const flashSessionValuesSchema = z.object({
  // validation schema from above
  toast: toastMessageSchema.optional(),
});

type FlashSessionValues = z.infer<typeof flashSessionValuesSchema>;

export async function flashMessage(
  flash: FlashSessionValues,
  headers?: ResponseInit["headers"],
) {
  // Creates cookie session storage with custom function that passes in Cloudflare
  // context (so we can access the env secrets)

  // const flashSessionStorage = createFlashSessionStorage(context);

  // Gets session defined above

  // const session = await flashSessionStorage.getSession();

  // stores flash information into it that we pass to this function

  // session.flash(FLASH_SESSSION, flash);

  // Creates a cookie header to store the response

  // const cookie = await flashSessionStorage.commitSession(session);

  // If you've already passed in some custom headers this will create headers
  // and allow us to append additional properties
  const newHeaders = new Headers(headers);

  // Unlike JS objects headers can have multiple properties of the same name
  // So if you already passed in a Set-Cookie it will work fine and it will
  // set both cookies at the same time

  // newHeaders.append("Set-Cookie", cookie);

  // returns the headers to be given to the response object
  return newHeaders;
}
