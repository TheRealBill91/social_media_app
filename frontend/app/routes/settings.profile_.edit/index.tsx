import { Separator } from "~/components/ui/Separator";
import ProfileForm from "./ProfileForm";

export default function Profile() {
  return (
    <div className="flex-1 outline outline-red-500 lg:ml-6 lg:max-w-[70%]">
      <h3 className="text-lg">Profile</h3>
      <Separator className="my-3" />
      <ProfileForm />
    </div>
  );
}
