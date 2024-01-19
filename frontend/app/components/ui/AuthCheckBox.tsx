import * as Checkbox from "@radix-ui/react-checkbox";
import { default as Check } from "../icons/icon.tsx";
import { Fieldset } from "@conform-to/react";

/**
 * Props for the auth related checkbox. Mainly used for
 * remember me functionality when signing in.
 */
interface AuthCheckBoxProps {
  /**
   * What the text next to the checkbox will read
   */
  label: string;

  /**
   * Fields object from `Conform` that is used for setting
   * the `for` attribute on the checkbox label
   */
  fields: Fieldset<{
    password: string;
    userIdentifier: string;
    rememberMe?: boolean | undefined;
  }>;
}

export function AuthCheckbox({ label, fields }: AuthCheckBoxProps) {
  return (
    <div className=" my-3 flex items-center px-3 md:my-2">
      <Checkbox.Root
        className="h-[22px] w-[22px] appearance-none items-center justify-center rounded-[4px] border border-gray-400 bg-white outline-none hover:bg-gray-100/40 focus:shadow focus:shadow-gray-400/40"
        id="c1"
        name="rememberMe"
        value="yes"
      >
        <Checkbox.Indicator className=" text-gray-700 ">
          <Check icon="check" className="h-[18px] w-[18px]" />
        </Checkbox.Indicator>
      </Checkbox.Root>
      <label
        className="pl-[10px] text-sm leading-none text-black md:text-base "
        htmlFor={fields.rememberMe.id}
      >
        {label}
      </label>
    </div>
  );
}
