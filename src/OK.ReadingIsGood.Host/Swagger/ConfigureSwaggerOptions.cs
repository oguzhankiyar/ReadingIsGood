using System;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OK.ReadingIsGood.Host.Config;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OK.ReadingIsGood.Host.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly HostConfig _config;

        public ConfigureSwaggerOptions(
            IApiVersionDescriptionProvider provider,
            HostConfig config)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = $"{_config.App.Title} v{description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                    Description = _config.App.Description + (description.IsDeprecated ? " (deprecated)" : string.Empty)
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Description = "Please insert JWT token into field"
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
            }
        }
    }
}