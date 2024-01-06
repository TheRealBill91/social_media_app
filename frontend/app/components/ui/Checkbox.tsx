import * as Checkbox from "@radix-ui/react-checkbox";
import { default as Check } from "../../components/icons/icon.tsx";

export function RadixCheckbox() {
  return (
    <div className=" my-3 flex items-center px-1 md:my-2 md:px-2">
      <Checkbox.Root
        className="flex h-[22px] w-[22px] appearance-none items-center justify-center rounded-[4px] border border-gray-400 bg-white shadow-[0_2px_10px] shadow-blackA4 outline-none hover:bg-violet3  [&:focus-visible]:shadow-[0_0_0_2px_black] "
        id="c1"
      >
        <Checkbox.Indicator className=" text-gray-700">
          <Check icon="check" className="h-[18px] w-[18px]" />
        </Checkbox.Indicator>
      </Checkbox.Root>
      <label
        className="pl-[10px] text-sm leading-none text-black md:text-base "
        htmlFor="c1"
      >
        Remember me?
      </label>
    </div>
  );
}
