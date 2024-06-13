import { useFetcher } from "@remix-run/react";
import { Label } from "~/components/ui/Label";
import { Textarea } from "~/components/ui/TextArea";
// import { z } from "zod";
import { Input } from "~/components/ui/Input";
import { Combobox } from "~/components/ui/Combobox";
import { FormDescription } from "~/components/Forms";
import { Button } from "~/components/ui/Button";

// const profileFormSchema = z.object({
//   bio: z.string({ required_error: "Bio is required" }).max(160).min(4),
//   url: z.string().url({ message: "Please enter a valid URL" }).optional(),
// });

export default function ProfileForm() {
  const fetcher = useFetcher();

  return (
    <fetcher.Form>
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
          />
        </div>

        <div className="space-y-2">
          <Label className="text-sm capitalize" htmlFor="location">
            location
          </Label>
          <Input type="text" placeholder="city"></Input>
        </div>
        <Combobox disabled={false} />

        <div className="space-y-2">
          <Label className="text-sm">URL</Label>
          <FormDescription className="text-[0.8rem] text-muted-foreground">
            Add a URL that can be displayed publicly on your profile
          </FormDescription>
          <Input className="" type="text" placeholder="www.yoururl.com"></Input>
        </div>
      </fieldset>
      <Button aria-disabled type="submit" className="mt-6 capitalize">
        update profile
      </Button>
    </fetcher.Form>
  );
}
