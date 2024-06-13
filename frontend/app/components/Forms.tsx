import { ReactNode, useId, useRef } from "react";
import { Checkbox, type CheckboxProps } from "./ui/Checkbox.tsx";
import { useInputEvent } from "@conform-to/react";
import { Label } from "./ui/Label.tsx";
import { cn } from "~/utils/misc.tsx";

interface FormDescriptionTypes {
  children: ReactNode;
  className?: string | undefined;
}

export function FormDescription({ children, className }: FormDescriptionTypes) {
  return <p className={className}>{children}</p>;
}

/**
 * Props for the auth related checkbox. Mainly used for
 * remember me functionality when signing in.
 */
interface AuthCheckBoxProps {
  checkBoxProps: CheckboxProps;
  className?: string | undefined;
  labelProps: React.JSX.IntrinsicElements["label"];
}

export function RememberMeCheckbox({
  checkBoxProps,
  className,
  labelProps,
}: AuthCheckBoxProps) {
  const fallbackId = useId();

  const buttonRef = useRef<HTMLButtonElement>(null);

  // To emulate native events that Conform listens to (only for for Conform 0.9.1, not v1)
  const control = useInputEvent({
    // Retrieve the checkbox element by name instead as Radix does not expose the internal checkbox element
    // See https://github.com/radix-ui/primitives/discussions/874
    ref: () =>
      buttonRef.current?.form?.elements.namedItem(checkBoxProps.name ?? ""),
    onFocus: () => buttonRef.current?.focus(),
  });

  const id = checkBoxProps.id ?? checkBoxProps.name ?? fallbackId;

  return (
    <div className="my-6 flex items-center px-2">
      <Checkbox
        id={id}
        ref={buttonRef}
        aria-describedby=""
        {...checkBoxProps}
        type="button"
        className={className}
        onCheckedChange={(state) => {
          control.change(Boolean(state.valueOf()));
          checkBoxProps.onCheckedChange?.(state);
        }}
        onFocus={(event) => {
          control.focus();
          checkBoxProps.onFocus?.(event);
        }}
        onBlur={(event) => {
          control.blur();
          checkBoxProps.onBlur?.(event);
        }}
      />
      <Label
        htmlFor={id}
        {...labelProps}
        className="pl-[10px] text-base leading-none text-black md:text-base"
      />
    </div>
  );
}

export type ListOfErrors = Array<string | null | undefined> | null | undefined;

export function ErrorList({
  id,
  errors,
  className,
}: {
  errors?: ListOfErrors;
  id?: string;
  className?: string;
}) {
  const errorsToRender = errors?.filter(Boolean);
  if (!errorsToRender?.length) return null;

  return (
    <ul id={id} className={cn("flex flex-col gap-1", className)}>
      {errorsToRender.map((error) => (
        <li key={error} className="text-foreground-destructive text-sm">
          {error}
        </li>
      ))}
    </ul>
  );
}

// function Field({
//   labelProps,
//   inputProps,
//   errors,
//   className,
// }: {
//   labelProps: React.LabelHTMLAttributes<HTMLLabelElement>;
//   inputProps: React.InputHTMLAttributes<HTMLInputElement>;
//   errors?: ListOfErrors;
//   className?: string;
// }) {
//   const fallbackId = useId();
//   const id = inputProps.id ?? fallbackId;
// }
