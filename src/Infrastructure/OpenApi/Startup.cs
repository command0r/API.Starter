using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NJsonSchema.Generation.TypeMappers;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;

namespace API.Starter.Infrastructure.OpenApi;

internal static class Startup
{
    internal static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services, IConfiguration config)
    {
        var settings = config.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();
        if (settings.Enable)
        {
            services.AddVersionedApiExplorer(o => o.SubstituteApiVersionInUrl = true);
            services.AddEndpointsApiExplorer();
            services.AddOpenApiDocument((document, serviceProvider) =>
            {
                document.PostProcess = doc =>
                {
                    doc.Info.Title = "API Starter";
                    doc.Info.Version = "1.0";
                    doc.Info.Description = string.Empty;
                    doc.Info.Contact = null;
                    doc.Info.License = null;
                };

                if (config["SecuritySettings:Provider"].Equals("AzureAd", StringComparison.OrdinalIgnoreCase))
                {
                    document.AddSecurity(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.OAuth2,
                        Flow = OpenApiOAuth2Flow.AccessCode,
                        Description = "OAuth2.0 Auth Code with PKCE",
                        Flows = new()
                        {
                            AuthorizationCode = new()
                            {
                                AuthorizationUrl = config["SecuritySettings:Swagger:AuthorizationUrl"],
                                TokenUrl = config["SecuritySettings:Swagger:TokenUrl"],
                                Scopes = new Dictionary<string, string>
                                {
                                    { config["SecuritySettings:Swagger:ApiScope"], "access the api" }
                                }
                            }
                        }
                    });
                }
                else
                {
                    document.AddSecurity(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Description = "Input your Bearer token to access this API",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Type = OpenApiSecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        BearerFormat = "JWT",
                    });
                }

                document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor());
                document.OperationProcessors.Add(new SwaggerGlobalAuthProcessor());


                document.OperationProcessors.Add(new SwaggerHeaderAttributeProcessor());

            });
        }

        return services;
    }

    internal static IApplicationBuilder UseOpenApiDocumentation(this IApplicationBuilder app, IConfiguration config)
    {
        if (config.GetValue<bool>("SwaggerSettings:Enable"))
        {
            app.UseOpenApi();
            app.UseSwaggerUi(options =>
            {
                options.DefaultModelsExpandDepth = -1;
                options.DocExpansion = "none";
                options.TagsSorter = "alpha";

                // Custom CSS to replace Swagger logo with commandor.ico
                options.CustomStylesheetPath = "/Files/swagger-custom.css";

                // Custom JavaScript for additional features
                options.CustomJavaScriptPath = "/Files/swagger-custom.js";

                if (config["SecuritySettings:Provider"].Equals("AzureAd", StringComparison.OrdinalIgnoreCase))
                {
                    options.OAuth2Client = new OAuth2ClientSettings
                    {
                        AppName = "Full Stack Hero Api Client",
                        ClientId = config["SecuritySettings:Swagger:OpenIdClientId"],
                        UsePkceWithAuthorizationCodeGrant = true,
                        ScopeSeparator = " "
                    };
                    options.OAuth2Client.Scopes.Add(config["SecuritySettings:Swagger:ApiScope"]);
                }
            });
        }

        return app;
    }
}