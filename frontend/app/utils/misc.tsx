import { useFormAction, useNavigation } from "@remix-run/react";
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

/**
 * Returns true if the current navigation is submitting the current route's
 * form. Defaults to the current route's form action and method POST.
 *
 * Defaults state to 'non-idle'
 *
 * NOTE: the default formAction will include query params, but the
 * navigation.formAction will not, so don't use the default formAction if you
 * want to know if a form is submitting without specific query params.
 */
export function useIsPending({
  formAction,
  formMethod = "POST",
  state = "non-idle",
}: {
  formAction?: string;
  formMethod?: "POST" | "GET" | "PUT" | "PATCH" | "DELETE";
  state?: "submitting" | "loading" | "non-idle";
} = {}) {
  const contextualFormAction = useFormAction();
  const navigation = useNavigation();
  const isPendingState =
    state === "non-idle"
      ? navigation.state !== "idle"
      : navigation.state === state;
  return (
    isPendingState &&
    navigation.formAction === (formAction ?? contextualFormAction) &&
    navigation.formMethod === formMethod
  );
}

/**
 * Combine multiple header objects into one (uses append so headers are not overridden)
 */
export function combineHeaders(
  ...headers: Array<ResponseInit["headers"] | null | undefined>
) {
  const combined = new Headers();
  for (const header of headers) {
    if (!header) continue;
    for (const [key, value] of new Headers(header).entries()) {
      combined.append(key, value);
    }
  }
  return combined;
}
