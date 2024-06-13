import { Link } from "@remix-run/react";
import { UserNav } from "./UserNav";
import type { ProfileSuccessResponse as UserInfo } from "~/utils/auth.server";
import { MobileNav } from "./MobileNav";

interface HeaderProps {
  userInfo: UserInfo | undefined;
}

// const nonMobileNavItems = [{ id: 1, name: "Settings", location: "/settings" }];

export function Header({ userInfo }: HeaderProps) {
  return (
    <>
      <header className="sticky top-0 z-30 flex w-full flex-col border-b border-gray-200/50 bg-gray-100/30 bg-opacity-30 px-6 backdrop-blur-md lg:mx-auto">
        <div className="flex h-[60px] w-full items-center justify-between py-2">
          <Link to={"/"} className="">
            <h1 className="self-center text-2xl capitalize text-gray-700 transition-colors focus-visible:text-gray-600 lg:ml-2 lg:hover:text-gray-600 lg:hover:transition-colors">
              disengage
            </h1>
          </Link>
          <div className="hidden pr-4 lg:flex">
            {userInfo ? <UserNav userInfo={userInfo} /> : null}
          </div>
          <MobileNav userInfo={userInfo} />
        </div>
      </header>
    </>
  );
}
