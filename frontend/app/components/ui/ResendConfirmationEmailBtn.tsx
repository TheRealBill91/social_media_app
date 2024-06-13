interface ResendEmailBtn {
  submitting: boolean;
  emailMissing: boolean;
}

export function ResendConfirmationEmailBtn({
  submitting,
  emailMissing,
}: ResendEmailBtn) {
  return (
    <button
      aria-disabled={submitting}
      disabled={emailMissing}
      type="submit"
      className="inline-flex h-10 w-full cursor-pointer items-center justify-center rounded-md bg-gray-800 px-4 py-2 text-center text-primary-foreground text-white outline-0 transition-all  hover:bg-gray-800/90  focus-visible:ring focus-visible:ring-gray-500 focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:bg-gray-300 disabled:text-gray-600 disabled:focus:ring-gray-400 aria-disabled:cursor-not-allowed aria-disabled:bg-gray-300 aria-disabled:text-gray-600 aria-disabled:focus:ring-gray-400"
    >
      {" "}
      Resend Confirmation Email
    </button>
  );
}
