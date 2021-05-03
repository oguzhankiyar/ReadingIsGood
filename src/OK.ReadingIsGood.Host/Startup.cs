using System;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OK.ReadingIsGood.Host.Config;
using OK.ReadingIsGood.Host.Filters;
using OK.ReadingIsGood.Host.Swagger;
using OK.ReadingIsGood.Identity.API;
using OK.ReadingIsGood.Identity.Business;
using OK.ReadingIsGood.Identity.Persistence;
using OK.ReadingIsGood.Order.API;
using OK.ReadingIsGood.Order.Business;
using OK.ReadingIsGood.Order.Persistence;
using OK.ReadingIsGood.Product.API;
using OK.ReadingIsGood.Product.Business;
using OK.ReadingIsGood.Product.Persistence;
using OK.ReadingIsGood.Shared.MessageBus;

namespace OK.ReadingIsGood.Host
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var config = new HostConfig();
            _configuration.Bind(config);
            services.AddSingleton(config);

            if (config.Cors == null || config.Cors.Origins == null || config.Cors.Origins.Length == 0)
            {
                services.AddCors(o => o
                    .AddPolicy("DefaultPolicy", b => b
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()));
            }
            else
            {
                services.AddCors(o => o
                    .AddPolicy("DefaultPolicy", b => b
                        .WithOrigins(config.Cors.Origins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()));
            }

            services.AddVersionedApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
            });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config.Modules.Identity.Business.Issuer,
                        ValidAudience = config.Modules.Identity.Business.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Modules.Identity.Business.SecurityKey))
                    };
                });

            services.AddHttpContextAccessor();
            services.AddScoped<IPrincipal>(sp => sp.GetService<IHttpContextAccessor>().HttpContext?.User);

            services.ConfigureOptions<ConfigureSwaggerOptions>();
            services.AddSwaggerGen();
            services.AddTransient<GlobalExceptionFilter>();

            services
                .AddControllers(o =>
                {
                    o.Filters.Add<GlobalExceptionFilter>();
                })
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    o.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                });

            services.AddIdentityPersistence(config.Modules.Identity.Persistence);
            services.AddIdentityBusiness(config.Modules.Identity.Business);
            services.AddIdentityAPI(config.Modules.Identity.API);

            services.AddProductPersistence(config.Modules.Product.Persistence);
            services.AddProductBusiness(config.Modules.Product.Business);
            services.AddProductAPI(config.Modules.Product.API);

            services.AddOrderPersistence(config.Modules.Order.Persistence);
            services.AddOrderBusiness(config.Modules.Order.Business);
            services.AddOrderAPI(config.Modules.Order.API);

            services.AddMessageBus();
            services.AddInMemoryMessageBus();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("DefaultPolicy");

            var config = app.ApplicationServices.GetService<HostConfig>();
            var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();

            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.DocumentTitle = config.App.Title;

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    opt.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"{config.App.Name} {description.GroupName}");
                }
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(e =>
            {
                e.MapControllers();

                e.MapGet("/", async context =>
                {
                    var assemblyName = Assembly.GetEntryAssembly().GetName();
                    var applicationInfo = new
                    {
                        Name = assemblyName.Name,
                        Version = assemblyName.Version.ToString(),
                        Environment = env.EnvironmentName,
                        Documentation = $"{context.Request.Scheme}://{context.Request.Host}/swagger"
                    };

                    await context.Response.WriteAsJsonAsync(applicationInfo);
                });
            });
        }
    }
}