import { Link } from "react-router-dom";
import { BackButtonProps } from "types/back-button-props";
import { tw } from "~/utils/tw-identity-helper";

export function BackButton({ navTo, twStyles }: BackButtonProps) {
  const extractedNavName = navTo.slice(1);
  const navName = extractedNavName.length > 0 ? extractedNavName : "home";
  console.log(twStyles);
  return (
    <Link
      to={navTo}
      className={tw`${twStyles} rounded-lg focus:ring focus:ring-gray-800/80 focus:ring-offset-2`}
    >
      <button
        type="button"
        className=" rounded-lg border border-solid border-gray-800 px-3 py-1 text-gray-700  dark:border-slate-100 dark:text-slate-100 dark:hover:border-slate-50 dark:hover:text-slate-50"
      >
        Back to {navName}
      </button>
    </Link>
  );
}
