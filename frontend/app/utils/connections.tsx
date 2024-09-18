import { Form } from "@remix-run/react";
import { z } from "zod";
import { default as GoogleLight } from "~/components/icons/icon.tsx";
import { cn } from "./misc";
import { Button } from "~/components/ui/Button";

export const GOOGLE_PROVIDER_NAME = "google";

export const providerNames = [GOOGLE_PROVIDER_NAME] as const;

export const ProviderNameSchema = z.enum(providerNames);

export type ProviderName = z.infer<typeof ProviderNameSchema>;

const formAction = `/auth/${GOOGLE_PROVIDER_NAME}`;

export function ProviderConnectionForm({
  btnClassName,
  iconClassName,
}: {
  btnClassName?: string;
  iconClassName?: string;
}) {
  return (
    <Form
      method="POST"
      className="flex w-full items-center justify-center gap-2"
      action={formAction}
    >
      <Button
        type="submit"
        className={cn(
          "flex h-10 w-full flex-auto items-center justify-center gap-2 self-center rounded-lg border-[1px] border-solid bg-gray-700 p-2 text-lg capitalize text-slate-50 shadow-sm shadow-gray-100 outline-none transition-all hover:bg-gray-700/95 focus-visible:ring focus-visible:ring-gray-700/80 focus-visible:ring-offset-1",
          btnClassName,
        )}
      >
        <GoogleLight
          name="web_light_sq_na"
          className={cn("fill-transparent", iconClassName)}
        />
        {GOOGLE_PROVIDER_NAME}
      </Button>
    </Form>
  );
}
