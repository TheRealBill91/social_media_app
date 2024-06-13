import { LoaderFunctionArgs, MetaFunction, json } from "@remix-run/cloudflare";
import { useRouteLoaderData } from "@remix-run/react";
import { type loader as rootLoader } from "~/root.tsx";
import { requireAuthUser } from "~/utils/auth.server";

export const meta: MetaFunction = () => {
  return [{ title: "Disengage | Home" }];
};

export function loader({ request }: LoaderFunctionArgs) {
  requireAuthUser(request);

  return json({});
}

export default function HomeFeed() {
  const data = useRouteLoaderData<typeof rootLoader>("root");
  const userInfo = data?.userInfo;

  return (
    <main className="flex flex-1 flex-col items-center justify-center bg-[#ffffff]">
      {userInfo ? (
        <h3 className="mb-4 border-b border-gray-600 px-4 py-2 text-2xl text-gray-600">
          Hi, {userInfo.UserName}
        </h3>
      ) : null}
      <h3 className="text-3xl">Home feed!</h3>
    </main>
  );
}
