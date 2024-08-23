using Microsoft.AspNetCore.HttpLogging;

namespace SocialMediaApp.Extensions;

public static class HttpLoggingExtensions
{
    public static void AddHttpLogging(
        this IServiceCollection services,
        IWebHostEnvironment env
    )
    {
        if (env.IsDevelopment())
        {
            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields =
                    HttpLoggingFields.RequestMethod
                    | HttpLoggingFields.RequestPath
                    | HttpLoggingFields.RequestProtocol
                    | HttpLoggingFields.RequestHeaders
                    | HttpLoggingFields.ResponseStatusCode
                    | HttpLoggingFields.ResponseHeaders
                    | HttpLoggingFields.ResponseBody;
                logging.RequestHeaders.Add("sec-ch-ua");
                logging.ResponseHeaders.Add("MyResponseHeader");
            });
        }
        else if (env.IsProduction())
        {
            services.AddHttpLogging(logging =>
                logging.LoggingFields =
                    HttpLoggingFields.RequestPath
                    | HttpLoggingFields.ResponseStatusCode
            );
        }
    }
}
