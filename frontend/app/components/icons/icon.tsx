import { type SVGProps } from "react";
import href from "./icon.svg";
export { href };

export default function Icon({
  icon,
  ...props
}: SVGProps<SVGSVGElement> & { icon: IconName }) {
  return (
    <svg {...props}>
      <use href={`${href}#${icon}`} />
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
