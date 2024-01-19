import {
  default as EyeOpen,
  default as EyeNone,
} from "~/components/icons/icon.tsx";

interface PasswordOnlyReveal {
  togglePassword: () => void;
  showPassword: boolean;
}

interface PasswordConfirmationReveal {
  togglePasswordConfirmation: () => void;
  showPasswordConfirmation: boolean;
}

type PasswordReveal = PasswordOnlyReveal | PasswordConfirmationReveal;

export function PasswordRevealBtn(props: PasswordReveal) {
  if ("togglePassword" in props) {
    return (
      <>
        {props.showPassword ? (
          <button
            onClick={() => props.togglePassword()}
            type="button"
            title="hide password icon"
            className="absolute right-4 top-4"
          >
            <EyeNone icon="eye-none" className="size-[22px] text-gray-800" />
          </button>
        ) : (
          <button
            onClick={() => props.togglePassword()}
            type="button"
            title="reveal password icon"
            className="absolute right-4 top-4"
          >
            <EyeOpen icon="eye-open" className="size-[22px] text-gray-800" />
          </button>
        )}
      </>
    );
  } else if ("togglePasswordConfirmation" in props) {
    return (
      <>
        {props.showPasswordConfirmation ? (
          <button
            onClick={() => props.togglePasswordConfirmation()}
            type="button"
            title="password reveal icon"
            className="absolute right-4 top-4"
          >
            <EyeNone icon="eye-none" className="size-[22px] text-gray-800" />
          </button>
        ) : (
          <button
            onClick={() => props.togglePasswordConfirmation()}
            type="button"
            title="password reveal icon"
            className="absolute right-4 top-4"
          >
            <EyeOpen icon="eye-open" className="size-[22px] text-gray-800" />
          </button>
        )}
      </>
    );
  }
}
