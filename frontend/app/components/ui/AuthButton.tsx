import { tw } from "~/utils/tw-identity-helper";
import { AuthButtonProps } from "../../../types/auth-button-props";

import { default as Spinner } from "../icons/icon.tsx";

export function AuthButton({ name, submitting }: AuthButtonProps) {
  return (
    <button
      aria-disabled={submitting}
      className={tw`flex w-full items-center justify-evenly self-center rounded-lg border-[1px]  border-solid bg-gray-700 p-2 text-lg text-slate-50  shadow-sm shadow-gray-100  outline-none transition-all hover:bg-gray-600 focus:ring focus:ring-gray-700/80  focus:ring-offset-1 aria-disabled:cursor-not-allowed aria-disabled:border-gray-300 aria-disabled:bg-gray-400 aria-disabled:text-gray-300 `}
      type="submit"
    >
      {name}
      {submitting ? (
        <Spinner
          icon="spinner"
          className="mr-3 h-5 w-5 animate-spin fill-transparent text-white"
        />
      ) : null}
    </button>
  );
}
