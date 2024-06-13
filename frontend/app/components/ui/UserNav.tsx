import { useRef } from "react";
import { Avatar, AvatarFallback, AvatarImage } from "./Avatar";
import { Button } from "./Button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuLabel,
  DropdownMenuTrigger,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuSeparator,
} from "./DropdownMenu";
import default_avatar from "/assets/default-avatar.png";
import { Form, Link, useSubmit } from "@remix-run/react";
import type { ProfileSuccessResponse as UserInfo } from "~/utils/auth.server";

export function UserNav({ userInfo }: { userInfo: UserInfo | undefined }) {
  const submit = useSubmit();
  const formRef = useRef<HTMLFormElement>(null);

  const profileURL = userInfo?.Photo_url ? userInfo.Photo_url : default_avatar;

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button
          variant="ghost"
          className="relative size-8 rounded-full focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-gray-300 focus-visible:ring-offset-1"
        >
          <Avatar className="size-8">
            <AvatarImage src={profileURL} />
            <AvatarFallback>FILLER</AvatarFallback>
          </Avatar>
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent className="w-56 bg-white" align="end" forceMount>
        <DropdownMenuLabel className="font-normal">
          <div className="flex flex-col space-y-1">
            <p className="text-sm font-semibold">{userInfo?.UserName}</p>
            <p className="text-xs text-muted-foreground">
              {userInfo?.FirstName} {userInfo?.LastName}
            </p>
          </div>
        </DropdownMenuLabel>
        <DropdownMenuSeparator />
        <DropdownMenuGroup>
          <Link prefetch="intent" to={"/settings/profile"}>
            <DropdownMenuItem className="cursor-pointer rounded font-normal transition-all hover:bg-gray-100">
              Settings
            </DropdownMenuItem>
          </Link>
          <DropdownMenuSeparator />
          <DropdownMenuItem
            className="rounded transition-all hover:bg-gray-100"
            asChild
            onSelect={(event) => {
              event.preventDefault();
              submit(formRef.current);
            }}
          >
            <Form action="/auth/logout" method="POST" ref={formRef}>
              <button className="" type="submit">
                Logout
              </button>
            </Form>
          </DropdownMenuItem>
        </DropdownMenuGroup>
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
