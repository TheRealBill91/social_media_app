import * as Checkbox from "@radix-ui/react-checkbox";
import { default as Check } from "../../components/icons/icon.tsx";

export function RadixCheckbox() {
  return (
    <>
      <div className="my-3 flex items-center px-3">
        <Checkbox.Root
          className="flex h-[20px] w-[20px] appearance-none items-center justify-center rounded-[4px] bg-white shadow-[0_2px_10px] shadow-blackA4 outline-none hover:bg-violet3 focus:shadow-[0_0_0_2px_black]"
          id="c1"
        >
          <Checkbox.Indicator className="text-gray-600">
            <Check icon="check" className="h-[15px] w-[15px]" />
          </Checkbox.Indicator>
        </Checkbox.Root>
        <label
          className="pl-[20px] text-lg leading-none text-gray-700"
          htmlFor="c1"
        >
          Remember me?
        </label>
      </div>
    </>
  );
}
