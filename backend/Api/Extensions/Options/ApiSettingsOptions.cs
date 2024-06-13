// namespace SocialMediaApp.Configuration;

/// <summary>
/// <c>ApiSettingsOptions</c>class that can be bound to
/// the<c>ApiSettings</c>values
/// <para></para>
/// <see href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-8.0#the-options-pattern"> Options pattern documentation</see>
/// </summary>
public class ApiSettingsOptions
{
    /// <summary>
    /// The <c>ApiSettings</c>field is used so the string<c>"ApiSettings"</c>
    /// doesn't need to be hard coded in the app when binding the class to a
    /// configuration provider
    /// <para></para>
    /// <see href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-8.0#the-options-pattern:~:text=The%20Position%20field%20is%20used%20so%20the%20string%20%22Position%22%20doesn%27t%20need%20to%20be%20hard%20coded%20in%20the%20app%20when%20binding%20the%20class%20to%20a%20configuration%20provider."> Relevant documentation</see>
    ///</summary>
    public const string ApiSettings = "ApiSettings";

    public string BaseUrl { get; set; } = String.Empty;
    public string FrontendUrl { get; set; } = String.Empty;
};
