import { useEffect } from "react";
import { toast as showToast } from "sonner";
import { ToastMessage } from "../flash-session/schema";

export function useToast(toast?: ToastMessage | null): void {
  useEffect(() => {
    if (toast) {
      showToast[toast.type](toast.text, {
        id: toast.type,
        duration: toast.duration || 5000,
      });
    }
  }, [toast]);
}
