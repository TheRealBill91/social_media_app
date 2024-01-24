import { useId, useRef } from "react";
import { Checkbox, type CheckboxProps } from "./Checkbox.tsx";
import { Fieldset } from "@conform-to/react";

/**
 * Props for the auth related checkbox. Mainly used for
 * remember me functionality when signing in.
 */
interface AuthCheckBoxProps {
  checkBoxProps: CheckboxProps;
  className: string | undefined;
  fields: Fieldset<{
    password: string;
    userIdentifier: string;
    rememberMe?: boolean | undefined;
  }>;
}

export function RememberMeCheckbox({
  fields,
  checkBoxProps,
  className,
}: AuthCheckBoxProps) {
  const fallbackId = useId();

  const buttonRef = useRef<HTMLButtonElement>(null);

  const id = checkBoxProps.id ?? checkBoxProps.name ?? fallbackId;

  return (
    <div className="my-3 flex items-center px-4 md:my-2">
      <Checkbox
        {...checkBoxProps}
        ref={buttonRef}
        id={id}
        type="button"
        className={className}
      />
      <label
        className="pl-[10px] text-sm leading-none text-black md:text-base "
        htmlFor={fields.rememberMe.id}
      >
        Remember me?
      </label>
    </div>
  );
}
