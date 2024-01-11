interface ResendEmailBtn {
  submitting: boolean;
}

export function ResendConfirmationEmailBtn({ submitting }: ResendEmailBtn) {
  return (
    <button
      aria-disabled={submitting}
      type="submit"
      className="text-primary-foreground inline-flex h-10 w-full cursor-pointer items-center justify-center rounded-md bg-gray-800 px-4 py-2 text-center text-white outline-0 transition-all hover:bg-gray-800/90  focus:ring focus:ring-gray-500 focus:ring-offset-2 aria-disabled:cursor-not-allowed aria-disabled:bg-gray-300 aria-disabled:text-gray-600 aria-disabled:focus:ring-gray-400"
    >
      {" "}
      Resend Confirmation Email
    </button>
  );
}
