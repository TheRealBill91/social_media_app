import { cn } from "~/utils/misc.tsx";
import { AuthButtonProps } from "../../../types/auth-button-props";

import { default as Spinner } from "../icons/icon.tsx";

export function AuthButton({ name, submitting, className }: AuthButtonProps) {
  return (
    <button
      aria-disabled={submitting}
      className={cn(
        "flex-1 self-center rounded-lg border-[1px] border-solid bg-gray-700 p-2 text-xl text-slate-50  shadow-sm shadow-gray-100  outline-none transition-all hover:bg-gray-600 focus:ring focus:ring-gray-700/80  focus:ring-offset-1 aria-disabled:cursor-not-allowed aria-disabled:border-gray-300 aria-disabled:bg-gray-400 aria-disabled:text-gray-300",
        className,
      )}
      type="submit"
    >
      {name}
      {submitting ? (
        <Spinner
          icon="spinner"
          className="mr-3 size-5 animate-spin fill-transparent text-white"
        />
      ) : null}
    </button>
  );
}
