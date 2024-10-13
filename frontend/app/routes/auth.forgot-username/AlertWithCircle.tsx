import { AlertCircle } from "lucide-react";
import { Alert, AlertTitle } from "~/components/ui/Alert";
import { cn } from "~/utils/misc";

export function AlertWithCircle({
  requestUsernameError,
  className,
}: {
  requestUsernameError: string;
  className?: string;
}) {
  return (
    <>
      <Alert className="md-shadow max-w-[24rem] bg-white">
        <AlertCircle name="alert-circle" className="size-[22px] fill-red-400" />
        <AlertTitle className={cn("pb-0", className)}>
          {requestUsernameError}
        </AlertTitle>
      </Alert>
    </>
  );
}
