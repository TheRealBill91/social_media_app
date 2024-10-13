import z from "zod";

export function transformSignupErrors(
  serverErrors: Record<"DuplicateEmail" | "DuplicateUserName", string[]>, // this type will only be for last two switch case statements
  ctx: z.RefinementCtx,
): void {
  const PasswordErrorMessage =
    "Password must be 8-20 characters with mixed case, a digit, and a symbol.";

  // const transformedErrors: Record<string, string[]> = {};
  for (const value in serverErrors) {
    switch (value) {
      case "Password":
      case "PasswordRequiresDigit":
      case "PasswordRequiresUpper":
      case "PasswordRequiresLower":
        ctx.addIssue({
          code: z.ZodIssueCode.custom,
          message: PasswordErrorMessage,
          path: ["password"],
        });

        break;
      case "DuplicateEmail":
        ctx.addIssue({
          code: z.ZodIssueCode.custom,
          message: serverErrors[value][0],
          path: ["email"],
        });

        break;
      case "DuplicateUserName":
        ctx.addIssue({
          code: z.ZodIssueCode.custom,
          message: serverErrors[value][0],
          path: ["username"],
        });
        break;
    }
  }
}
