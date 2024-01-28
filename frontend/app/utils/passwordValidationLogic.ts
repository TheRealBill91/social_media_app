export function passwordValidationLogic(password: string) {
  const hasLowercase = password.toUpperCase() !== password;
  const hasUppercase = password.toLowerCase() !== password;
  const hasDigit = /\d/.test(password);
  const hasNonAlphanumeric = /\W/.test(password);

  return {
    valid: hasLowercase && hasUppercase && hasDigit && hasNonAlphanumeric,
    message:
      "Password must have one lower case, one uppercase, a digit, and a symbol.",
  };
}
