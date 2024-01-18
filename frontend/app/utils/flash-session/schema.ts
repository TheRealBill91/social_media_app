import { z } from "zod";

type ToastTypes =
  | "normal"
  | "action"
  | "success"
  | "info"
  | "warning"
  | "error"
  | "loading"
  | "default";

const toastMessageSchema = z.object({
  // message to diplay to the user
  text: z.string(),

  // type of notification
  type: z.custom<ToastTypes>(),
});

// Infers the type for later usage and type safety
export type ToastMessage = z.infer<typeof toastMessageSchema>;

export const flashSessionValuesSchema = z.object({
  // validation schema from above
  toast: toastMessageSchema.optional(),
});

export type FlashSessionValues = z.infer<typeof flashSessionValuesSchema>;
