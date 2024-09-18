import { Separator } from "~/components/ui/Separator";
import { Outlet, useRouteLoaderData } from "@remix-run/react";
import { type loader as rootLoader } from "~/root.tsx";
import { SidebarNav } from "./SidebarNav";
import { LoaderFunctionArgs, MetaFunction, json } from "@remix-run/cloudflare";
import { requireAuthUser } from "~/utils/auth.server";

export const meta: MetaFunction = () => {
  return [{ title: "Disengage | Settings" }];
};

export function loader({ request }: LoaderFunctionArgs) {
  requireAuthUser(request);

  return json({});
}

export default function Profile() {
  const data = useRouteLoaderData<typeof rootLoader>("root");
  const userInfo = data?.userInfo;

  console.log(userInfo);

  const sideBarNavItems = [
    {
      title: "Profile",
      href: "/settings/profile",
    },
  ];

  return (
    <main className="flex flex-1 flex-col p-6">
      <h1 className="semi-bold text-2xl lg:my-3 lg:ml-2">Settings</h1>
      <Separator className="my-3" />
      <div className="flex flex-1 flex-col gap-3 p-4 pt-6 lg:flex-row lg:gap-0">
        <aside className="lg:w-1/5">
          <SidebarNav items={sideBarNavItems} />
        </aside>
        <Outlet />
      </div>
    </main>
  );
}
