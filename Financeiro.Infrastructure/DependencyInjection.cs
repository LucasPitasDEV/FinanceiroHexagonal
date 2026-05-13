using Financeiro.Domain.Repositories;
using Financeiro.Infrastructure.Repositories;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Financeiro.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        var mongoUri = configuration.GetConnectionString("MongoDb");
        services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoUri));

        services.AddScoped<ITransacaoRepository, TransacaoRepository>();

        services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(DependencyInjection).Assembly);

            x.AddMongoDbOutbox(o =>
            {
                o.QueryDelay = TimeSpan.FromSeconds(1);
                o.ClientFactory(provider => provider.GetRequiredService<IMongoClient>());
                o.DatabaseFactory(provider => provider.GetRequiredService<IMongoClient>().GetDatabase("FinanceiroDB"));
                
                o.UseBusOutbox();
            });

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitUri = configuration.GetConnectionString("RabbitMq");
                cfg.Host(new Uri(rabbitUri ?? "amqp://localhost:5672"));

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
