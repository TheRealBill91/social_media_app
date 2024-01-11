using System.Security.Claims;
using System.Threading.RateLimiting;
using Microsoft.Extensions.Primitives;

public static class RateLimiterConfigurator
{
    public static IServiceCollection AddCustomRateLimiting(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // START OF AUTH RELATED RATE LIMITING POLICIES

        // signin sliding window rate limit configuration
        var SignInSlidingWindowOptions = new SlidingWindowPolicy();
        configuration
            .GetSection("RateLimiting:Authentication:SignInSlidingWindow")
            .Bind(SignInSlidingWindowOptions);
        var SignInPolicyName = "signInSlidingWindow";
        services.AddRateLimiter(options =>
        {
            options.AddPolicy(
                policyName: SignInPolicyName,
                partitioner: httpContext =>
                {
                    string partitionKey =
                        httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? httpContext?.Connection.RemoteIpAddress?.ToString()
                        ?? string.Empty;

                    if (!StringValues.IsNullOrEmpty(partitionKey))
                    {
                        return RateLimitPartition.GetSlidingWindowLimiter(
                            partitionKey,
                            factory =>
                                new SlidingWindowRateLimiterOptions
                                {
                                    PermitLimit = SignInSlidingWindowOptions.PermitLimit,
                                    QueueLimit = SignInSlidingWindowOptions.QueueLimit,
                                    Window = TimeSpan.FromMinutes(
                                        SignInSlidingWindowOptions.WindowInMinutes
                                    ),
                                    SegmentsPerWindow = SignInSlidingWindowOptions.SegmentsPerWindow
                                }
                        );
                    }

                    return RateLimitPartition.GetSlidingWindowLimiter(
                        "Anon",
                        factory =>
                            new SlidingWindowRateLimiterOptions
                            {
                                PermitLimit = 15,
                                QueueLimit = SignInSlidingWindowOptions.QueueLimit,
                                Window = TimeSpan.FromMinutes(
                                    SignInSlidingWindowOptions.WindowInMinutes
                                ),
                                SegmentsPerWindow = 6
                            }
                    );
                }
            );
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        // signup sliding window rate limit configuration
        var SignUpSlidingWindowOptions = new SlidingWindowPolicy();
        configuration
            .GetSection("RateLimiting:Authentication:SignUpSlidingWindow")
            .Bind(SignUpSlidingWindowOptions);
        var SignUpPolicyName = "signUpSlidingWindow";

        services.AddRateLimiter(options =>
        {
            options.AddPolicy(
                policyName: SignUpPolicyName,
                partitioner: httpContext =>
                {
                    string partitionKey =
                        httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? httpContext?.Connection.RemoteIpAddress?.ToString()
                        ?? string.Empty;

                    if (!StringValues.IsNullOrEmpty(partitionKey))
                    {
                        return RateLimitPartition.GetSlidingWindowLimiter(
                            partitionKey,
                            factory =>
                                new SlidingWindowRateLimiterOptions
                                {
                                    PermitLimit = SignUpSlidingWindowOptions.PermitLimit,
                                    QueueLimit = SignUpSlidingWindowOptions.QueueLimit,
                                    Window = TimeSpan.FromMinutes(
                                        SignUpSlidingWindowOptions.WindowInMinutes
                                    ),
                                    SegmentsPerWindow = SignUpSlidingWindowOptions.SegmentsPerWindow
                                }
                        );
                    }

                    return RateLimitPartition.GetSlidingWindowLimiter(
                        "Anon",
                        factory =>
                            new SlidingWindowRateLimiterOptions
                            {
                                PermitLimit = 15,
                                QueueLimit = SignUpSlidingWindowOptions.QueueLimit,
                                Window = TimeSpan.FromMinutes(
                                    SignUpSlidingWindowOptions.WindowInMinutes
                                ),
                                SegmentsPerWindow = 6
                            }
                    );
                }
            );
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        // confirm email sliding window rate limit configuration
        var ConfirmEmailSlidingWindowOptions = new SlidingWindowPolicy();
        configuration
            .GetSection("RateLimiting:Authentication:ConfirmEmailSlidingWindow")
            .Bind(ConfirmEmailSlidingWindowOptions);
        var ConfirmEmailPolicyName = "confirmEmailSlidingWindow";

        services.AddRateLimiter(options =>
        {
            options.AddPolicy(
                policyName: ConfirmEmailPolicyName,
                partitioner: httpContext =>
                {
                    string partitionKey =
                        httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? httpContext?.Connection.RemoteIpAddress?.ToString()
                        ?? string.Empty;

                    if (!StringValues.IsNullOrEmpty(partitionKey))
                    {
                        return RateLimitPartition.GetSlidingWindowLimiter(
                            partitionKey,
                            factory =>
                                new SlidingWindowRateLimiterOptions
                                {
                                    PermitLimit = ConfirmEmailSlidingWindowOptions.PermitLimit,
                                    QueueLimit = ConfirmEmailSlidingWindowOptions.QueueLimit,
                                    Window = TimeSpan.FromMinutes(
                                        ConfirmEmailSlidingWindowOptions.WindowInMinutes
                                    ),
                                    SegmentsPerWindow =
                                        ConfirmEmailSlidingWindowOptions.SegmentsPerWindow
                                }
                        );
                    }

                    return RateLimitPartition.GetSlidingWindowLimiter(
                        "Anon",
                        factory =>
                            new SlidingWindowRateLimiterOptions
                            {
                                PermitLimit = 3,
                                QueueLimit = ConfirmEmailSlidingWindowOptions.QueueLimit,
                                Window = TimeSpan.FromMinutes(
                                    ConfirmEmailSlidingWindowOptions.WindowInMinutes
                                ),
                                SegmentsPerWindow = 6
                            }
                    );
                }
            );
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        // logout sliding window rate limit configuration
        var SignoutSlidingWindowOptions = new SlidingWindowPolicy();
        configuration
            .GetSection("RateLimiting:Authentication:SignoutOutSlidingWindow")
            .Bind(SignoutSlidingWindowOptions);
        var SignoutPolicyName = "signoutSlidingWindow";

        services.AddRateLimiter(options =>
        {
            options.AddPolicy(
                policyName: SignoutPolicyName,
                partitioner: httpContext =>
                {
                    string partitionKey =
                        httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? httpContext?.Connection.RemoteIpAddress?.ToString()
                        ?? string.Empty;

                    if (!StringValues.IsNullOrEmpty(partitionKey))
                    {
                        return RateLimitPartition.GetSlidingWindowLimiter(
                            partitionKey,
                            factory =>
                                new SlidingWindowRateLimiterOptions
                                {
                                    PermitLimit = SignoutSlidingWindowOptions.PermitLimit,
                                    QueueLimit = SignoutSlidingWindowOptions.QueueLimit,
                                    Window = TimeSpan.FromMinutes(
                                        SignoutSlidingWindowOptions.WindowInMinutes
                                    ),
                                    SegmentsPerWindow =
                                        SignoutSlidingWindowOptions.SegmentsPerWindow
                                }
                        );
                    }

                    return RateLimitPartition.GetSlidingWindowLimiter(
                        "Anon",
                        factory =>
                            new SlidingWindowRateLimiterOptions
                            {
                                PermitLimit = 5,
                                QueueLimit = SignoutSlidingWindowOptions.QueueLimit,
                                Window = TimeSpan.FromMinutes(
                                    SignoutSlidingWindowOptions.WindowInMinutes
                                ),
                                SegmentsPerWindow = 6
                            }
                    );
                }
            );
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        // resend confirmation email sliding window rate limit configuration
        var ResendConfirmationEmailSlidingWindowOptions = new SlidingWindowPolicy();
        configuration
            .GetSection("RateLimiting:Authentication:ResendConfirmationEmailSlidingWindow")
            .Bind(ResendConfirmationEmailSlidingWindowOptions);
        var ResendConfirmationEmailPolicyName = "resendConfirmationEmailSlidingWindow";

        services.AddRateLimiter(options =>
        {
            options.AddPolicy(
                policyName: ResendConfirmationEmailPolicyName,
                partitioner: httpContext =>
                {
                    string partitionKey =
                        httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? httpContext?.Connection.RemoteIpAddress?.ToString()
                        ?? string.Empty;

                    if (!StringValues.IsNullOrEmpty(partitionKey))
                    {
                        return RateLimitPartition.GetSlidingWindowLimiter(
                            partitionKey,
                            factory =>
                                new SlidingWindowRateLimiterOptions
                                {
                                    PermitLimit =
                                        ResendConfirmationEmailSlidingWindowOptions.PermitLimit,
                                    QueueLimit =
                                        ResendConfirmationEmailSlidingWindowOptions.QueueLimit,
                                    Window = TimeSpan.FromMinutes(
                                        ResendConfirmationEmailSlidingWindowOptions.WindowInMinutes
                                    ),
                                    SegmentsPerWindow =
                                        ResendConfirmationEmailSlidingWindowOptions.SegmentsPerWindow
                                }
                        );
                    }

                    return RateLimitPartition.GetSlidingWindowLimiter(
                        "Anon",
                        factory =>
                            new SlidingWindowRateLimiterOptions
                            {
                                PermitLimit = 3,
                                QueueLimit = ResendConfirmationEmailSlidingWindowOptions.QueueLimit,
                                Window = TimeSpan.FromMinutes(
                                    ResendConfirmationEmailSlidingWindowOptions.WindowInMinutes
                                ),
                                SegmentsPerWindow = 6
                            }
                    );
                }
            );
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        // password reset request sliding window rate limit configuration
        var PasswordResetRequestSlidingWindowOptions = new SlidingWindowPolicy();
        configuration
            .GetSection("RateLimiting:Authentication:PasswordResetRequestSlidingWindow")
            .Bind(PasswordResetRequestSlidingWindowOptions);

        var PasswordResetRequestPolicyName = "passwordResetRequestSlidingWindow";

        services.AddRateLimiter(options =>
        {
            options.AddPolicy(
                policyName: PasswordResetRequestPolicyName,
                partitioner: httpContext =>
                {
                    string partitionKey =
                        httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? httpContext?.Connection.RemoteIpAddress?.ToString()
                        ?? string.Empty;

                    if (!StringValues.IsNullOrEmpty(partitionKey))
                    {
                        return RateLimitPartition.GetSlidingWindowLimiter(
                            partitionKey,
                            factory =>
                                new SlidingWindowRateLimiterOptions
                                {
                                    PermitLimit =
                                        PasswordResetRequestSlidingWindowOptions.PermitLimit,
                                    QueueLimit =
                                        PasswordResetRequestSlidingWindowOptions.QueueLimit,
                                    Window = TimeSpan.FromMinutes(
                                        PasswordResetRequestSlidingWindowOptions.WindowInMinutes
                                    ),
                                    SegmentsPerWindow =
                                        PasswordResetRequestSlidingWindowOptions.SegmentsPerWindow
                                }
                        );
                    }

                    return RateLimitPartition.GetSlidingWindowLimiter(
                        "Anon",
                        factory =>
                            new SlidingWindowRateLimiterOptions
                            {
                                PermitLimit = 3,
                                QueueLimit = PasswordResetRequestSlidingWindowOptions.QueueLimit,
                                Window = TimeSpan.FromMinutes(
                                    PasswordResetRequestSlidingWindowOptions.WindowInMinutes
                                ),
                                SegmentsPerWindow = 6
                            }
                    );
                }
            );

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        // Rate limiting policies for getting a single resource (single comment or multiple posts)
        var GetResourceSlidingWindowOptions = new SlidingWindowPolicy();
        configuration
            .GetSection("RateLimiting:GetResourceSlidingWindow")
            .Bind(GetResourceSlidingWindowOptions);
        var GetSingleResourcePolicyName = "getResourceSlidingWindow";

        services.AddRateLimiter(options =>
        {
            options.AddPolicy(
                policyName: GetSingleResourcePolicyName,
                partitioner: httpContext =>
                {
                    string partitionKey =
                        httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? httpContext?.Connection.RemoteIpAddress?.ToString()
                        ?? string.Empty;

                    if (!StringValues.IsNullOrEmpty(partitionKey))
                    {
                        return RateLimitPartition.GetSlidingWindowLimiter(
                            partitionKey,
                            factory =>
                                new SlidingWindowRateLimiterOptions
                                {
                                    PermitLimit = GetResourceSlidingWindowOptions.PermitLimit,
                                    QueueLimit = GetResourceSlidingWindowOptions.QueueLimit,
                                    Window = TimeSpan.FromMinutes(
                                        GetResourceSlidingWindowOptions.WindowInMinutes
                                    ),
                                    SegmentsPerWindow =
                                        GetResourceSlidingWindowOptions.SegmentsPerWindow
                                }
                        );
                    }

                    return RateLimitPartition.GetSlidingWindowLimiter(
                        "Anon",
                        factory =>
                            new SlidingWindowRateLimiterOptions
                            {
                                PermitLimit = 30,
                                QueueLimit = GetResourceSlidingWindowOptions.QueueLimit,
                                Window = TimeSpan.FromMinutes(
                                    GetResourceSlidingWindowOptions.WindowInMinutes
                                ),
                                SegmentsPerWindow = 6
                            }
                    );
                }
            );
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        // Rate limiting policy for creating (POST) resources (creating a post or comment)
        var CreateResourceSlidingWindowOptions = new SlidingWindowPolicy();
        configuration
            .GetSection("RateLimiting:CreateResourceSlidingWindow")
            .Bind(CreateResourceSlidingWindowOptions);
        var CreateResourcePolicyName = "createResourceSlidingWindow";

        services.AddRateLimiter(options =>
        {
            options.AddPolicy(
                policyName: CreateResourcePolicyName,
                partitioner: httpContext =>
                {
                    string partitionKey =
                        httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? httpContext?.Connection.RemoteIpAddress?.ToString()
                        ?? string.Empty;

                    if (!StringValues.IsNullOrEmpty(partitionKey))
                    {
                        return RateLimitPartition.GetSlidingWindowLimiter(
                            partitionKey,
                            factory =>
                                new SlidingWindowRateLimiterOptions
                                {
                                    PermitLimit = CreateResourceSlidingWindowOptions.PermitLimit,
                                    QueueLimit = CreateResourceSlidingWindowOptions.QueueLimit,
                                    Window = TimeSpan.FromMinutes(
                                        CreateResourceSlidingWindowOptions.WindowInMinutes
                                    ),
                                    SegmentsPerWindow =
                                        CreateResourceSlidingWindowOptions.SegmentsPerWindow
                                }
                        );
                    }

                    return RateLimitPartition.GetSlidingWindowLimiter(
                        "Anon",
                        factory =>
                            new SlidingWindowRateLimiterOptions
                            {
                                PermitLimit = 6,
                                QueueLimit = CreateResourceSlidingWindowOptions.QueueLimit,
                                Window = TimeSpan.FromMinutes(
                                    CreateResourceSlidingWindowOptions.WindowInMinutes
                                ),
                                SegmentsPerWindow = 6
                            }
                    );
                }
            );
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        // Rate limiting policy for updating (PATCH) resources (updating a comment or post)
        var UpdateResourceSlidingWindowOptions = new SlidingWindowPolicy();
        configuration
            .GetSection("RateLimiting:UpdateResourceSlidingWindow")
            .Bind(UpdateResourceSlidingWindowOptions);
        var UpdateResourcePolicyName = "updateResourceSlidingWindow";

        services.AddRateLimiter(options =>
        {
            options.AddPolicy(
                policyName: UpdateResourcePolicyName,
                partitioner: httpContext =>
                {
                    string partitionKey =
                        httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? httpContext?.Connection.RemoteIpAddress?.ToString()
                        ?? string.Empty;

                    if (!StringValues.IsNullOrEmpty(partitionKey))
                    {
                        return RateLimitPartition.GetSlidingWindowLimiter(
                            partitionKey,
                            factory =>
                                new SlidingWindowRateLimiterOptions
                                {
                                    PermitLimit = UpdateResourceSlidingWindowOptions.PermitLimit,
                                    QueueLimit = UpdateResourceSlidingWindowOptions.QueueLimit,
                                    Window = TimeSpan.FromMinutes(
                                        UpdateResourceSlidingWindowOptions.WindowInMinutes
                                    ),
                                    SegmentsPerWindow =
                                        UpdateResourceSlidingWindowOptions.SegmentsPerWindow
                                }
                        );
                    }

                    return RateLimitPartition.GetSlidingWindowLimiter(
                        "Anon",
                        factory =>
                            new SlidingWindowRateLimiterOptions
                            {
                                PermitLimit = 15,
                                QueueLimit = UpdateResourceSlidingWindowOptions.QueueLimit,
                                Window = TimeSpan.FromMinutes(
                                    UpdateResourceSlidingWindowOptions.WindowInMinutes
                                ),
                                SegmentsPerWindow = 6
                            }
                    );
                }
            );
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        // Rate limiting policy for updating (PATCH) resources (updating a comment or post)
        var DeleteResourceSlidingWindowOptions = new SlidingWindowPolicy();
        configuration
            .GetSection("RateLimiting:DeleteResourceSlidingWindow")
            .Bind(DeleteResourceSlidingWindowOptions);
        var DeleteResourcePolicyName = "deleteResourceSlidingWindow";

        services.AddRateLimiter(options =>
        {
            options.AddPolicy(
                policyName: DeleteResourcePolicyName,
                partitioner: httpContext =>
                {
                    string partitionKey =
                        httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? httpContext?.Connection.RemoteIpAddress?.ToString()
                        ?? string.Empty;

                    if (!StringValues.IsNullOrEmpty(partitionKey))
                    {
                        return RateLimitPartition.GetSlidingWindowLimiter(
                            partitionKey,
                            factory =>
                                new SlidingWindowRateLimiterOptions
                                {
                                    PermitLimit = DeleteResourceSlidingWindowOptions.PermitLimit,
                                    QueueLimit = DeleteResourceSlidingWindowOptions.QueueLimit,
                                    Window = TimeSpan.FromMinutes(
                                        DeleteResourceSlidingWindowOptions.WindowInMinutes
                                    ),
                                    SegmentsPerWindow =
                                        DeleteResourceSlidingWindowOptions.SegmentsPerWindow
                                }
                        );
                    }

                    return RateLimitPartition.GetSlidingWindowLimiter(
                        "Anon",
                        factory =>
                            new SlidingWindowRateLimiterOptions
                            {
                                PermitLimit = 7,
                                QueueLimit = DeleteResourceSlidingWindowOptions.QueueLimit,
                                Window = TimeSpan.FromMinutes(
                                    DeleteResourceSlidingWindowOptions.WindowInMinutes
                                ),
                                SegmentsPerWindow = 6
                            }
                    );
                }
            );
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        // Rate limiting policy for upvoting a comment and post
        var ResourceUpvoteTokenBucketOptions = new TokenBucketPolicy();
        configuration
            .GetSection("RateLimiting:TokenBucketLimiters:ResourceUpvote")
            .Bind(ResourceUpvoteTokenBucketOptions);
        var ResourceUpvotePolictName = "resourceUpvoteTokenBucket";

        services.AddRateLimiter(options =>
        {
            options.AddPolicy(
                policyName: ResourceUpvotePolictName,
                partitioner: httpContext =>
                {
                    string partitionKey =
                        httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? httpContext?.Connection.RemoteIpAddress?.ToString()
                        ?? string.Empty;

                    if (!StringValues.IsNullOrEmpty(partitionKey))
                    {
                        return RateLimitPartition.GetTokenBucketLimiter(
                            partitionKey,
                            factory =>
                                new TokenBucketRateLimiterOptions
                                {
                                    TokenLimit = ResourceUpvoteTokenBucketOptions.TokenLimit,
                                    TokensPerPeriod =
                                        ResourceUpvoteTokenBucketOptions.TokensPerPeriod,
                                    ReplenishmentPeriod = TimeSpan.FromSeconds(
                                        ResourceUpvoteTokenBucketOptions.ReplenishmentPeriod
                                    ),
                                    QueueLimit = ResourceUpvoteTokenBucketOptions.QueueLimit
                                }
                        );
                    }

                    return RateLimitPartition.GetTokenBucketLimiter(
                        "Anon",
                        factory =>
                            new TokenBucketRateLimiterOptions
                            {
                                TokenLimit = 15,
                                TokensPerPeriod = ResourceUpvoteTokenBucketOptions.TokensPerPeriod,
                                ReplenishmentPeriod = TimeSpan.FromSeconds(
                                    ResourceUpvoteTokenBucketOptions.ReplenishmentPeriod
                                ),
                                QueueLimit = ResourceUpvoteTokenBucketOptions.QueueLimit
                            }
                    );
                }
            );
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        return services;
    }
}
