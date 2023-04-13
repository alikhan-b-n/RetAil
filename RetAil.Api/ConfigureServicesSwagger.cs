using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace RetAil;

public class ConfigureServicesSwagger
{
    private const string AppTitle = "Akvelon Test Task API";
    private static readonly string AppVersion = $"q.0.0";
    private const string SwaggerConfig = "/swagger/v1/swagger.json";
    private const string SwaggerUrl = "api/manual";

    /// <summary>
    /// ConfigureServices Swagger services
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = AppTitle,
                Version = AppVersion,
                Description = "Web API for Akvelon Test Task"
            });

            options.ResolveConflictingActions(x => x.First());

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
    }

    /// <summary>
    /// Set up some properties for swagger UI for client
    /// </summary>
    /// <param name="settings"></param>
    public static void SwaggerSettings(SwaggerUIOptions settings)
    {
        settings.SwaggerEndpoint(SwaggerConfig, $"{AppTitle} v.{AppVersion}");
        settings.RoutePrefix = SwaggerUrl;
        settings.HeadContent = $"";
        settings.DocumentTitle = $"{AppTitle}";
        settings.DefaultModelExpandDepth(0);
        settings.DefaultModelRendering(ModelRendering.Model);
        settings.DefaultModelsExpandDepth(0);
        settings.DocExpansion(DocExpansion.None);
        settings.OAuthClientId("microservice1");
        settings.OAuthScopeSeparator(" ");
        settings.OAuthClientSecret("secret");
        settings.DisplayRequestDuration();
        settings.OAuthAppName("Microservice module API");
    }
}