import { Link } from "@remix-run/react";
import { default as HamburgerMenu } from "~/components/icons/icon.tsx";

interface HeaderProps {
  toggleMobileMenu: () => void;
}

export function Header({ toggleMobileMenu }: HeaderProps) {
  console.log();
  return (
    <>
      <header className="sticky top-0 z-30 flex w-full flex-col border-b border-gray-200/50 bg-gray-100/30 bg-opacity-30 px-6 backdrop-blur-md">
        <div className="flex h-[60px] w-full items-center justify-between py-2">
          <Link to={"/"} className="">
            <h1 className="self-center text-2xl capitalize text-gray-700 transition-colors focus-visible:text-gray-600 lg:ml-2 lg:hover:text-gray-600 lg:hover:transition-colors">
              disengage
            </h1>
          </Link>
          <button onClick={toggleMobileMenu} className="lg:hidden">
            <HamburgerMenu className="size-8" icon="list" />
          </button>
        </div>
      </header>
    </>
  );
}
