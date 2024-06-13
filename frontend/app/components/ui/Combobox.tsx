import * as React from "react";
import { Check, ChevronsUpDown } from "lucide-react";
import { cn } from "~/utils/misc";
import { Button } from "./Button";
import { Command, CommandEmpty, CommandInput, CommandItem } from "./Command";
import { Popover, PopoverContent, PopoverTrigger } from "./Popover";
import { CommandList } from "cmdk";
import { us_states } from "~/routes/settings.profile_.edit/us-states";

type PopoverTriggerProps = React.ComponentPropsWithoutRef<
  typeof PopoverTrigger
>;

interface IProps extends PopoverTriggerProps {
  disabled: boolean;
}

export function Combobox({ className, disabled }: IProps) {
  const [open, setOpen] = React.useState(false);
  const [value, setValue] = React.useState("");

  const stateObj = us_states.find((state) => state.label === value);

  return (
    <Popover modal open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          disabled={disabled}
          variant="outline"
          aria-disabled={disabled}
          role="combobox"
          aria-expanded={open}
          className={cn("flex w-full justify-between", className)}
        >
          {value
            ? us_states.find((state) => state.label === value)?.label
            : "State"}
          <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="max-h-[--radix-popover-content-available-height] w-[--radix-popover-trigger-width] p-0">
        <Command
          filter={(value, search) => {
            if (value.toLowerCase().includes(search.toLowerCase())) return 1;
            return 0;
          }}
          className="max-h-[--radix-popover-content-available-height]"
        >
          <CommandInput placeholder="Search state..." />
          <input type="hidden" name="stateValue" value={stateObj?.value} />
          <CommandList className=" overflow-y-auto">
            <CommandEmpty>No state found.</CommandEmpty>

            {us_states.map((state) => (
              <CommandItem
                key={state.value}
                value={state.label}
                onSelect={(currentValue) => {
                  setValue(currentValue === value ? "" : currentValue);
                  setOpen(false);
                }}
              >
                <Check
                  className={cn(
                    "mr-2 h-4 w-4",
                    value === state.value ? "opacity-100" : "opacity-0",
                  )}
                />
                {state.label}
              </CommandItem>
            ))}
          </CommandList>
        </Command>
      </PopoverContent>
    </Popover>
  );
}
