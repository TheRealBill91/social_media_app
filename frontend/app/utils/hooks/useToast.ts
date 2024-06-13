import { useEffect } from "react";
import { toast as showToast } from "sonner";

type ToastTypes =
  | "normal"
  | "action"
  | "success"
  | "info"
  | "warning"
  | "error"
  | "loading"
  | "default";

interface toastProps {
  type: ToastTypes;
  text: string;
  duration?: number | undefined;
}

export function useToast(toast?: toastProps | undefined): void {
  useEffect(() => {
    if (toast?.type === "error") {
      showToast.error(toast.text, {
        id: "errorId",
        duration: toast.duration || 5000,
      });
    } else if (toast?.type === "success") {
      showToast.success(toast.text, {
        id: "successId",
        duration: toast.duration || 5000,
      });
    }
  }, [toast]);
}
