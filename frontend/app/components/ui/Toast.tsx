import { Toaster } from "sonner";

interface ToastOptions {
  className?: string;
  closeButton?: boolean;
  descriptionClassName?: string;
  style?: React.CSSProperties;
  cancelButtonStyle?: React.CSSProperties;
  actionButtonStyle?: React.CSSProperties;
  duration?: number;
  unstyled?: boolean;
  classNames?: ToastClassnames;
}

interface ToastClassnames {
  toast?: string;
  title?: string;
  description?: string;
  loader?: string;
  closeButton?: string;
  cancelButton?: string;
  actionButton?: string;
  success?: string;
  error?: string;
  info?: string;
  warning?: string;
  loading?: string;
  default?: string;
  content?: string;
  icon?: string;
}

export interface ToastProps {
  /**
   * Whether we want a close button on the toast or not
   */
  closeButton: boolean | undefined;
  position?:
    | "top-left"
    | "top-right"
    | "bottom-left"
    | "bottom-right"
    | "top-center"
    | "bottom-center";
  toastOptions: ToastOptions;
}

interface IProps {
  toastProps: ToastProps;
}

export function Toast({ toastProps }: IProps) {
  const { closeButton, position, toastOptions } = toastProps;
  return (
    <Toaster
      closeButton={closeButton}
      position={position}
      toastOptions={toastOptions}
    />
  );
}
