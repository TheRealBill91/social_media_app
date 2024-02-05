import { z } from "zod";
import { passwordValidationLogic } from "~/utils/passwordValidationLogic";

const usernameSchema = z
  .string({ required_error: "Username is required" })
  .min(3, { message: "Username is too short " })
  .max(20, { message: "Username is too long" })
  .regex(/^[a-zA-Z0-9_]+$/, {
    message: "Username can only include letters, numbers, and underscores",
  })
  .trim();

const passwordSchema = z
  .string({ required_error: "Password is required" })
  .min(8, { message: "Password is too short" })
  .max(50, { message: "Password is too long" })
  .trim();

export const loginSchema = z
  .object({
    username: usernameSchema,
    password: passwordSchema,
    rememberMe: z.boolean().optional(),
  })

  .superRefine((loginSchema, ctx) => {
    const passwordValidation = passwordValidationLogic(loginSchema.password);
    if (!passwordValidation.valid) {
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message: passwordValidation.message,
        path: ["password"],
      });
    }
  });
