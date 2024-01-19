import { z } from "zod";
import { passwordValidationLogic } from "~/utils/passwordValidationLogic";

export const loginSchema = z
  .object({
    userIdentifier: z.string().refine(
      (value) => {
        // Check if value is an email or username
        if (isEmail(value)) {
          return z.string().email().safeParse(value).success;
        } else {
          return z.string().max(50).safeParse(value).success;
        }
      },
      {
        message: "Please enter a valid email or username.",
      },
    ),
    password: z.string({ required_error: "Password is required" }),
    rememberMe: z.boolean().optional(),
  })

  .superRefine((data, ctx) => {
    const passwordValidation = passwordValidationLogic(data.password);
    if (!passwordValidation.valid) {
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message: passwordValidation.message,
        path: ["password"],
      });
    }
  });

function isEmail(input: string): boolean {
  const emailRegex = /^[^@]+@[^@]+\.[^@]+$/;

  return emailRegex.test(input);
}
