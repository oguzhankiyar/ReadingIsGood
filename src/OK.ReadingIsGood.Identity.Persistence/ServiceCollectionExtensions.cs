using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OK.ReadingIsGood.Identity.Persistence.Config;
using OK.ReadingIsGood.Identity.Persistence.Constants;
using OK.ReadingIsGood.Identity.Persistence.Contexts;
using OK.ReadingIsGood.Identity.Persistence.HostedServices;

namespace OK.ReadingIsGood.Identity.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityPersistence(this IServiceCollection services, IdentityPersistenceConfig config)
        {
            services.AddSingleton(config);

            services.AddDbContext<IdentityDataContext>(options =>
            {
                options.UseNpgsql(config.ConnectionString,
                        npgsqlOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsHistoryTable(TableConstants.MigrationTableName, TableConstants.SchemaName);
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: 10,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorCodesToAdd: null);
                        });
            });

            services.AddHostedService<MigrationHostedService>();

            return services;
        }
    }
}