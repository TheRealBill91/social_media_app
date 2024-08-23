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
    public EmailConfirmationTokenProviderOptions()
    {
        Name = "EmailDataProtectorTokenProvider";
        TokenLifespan = TimeSpan.FromMinutes(2);
    }
}
