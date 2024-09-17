import { useIsPending } from "~/utils/misc";
import { StatusButton } from "./StatusButton";

interface ResendEmailBtn {
  emailMissing: boolean;
  state: "submitting" | "loading" | "idle";
  data?: { status: "error" } | { status: null } | undefined;
}

export function ResendConfirmationEmailBtn({
  emailMissing,
  state,
  data,
}: ResendEmailBtn) {
  const isPending = useIsPending();

  return (
    <div className="flex w-full flex-1 items-center justify-between gap-6">
      <StatusButton
        aria-disabled={isPending}
        className="w-full flex-1 cursor-pointer gap-4 bg-gray-800 px-3 py-[10px] text-sm capitalize hover:bg-gray-900 disabled:cursor-not-allowed"
        status={state === "submitting" ? "pending" : data?.status ?? "idle"}
        type="submit"
        disabled={isPending || emailMissing}
      >
        Resend Confirmation Email
      </StatusButton>
    </div>
  );
}
