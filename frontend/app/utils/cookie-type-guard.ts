import { Cookie } from "@remix-run/cloudflare";

/** Type guard to check the expected parsed cookie obj type at runtime
 *
 * Note: This function is not currently being used, as we have opted
 * to turn off `@typescript-eslint/no-unsafe-member-access` and
 * `@typescript-eslint/no-unsafe-assignment`.
 */
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
   * Neccessary when using `remix.run` cookie parse utility along with the 
   *`@typescript-eslint/recommended-type-checked`plugin in order to avoid
   unsafe assignment of`any`
   *
   * Note: This function is not currently being used, as we have opted
   * to turn off `@typescript-eslint/no-unsafe-member-access` and 
   * `@typescript-eslint/no-unsafe-assignment`.
   */
export async function getParsedCookie(
  cookieHeader: string | null,
  cookieType: Cookie,
): Promise<Record<string, string>> {
  const rawCookieObj: unknown = await cookieType.parse(cookieHeader);

  console.log("cookie obj: " + JSON.stringify(rawCookieObj));

  if (!isRecordType(rawCookieObj)) {
    throw new Error("Invalid parsed cookie format");
  }

  return rawCookieObj;
}
