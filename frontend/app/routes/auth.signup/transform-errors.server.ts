export function transformErrors(
  serverErrors: Record<string, string[]>,
): Record<string, string[]> {
  const PasswordErrorMessage =
    "Password must be 8-20 characters with mixed case, a digit, and a symbol.";

  const transformedErrors: Record<string, string[]> = {};
  for (const value in serverErrors) {
    switch (value) {
      case "Password":
      case "PasswordRequiresDigit":
      case "PasswordRequiresUpper":
      case "PasswordRequiresLower":
        if (!transformedErrors.password?.includes(PasswordErrorMessage)) {
          transformedErrors.password = [PasswordErrorMessage];
          transformedErrors.passwordConfirmation = [PasswordErrorMessage];
        }

        break;
      case "DuplicateEmail":
        if (!transformedErrors.email) {
          transformedErrors.email = [];
        }

        transformedErrors.email = [...serverErrors[value]];
        break;
      case "DuplicateUserName":
        if (!transformedErrors.username) {
          transformedErrors.username = [];
        }

        transformedErrors.username = [...serverErrors[value]];
        break;
    }
  }

  return transformedErrors;
}
