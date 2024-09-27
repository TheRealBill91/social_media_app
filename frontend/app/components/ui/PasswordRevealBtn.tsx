import {
  default as EyeOpen,
  default as EyeNone,
} from "~/components/icons/icon.tsx";

interface PasswordReveal {
  togglePassword: () => void;
  showPassword: boolean;
}

export function PasswordRevealBtn({
  togglePassword,
  showPassword,
}: PasswordReveal) {
  return (
    <>
      {showPassword ? (
        <button
          onClick={togglePassword}
          type="button"
          title="hide password icon"
          className="absolute right-4 top-4"
        >
          <EyeNone name="eye-none" className="size-[22px] text-gray-800" />
        </button>
      ) : (
        <button
          onClick={togglePassword}
          type="button"
          title="reveal password icon"
          className="absolute right-4 top-4"
        >
          <EyeOpen name="eye-open" className="size-[22px] text-gray-800" />
        </button>
      )}
    </>
  );
}
