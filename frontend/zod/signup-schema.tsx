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
    password: z
      .string({ required_error: "Password is required" })
      .refine((val) => val.length >= 8 && val.length <= 20, {
        message: "Password must be between 8 and 20 characters.",
      }),
    confirmPassword: z.string({
      required_error: "Password confirmation is required",
    }),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "Passwords do not match",
    path: ["confirmPassword"],
  });
