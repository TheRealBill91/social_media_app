/** Component for displaying form related errors
 *
 *  Different from @function ErrorList, which usually displays field related errors
 */
export function FormError({ formError }: { formError?: string }) {
  if (!formError) return null;
  return (
    <span
      className={
        "mt-3 self-center px-4 text-[1rem] text-red-700 transition-opacity duration-300 ease-in-out"
      }
    >
      {formError}
    </span>
  );
}
