using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OK.ReadingIsGood.Order.Persistence.Config;
using OK.ReadingIsGood.Order.Persistence.Constants;
using OK.ReadingIsGood.Order.Persistence.Contexts;
using OK.ReadingIsGood.Order.Persistence.HostedServices;

namespace OK.ReadingIsGood.Order.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrderPersistence(this IServiceCollection services, OrderPersistenceConfig config)
        {
            services.AddSingleton(config);

            services.AddDbContext<OrderDataContext>(options =>
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