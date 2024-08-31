import { cn } from "~/utils/misc.tsx";
import { useSpinDelay } from "spin-delay";
import Icon from "../icons/icon.tsx";
import { Button, ButtonProps } from "./Button.tsx";
import React from "react";
import { Tooltip, TooltipProvider, TooltipTrigger } from "./Tooltip.tsx";

export const StatusButton = React.forwardRef<
  HTMLButtonElement,
  ButtonProps & {
    status: "pending" | "success" | "error" | "idle";
    message?: string | null;
    spinDelay?: Parameters<typeof useSpinDelay>;
  }
>(({ message, status, className, children, spinDelay, ...props }, ref) => {
  const delayedPending = useSpinDelay(status === "pending", {
    delay: 400,
    minDuration: 300,
    ...spinDelay,
  });

  const companion = {
    pending: delayedPending ? (
      <div
        role="status"
        className="inline-flex size-6 items-center justify-center"
      >
        <Icon
          name="update"
          className="size-[40px] animate-spin"
          title="loading"
        />
      </div>
    ) : null,
    success: (
      <div
        role="status"
        className="inline-flex size-6 items-center justify-center"
      >
        <Icon name="check" title="success" />
      </div>
    ),
    error: (
      <div
        role="status"
        className="inline-flex size-6 items-center justify-center rounded-full bg-destructive"
      >
        <Icon
          name="cross-1"
          className="text-destructive-foreground"
          title="error"
        />
      </div>
    ),
    idle: null,
  }[status];

  return (
    <Button
      ref={ref}
      className={cn(
        "transition-al self-center rounded-l border-[1px] border-solid bg-gray-700 p-2 text-xl text-slate-50 shadow-sm shadow-gray-100 outline-none hover:bg-primary/90 focus-visible:ring focus-visible:ring-gray-700/80 focus-visible:ring-offset-1 disabled:opacity-50",
        className,
      )}
      {...props}
    >
      <div>{children}</div>
      {message ? (
        <TooltipProvider>
          <Tooltip>
            <TooltipTrigger></TooltipTrigger>
            <TooltipTrigger>{message}</TooltipTrigger>
          </Tooltip>
        </TooltipProvider>
      ) : (
        companion
      )}
    </Button>
  );
});

StatusButton.displayName = "Button";
