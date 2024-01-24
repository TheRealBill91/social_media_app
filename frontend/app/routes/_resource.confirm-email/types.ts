interface MissingUserID {
  ErrorMessage: string;
}

interface EmailConfirmationExpired {
  ErrorMessage: string;
  Email: string;
}

export type EmailConfirmationResponse =
  | MissingUserID
  | EmailConfirmationExpired;
