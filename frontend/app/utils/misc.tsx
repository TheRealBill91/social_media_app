import { clsx, type ClassValue } from "clsx";

import { twMerge } from "tailwind-merge";

/**
 * Combines class names using`clsx`and merges Tailwind CSS classes using`twMerge`.
 *
 * This function takes multiple class name inputs and processes them with`clsx`to
 * handle conditional and dynamic class names. It then merges the resultant class
 * string with`twMerge`to ensure Tailwind CSS classes do not conflict and are
 * efficiently combined.
 *
 * @param inputs - A variadic number of class name inputs, which
 * can be strings, objects, arrays, or a combination thereof. These inputs will be
 * processed by`clsx`and then merged by`twMerge`.
 * @returns The combined and merged class name string.
 *
 */
export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}
