import { useState } from "react";

/**
 * Hook that toggles the visibility of the password in the input
 */

export function usePasswordReveal() {
  const [showPassword, setShowPassword] = useState(false);

  const togglePassword = () => setShowPassword(!showPassword);

  return {
    togglePassword,
    showPassword,
  };
}
