import { Form, useNavigation, Link } from "@remix-run/react";
import { SheetTrigger, SheetContent, SheetClose, Sheet } from "./Sheet";
import { useEffect, useState } from "react";
import { default as HamburgerMenu } from "~/components/icons/icon.tsx";
import type { ProfileSuccessResponse as UserInfo } from "~/utils/auth.server";
import { Separator } from "./Separator";
import { Avatar, AvatarFallback, AvatarImage } from "./Avatar";
import default_avatar from "~/../assets/default-avatar.png";

interface HeaderProps {
  userInfo: UserInfo | undefined;
}

export function MobileNav({ userInfo }: HeaderProps) {
  const [open, setOpen] = useState(false);

  const navigation = useNavigation();

  const profileURL = userInfo?.Photo_url ? userInfo.Photo_url : default_avatar;

  // prevents mobile menu from closing until the logout action is complete
  useEffect(() => {
    if (navigation.state === "idle") {
      setOpen(false);
    }
  }, [navigation]);

  return (
    <>
      <Sheet open={open} onOpenChange={setOpen} key={"right"}>
        <SheetTrigger asChild>
          <button className="lg:hidden">
            <HamburgerMenu className="size-8" name="list" />
          </button>
        </SheetTrigger>
        <SheetContent side={"right"} className="bottom-0">
          <div className="flex h-full flex-1 flex-col">
            <nav className="mt-12 flex flex-1 flex-col items-center gap-6">
              {userInfo ? (
                <Form action="/auth/logout" method="POST">
                  <button className="text-2xl" type="submit">
                    Logout
                  </button>
                </Form>
              ) : (
                <SheetClose asChild>
                  <Link
                    prefetch="intent"
                    className="text-2xl text-gray-700 transition-colors hover:text-black"
                    to="/auth/login"
                  >
                    Login
                  </Link>
                </SheetClose>
              ) : null}

              {location.pathname !== "/auth/signup" ? (
                <SheetClose asChild>
                  <Link
                    prefetch="intent"
                    className="text-2xl text-gray-700 transition-colors hover:text-black"
                    to="/auth/signup"
                  >
                    Signup
                  </Link>
                </SheetClose>
              ) : null}
              {userInfo ? (
                <SheetClose asChild>
                  <Link
                    prefetch="intent"
                    className="text-2xl text-gray-700 transition-colors hover:text-black"
                    to="/settings"
                  >
                    Settings
                  </Link>
                </SheetClose>
              ) : null}
            </nav>
            {userInfo ? (
              <div className="flex items-center justify-center gap-4">
                <p className="font-semibold">{userInfo?.UserName}</p>
                <Separator orientation="vertical" />
                <Avatar className="size-8">
                  <AvatarImage src={profileURL} />
                  <AvatarFallback>FILLER</AvatarFallback>
                </Avatar>
              </div>
            ) : null}
          </div>
        </SheetContent>
      </Sheet>
    </>
  );
}
