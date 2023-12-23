import { tw } from "../../utils/tw-identity-helper";
import { AuthButtonProps } from "../../../types/auth-button-props";

import { default as Spinner } from "../icons/icon.tsx";

export function AuthButton({ name, submitting }: AuthButtonProps) {
  return (
    <button
      aria-disabled={submitting}
      className={tw`${
        submitting ? "w-auto gap-6 pl-4" : ""
      } flex w-[140px] items-center justify-evenly  self-center rounded-lg border-[1px] border-solid border-gray-400 bg-white  p-2 text-lg text-gray-700 shadow-sm shadow-gray-100  transition-all hover:border-[1px] hover:border-solid hover:border-gray-900 hover:text-gray-900 aria-disabled:border-gray-300 aria-disabled:text-gray-300 dark:bg-slate-100 dark:text-slate-800 dark:aria-disabled:text-gray-300`}
      type="submit"
    >
      {name}
      {submitting ? (
        <Spinner
          icon="spinner"
          className="mr-3 h-5 w-5 animate-spin  text-gray-600"
        />
      ) : null}
    </button>
  );
}
