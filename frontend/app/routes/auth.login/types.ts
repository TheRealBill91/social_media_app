/**
 * An object that contains the error string
 * when the user login fails
 */

export interface LoginErrorResponse {
  /**
   * The error message when the user login fails
   */
  ErrorMessage: string;
}

/**
 *  An object that contains the success string when
 *  the user successfully logs in
 */

export interface LoginSuccessResponse {
  /**
   *  The user id for the user who just logged in
   */
  UserId: string;
}
