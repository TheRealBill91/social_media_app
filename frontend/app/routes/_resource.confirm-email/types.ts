interface MissingUserID {
  ErrorMessage: string;
}

interface EmailConfirmationExpired {
  ErrorMessage: string;
  Email: string;
  /**
   * dotnet backend checks whether the user has hit their daily limit
   * of three email confirmation requests (resending the email confirmation
   * link if the first one has expired).
   *
   * We use this determine whether or not we should set the email on the cookie,
   * which then informs the frontend UI whether the user can request a new
   * email confirmation link.
   */
  CanRequestNewEmailConfirmation: boolean;
}

export type EmailConfirmationResponse =
  | MissingUserID
  | EmailConfirmationExpired;
