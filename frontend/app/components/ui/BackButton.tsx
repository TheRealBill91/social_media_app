import { Link } from "react-router-dom";
import { BackButtonProps } from "types/back-button-props";
import { tw } from "~/utils/tw-identity-helper";

export function BackButton({ navTo, twStyles }: BackButtonProps) {
  const extractedNavName = navTo.slice(1);
  const navName = extractedNavName.length > 0 ? extractedNavName : "home";
  return (
    <>
      <Link
        to={navTo}
        className={tw`${twStyles} rounded-lg bg-gray-700 px-4 py-2 text-gray-100 outline-none hover:bg-gray-700/90 focus-visible:ring focus-visible:ring-gray-700/80 focus-visible:ring-offset-2 `}
      >
        {" "}
        Back to {navName}
      </Link>
    </>
  );
}
