using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace API.Starter.Infrastructure.SecurityHeaders;

internal static class Startup
{
    internal static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app, IConfiguration config)
    {
        var settings = config.GetSection(nameof(SecurityHeaderSettings)).Get<SecurityHeaderSettings>();

        if (settings?.Enable is true)
        {
            app.Use(async (context, next) =>
            {
                if (!string.IsNullOrWhiteSpace(settings.XFrameOptions))
                {
                    context.Response.Headers[HeaderNames.XFRAMEOPTIONS] = settings.XFrameOptions;
                }

                if (!string.IsNullOrWhiteSpace(settings.XContentTypeOptions))
                {
                    context.Response.Headers[HeaderNames.XCONTENTTYPEOPTIONS] = settings.XContentTypeOptions;
                }

                if (!string.IsNullOrWhiteSpace(settings.ReferrerPolicy))
                {
                    context.Response.Headers[HeaderNames.REFERRERPOLICY] = settings.ReferrerPolicy;
                }

                if (!string.IsNullOrWhiteSpace(settings.PermissionsPolicy))
                {
                    context.Response.Headers[HeaderNames.PERMISSIONSPOLICY] = settings.PermissionsPolicy;
                }

                if (!string.IsNullOrWhiteSpace(settings.SameSite))
                {
                    context.Response.Headers[HeaderNames.SAMESITE] = settings.SameSite;
                }

                if (!string.IsNullOrWhiteSpace(settings.XXSSProtection))
                {
                    context.Response.Headers[HeaderNames.XXSSPROTECTION] = settings.XXSSProtection;
                }

                await next();
            });
        }

        return app;
    }
}