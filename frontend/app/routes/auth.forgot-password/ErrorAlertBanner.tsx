import { default as AlertCircle } from "~/components/icons/icon.tsx";
import { Alert, AlertTitle } from "~/components/ui/Alert";

interface ErrorAlertBannerProps {
  requestPasswordResetError: string;
}

export function ErrorAlertBanner({
  requestPasswordResetError,
}: ErrorAlertBannerProps) {
  return (
    <Alert className="md-shadow max-w-[24rem] bg-white">
      <AlertCircle name="alert-circle" className="size-[22px] fill-red-400" />
      <AlertTitle className="pb-0 leading-normal">
        {requestPasswordResetError}
      </AlertTitle>
    </Alert>
  );
}
