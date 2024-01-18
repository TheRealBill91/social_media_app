import { z } from "zod";

export const signUpSchema = z
  .object({
    firstName: z
      .string({ required_error: "First name is required" })
      .max(50, { message: "Maximum length of 50 characters exceeded." })
      .trim(),
    lastName: z
      .string({ required_error: "Last name is required" })
      .max(50, { message: "Maximum length of 50 characters exceeded." })
      .trim(),
    username: z
      .string({ required_error: "Username is required" })
      .max(50, { message: "Maximum length of 50 characters exceeded." })
      .trim(),
    email: z
      .string({ required_error: "Email is required" })
      .email(
        "Please enter a valid email address in the format: example@domain.com.",
      ),
    password: z.string({ required_error: "Password is required" }),
    passwordConfirmation: z.string({
      required_error: "Password confirmation is required",
    }),
  })

  .superRefine((data, ctx) => {
    const passwordValidation = passwordValidationLogic(data.password);
    if (!passwordValidation.valid) {
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message: passwordValidation.message,
        path: ["password"],
      });
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message: passwordValidation.message,
        path: ["passwordConfirmation"],
      });
    }

    if (data.password !== data.passwordConfirmation) {
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message: "Passwords do not match",
        path: ["password"],
      });
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message: "Passwords do not match",
        path: ["passwordConfirmation"],
      });
    }
  });

function passwordValidationLogic(password: string) {
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
