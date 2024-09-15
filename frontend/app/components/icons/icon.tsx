import { type SVGProps } from "react";
import href from "./icon.svg";
export { href };

export default function Icon({
  name,
  title,
  ...props
}: SVGProps<SVGSVGElement> & { name: IconName; title?: string }) {
  return (
    <svg {...props}>
      {title ? <title>{title}</title> : null}
      <use href={`${href}#${name}`} />
    </svg>
  );
}

export const iconNames = [
  "alert-circle",
  "check-circle-outline",
  "check",
  "cross-1",
  "default-avatar",
  "eye-none",
  "eye-open",
  "list",
  "timer-sand-empty",
  "update",
  "web_light_sq_na",
] as const;
export type IconName = (typeof iconNames)[number];
