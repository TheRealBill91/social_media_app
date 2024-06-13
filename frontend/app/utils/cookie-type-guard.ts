import { emailConfirmationFailure } from "./cookie.server";

/** Type guard to check the expected parsed cookie obj type at runtime */
function isRecordType(obj: unknown): obj is Record<string, string> {
  return (
    obj !== null &&
    typeof obj === "object" &&
    !Array.isArray(obj) &&
    Object.keys(obj).every(
      (key) =>
        typeof key === "string" &&
        typeof obj[key as keyof typeof obj] === "string",
    )
  );
}

/** Function that parses the cookie using the`isRecordType`typeguard function 
   * 
   * Neccessary when using remix.run cookie parse utility along with the 
   *`@typescript-eslint/recommended-type-checked`plugin in order to avoid
   unsafe assignment of`any`
  */
export async function getParsedCookie(
  cookieHeader: string | null,
): Promise<Record<string, string>> {
  const rawCookieObj: unknown =
    await emailConfirmationFailure.parse(cookieHeader);

  if (!isRecordType(rawCookieObj)) {
    throw new Error("Invalid parsed cookie format");
  }

  return rawCookieObj;
}
