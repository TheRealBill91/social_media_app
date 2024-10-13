import { z } from "zod";
import { passwordValidationLogic } from "~/utils/passwordValidationLogic";

const firstNameSchema = z
  .string({ required_error: "First name is required" })
  .max(50, { message: "Maximum length of 50 characters exceeded." })
  .trim();

const lastNameSchema = z
  .string({ required_error: "Last name is required" })
  .max(50, { message: "Maximum length of 50 characters exceeded." })
  .trim();

const usernameSchema = z
  .string({ required_error: "Username is required" })
  .min(3, { message: "Username is too short " })
  .max(20, { message: "Username is too long" })
  .regex(/^[a-zA-Z0-9_]+$/, {
    message: "Username can only include letters, numbers, and underscores",
  })
  .trim();

export const emailSchema = z
  .string({ required_error: "Email is required" })
  .email({
    message:
      "Please enter a valid email address in the format: example@domain.com.",
  })
  .min(3, { message: "Email is too short " })
  .max(50, { message: "Email is too long" });

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

export const signUpSchema = z
  .object({
    firstName: firstNameSchema,
    lastName: lastNameSchema,
    username: usernameSchema,
    email: emailSchema,
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
