import { z } from "zod";
import { passwordValidationLogic } from "~/utils/passwordValidationLogic";

const passwordSchema = z
  .string({ required_error: "Password is required" })
  .min(8, { message: "Password is too short" })
  .max(50, { message: "Password is too long" })
  .trim();

const passwordConfirmationSchema = z
  .string({ required_error: "Password confirmation is required" })
  .min(8, { message: "Password confirmation is too short" })
  .max(50, { message: "Password confirmation is too long" })
  .trim();

export const PasswordAndConfirmPasswordSchema = z
  .object({
    password: passwordSchema,
    passwordConfirmation: passwordConfirmationSchema,
  })
  .superRefine((data, ctx) => {
    const passwordValidation = passwordValidationLogic(data.password);
    const passwordConfirmationValidation = passwordValidationLogic(
      data.passwordConfirmation,
    );
    if (!passwordValidation.valid) {
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message: passwordValidation.message,
        path: ["password"],
      });
    }
    if (!passwordConfirmationValidation) {
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
        path: ["passwordConfirmation"],
      });
    }
  });
