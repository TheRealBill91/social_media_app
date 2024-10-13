import { ReactNode, useId, useRef } from "react";
import { Checkbox, type CheckboxProps } from "./ui/Checkbox.tsx";
import { FieldConfig, conform, useInputEvent } from "@conform-to/react";
import { Label } from "./ui/Label.tsx";
import { cn } from "~/utils/misc.tsx";
import { PasswordRevealBtn } from "./ui/PasswordRevealBtn.tsx";
import { usePasswordReveal } from "~/utils/hooks/usePasswordReveal.ts";
import { Input } from "./ui/Input.tsx";

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
    <div className="flex items-center px-2">
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

/**
 * @summary Props for the input in {@link RevealInputField}
 *
 * @extends React.InputHTMLAttributes<HTMLInputElement>
 * @property {@link passwordField}
 * @property {@link baseClass}
 * @property {@link inputErrorsClass}
 */
interface RevealInputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  passwordField: FieldConfig<string>;
  baseClass?: string;
  inputErrorsClass?: string;
}

/**
 * @summary Props for the label in {@link RevealInputField}
 *
 * @extends React.LabelHTMLAttributes<HTMLLabelElement>
 * @property {@link children}
 * @property {@link baseClass}
 * @property {@link errorsClass}
 */
interface RevealLabelProps extends React.LabelHTMLAttributes<HTMLLabelElement> {
  children: ReactNode;
  baseClass?: string;
  errorsClass?: string;
}

/**
 * @summary Error props for the field input that uses the input reveal toggle
 * @property {@link errors}
 * @property {@link errorClass}
 */
interface RevealErrorProps {
  errors?: ListOfErrors;
  errorClass?: string;
}

/**
 *
 * @summary Commonly used for revealing sensitive information in the user input
 * like with passwords
 *
 * @param {RevealLabelProps} {@link RevealLabelProps}
 * @param {InputProps} {@link RevealInputProps}
 * @param {RevealErrorProps} {@link RevealErrorProps}
 *
 */
export function RevealInputField({
  labelProps,
  inputProps,
  errorProps,
  className,
}: {
  labelProps: RevealLabelProps;
  inputProps: RevealInputProps;
  errorProps: RevealErrorProps;
  className?: string;
}) {
  const passwordReveal = usePasswordReveal();
  const passwordInputType = passwordReveal.showPassword ? "text" : "password";

  const fallbackId = useId();

  const id = inputProps.id ?? fallbackId;
  const errorId = errorProps.errors?.length ? `${id}-error` : undefined;

  return (
    <div className={cn("mt-8 flex flex-col items-center gap-[6px]", className)}>
      <div className="relative w-full">
        <Input
          {...conform.input(inputProps.passwordField, {
            type: passwordInputType,
          })}
          id={id}
          className={cn(
            inputProps.baseClass,
            errorProps.errors?.length ? inputProps.inputErrorsClass : "",
          )}
          placeholder="test"
          aria-invalid={errorId ? true : undefined}
          aria-describedby={errorId}
        />
        <PasswordRevealBtn
          showPassword={passwordReveal.showPassword}
          togglePassword={passwordReveal.togglePassword}
        />
        <Label
          htmlFor={id}
          className={cn(
            labelProps.baseClass,
            errorProps.errors?.length ? labelProps.errorsClass : "",
          )}
        >
          {labelProps.children}
        </Label>
      </div>
      {errorId ? (
        <ErrorList
          className={errorProps.errorClass}
          errors={errorProps.errors}
          id={errorId}
        />
      ) : null}
    </div>
  );
}

/**
 * @summary Props for the input in {@link Field}
 *
 * @extends React.InputHTMLAttributes<HTMLInputElement>
 * @property {@link passwordField}
 * @property {@link baseClass}
 * @property {@link inputErrorsClass}
 */
interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  field: FieldConfig<string>;
  baseClass?: string;
  inputErrorsClass?: string;
}

/**
 * @summary Props for the label in {@link Field}
 *
 * @extends React.LabelHTMLAttributes<HTMLLabelElement>
 * @property {@link children}
 * @property {@link baseClass}
 * @property {@link errorsClass}
 */
interface LabelProps extends React.LabelHTMLAttributes<HTMLLabelElement> {
  children: ReactNode;
  baseClass?: string;
  errorsClass?: string;
}

/**
 * @summary Error props for {@link Field}
 * @property {@link errors}
 * @property {@link errorClass}
 */
interface ErrorProps {
  errors?: ListOfErrors;
  errorClass?: string;
}

export function Field({
  labelProps,
  inputProps,
  errorProps,
  className,
}: {
  labelProps: LabelProps;
  inputProps: InputProps;
  errorProps: ErrorProps;
  className?: string;
}) {
  const fallbackId = useId();

  const id = inputProps.id ?? fallbackId;
  const errorId = errorProps.errors?.length ? `${id}-error` : undefined;

  return (
    <div className={cn("flex flex-col items-center gap-[6px]", className)}>
      <div className="relative w-full">
        <Input
          {...conform.input(inputProps.field, {
            type: "text",
          })}
          id={id}
          className={cn(
            inputProps.baseClass,
            errorProps.errors?.length ? inputProps.inputErrorsClass : "",
          )}
          placeholder={inputProps.placeholder}
          aria-invalid={errorId ? true : undefined}
          aria-describedby={errorId}
        />

        <Label
          htmlFor={id}
          className={cn(
            labelProps.baseClass,
            errorProps.errors?.length ? labelProps.errorsClass : "",
          )}
        >
          {labelProps.children}
        </Label>
      </div>

      {errorId ? (
        <ErrorList
          className={errorProps.errorClass}
          errors={errorProps.errors}
          id={errorId}
        />
      ) : null}
    </div>
  );
}
