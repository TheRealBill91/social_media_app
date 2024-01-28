import { type SVGProps } from "react";
import href from "./icon.svg";
export { href };

export default function Icon({ icon, ...props}: SVGProps<SVGSVGElement> & { icon: IconName }) {
  return (
    <svg {...props}>
      <use href={`${href}#${icon}`} />
    </svg>
  );
}

export const iconNames = [
  "check-circle-outline",
  "check",
  "eye-none",
  "eye-open",
  "google",
  "spinner",
  "timer-sand-empty",
] as const;
export type IconName = typeof iconNames[number];