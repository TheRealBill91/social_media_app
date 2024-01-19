export function passwordValidationLogic(password: string) {
  const hasLowercase = password.toUpperCase() !== password;
  const hasUppercase = password.toLowerCase() !== password;
  const hasDigit = /\d/.test(password);
  const hasNonAlphanumeric = /\W/.test(password);
  const isLengthValid = password.length >= 8 && password.length <= 20;

  return {
    valid:
      hasLowercase &&
      hasUppercase &&
      hasDigit &&
      hasNonAlphanumeric &&
      isLengthValid,
    message:
      "Password must be 8-20 characters with mixed case, a digit, and a symbol.",
  };
}
