public class EmailConfirmationResponse
{
    public string ErrorMessage { get; set; } = null!;

    public string Email { get; set; } = null!;

    /// <summary>
    ///  Optional property for informing the frontend whether
    ///  or not the user can request a new email confirmation link
    ///  if theirs has expired. It is based on the daily limit of three
    ///  email confirmation expired requests per day per user
    /// </summary>
    public bool? CanRequestNewEmailConfirmation { get; set; }
}
