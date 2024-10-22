import { z } from "zod";

const toastMessageSchema = z.object({
  // message to diplay to the user
  text: z.string(),

  // type of notification
  type: z
    .enum(["message", "success", "info", "warning", "error", "loading"])
    .default("message"),

  // duration for the toast (in milliseconds)
  duration: z.number().optional(),
});

// Infers the type for later usage and type safety
export type ToastMessage = z.infer<typeof toastMessageSchema>;

export const flashSessionValuesSchema = z.object({
  // validation schema from above
  toast: toastMessageSchema.optional(),
});

export type FlashSessionValues = z.infer<typeof flashSessionValuesSchema>;
