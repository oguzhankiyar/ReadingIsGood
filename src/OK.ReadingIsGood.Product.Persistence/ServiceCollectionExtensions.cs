using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OK.ReadingIsGood.Product.Persistence.Config;
using OK.ReadingIsGood.Product.Persistence.Constants;
using OK.ReadingIsGood.Product.Persistence.Contexts;
using OK.ReadingIsGood.Product.Persistence.HostedServices;

namespace OK.ReadingIsGood.Product.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProductPersistence(this IServiceCollection services, ProductPersistenceConfig config)
        {
            services.AddSingleton(config);

            services.AddDbContext<ProductDataContext>(options =>
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