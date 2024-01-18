interface MissingUserID {
  error: string;
}

interface EmailConfirmationExpired {
  error: string;
  email: string;
}

export type EmailConfirmationResponse =
  | MissingUserID
  | EmailConfirmationExpired;
