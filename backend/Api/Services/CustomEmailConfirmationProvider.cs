using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

/// <summary>
/// Custom token provider for email confirmation tokens.
/// </summary>
/// <typeparam name="TUser">The type of user object.</typeparam>
public class CustomEmailConfirmationTokenProvider<TUser>
    : DataProtectorTokenProvider<TUser>
    where TUser : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomEmailConfirmationTokenProvider{TUser}"/> class.
    /// </summary>
    /// <param name="dataProtectionProvider">The data protection provider used for creating data protectors.</param>
    /// <param name="options">The options used for configuring the token provider.</param>
    /// <param name="logger">The logger used for logging.</param>
    public CustomEmailConfirmationTokenProvider(
        IDataProtectionProvider dataProtectionProvider,
        IOptions<EmailConfirmationTokenProviderOptions> options,
        ILogger<DataProtectorTokenProvider<TUser>> logger
    )
        : base(dataProtectionProvider, options, logger) { }
}

/// <summary>
/// Options class for configuring the email confirmation token provider.
/// </summary>
public class EmailConfirmationTokenProviderOptions
    : DataProtectionTokenProviderOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailConfirmationTokenProviderOptions"/> class.
    /// Sets default values for the token provider options.
    /// </summary>
    public EmailConfirmationTokenProviderOptions()
    {
        // Set the name of the token provider
        Name = "EmailDataProtectorTokenProvider";

        // Set the lifespan of the token to 2 minutes
        TokenLifespan = TimeSpan.FromMinutes(2);
    }
}
