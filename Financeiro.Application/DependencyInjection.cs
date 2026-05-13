using Financeiro.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Financeiro.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<TransacaoAppService>();
        
        return services;
    }
}
