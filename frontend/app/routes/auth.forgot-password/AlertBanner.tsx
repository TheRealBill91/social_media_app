import { Alert, AlertTitle } from "~/components/ui/Alert";
import { tw } from "~/utils/tw-identity-helper";

interface AlertBannerProps {
  alertTitleFontSize: string | null;
  requestPasswordResetSuccess: string;
}

export function AlertBanner({
  alertTitleFontSize,
  requestPasswordResetSuccess,
}: AlertBannerProps) {
  return (
    <Alert className="md-shadow max-w-[24rem] bg-white">
      <AlertTitle className={tw`pb-0 ${alertTitleFontSize}`}>
        {requestPasswordResetSuccess}
      </AlertTitle>
    </Alert>
  );
}
