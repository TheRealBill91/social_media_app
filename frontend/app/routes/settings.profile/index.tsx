import { Label } from "~/components/ui/Label";
import { json, useRouteLoaderData } from "@remix-run/react";
import { Combobox } from "~/components/ui/Combobox";
import { FormDescription } from "~/components/Forms";
import { Input } from "~/components/ui/Input";
import { Textarea } from "~/components/ui/TextArea";
import { type loader as rootLoader } from "~/root.tsx";
import { ProfileSuccessResponse as UserInfo } from "~/utils/auth.server";
import { Separator } from "~/components/ui/Separator";

export function loader() {
  return json({});
}

export default function ProfileSettings() {
  const data = useRouteLoaderData<typeof rootLoader>("root");
  const userInfo: UserInfo | undefined = data?.userInfo;

  return (
    <div className="flex-1 lg:ml-6 lg:max-w-[70%]">
      <h3 className="text-lg">Profile</h3>
      <Separator className="my-3" />
      <ProfileInfo userInfo={userInfo} />
    </div>
  );
}

interface IProps {
  userInfo: UserInfo | undefined;
}

function ProfileInfo({ userInfo }: IProps) {
  const bioValue = userInfo?.Bio ? userInfo.Bio : "";

  return (
    <fieldset className="space-y-6">
      <div>
        <Label className="text-sm capitalize" htmlFor="bio">
          bio
        </Label>
        <Textarea
          className="mt-1 resize-none"
          id="bio"
          name="name"
          placeholder="Bio"
          disabled
          value={bioValue}
        />
      </div>

      <div className="space-y-2">
        <Label className="text-sm capitalize" htmlFor="location">
          location
        </Label>
        <Input type="text" placeholder="city" disabled></Input>
      </div>
      <Combobox disabled={true} />

      <div className="space-y-2">
        <Label className="text-sm">URL</Label>
        <FormDescription className="text-[0.8rem] text-muted-foreground">
          Add a URL that can be displayed publicly on your profile
        </FormDescription>
        <Input
          className=""
          type="text"
          placeholder="www.yoururl.com"
          disabled
        ></Input>
      </div>
    </fieldset>
  );
}
