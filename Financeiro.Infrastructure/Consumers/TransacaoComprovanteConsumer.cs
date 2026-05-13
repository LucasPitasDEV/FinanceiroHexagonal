using Financeiro.Domain.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Financeiro.Infrastructure.Consumers;

public class TransacaoComprovanteConsumer : IConsumer<Transacao>
{
    private readonly ILogger<TransacaoComprovanteConsumer> _logger;

    public TransacaoComprovanteConsumer(ILogger<TransacaoComprovanteConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<Transacao> context)
    {
        var transacao = context.Message;

        _logger.LogInformation("[CONSUMER] Gerando comprovante para transação {Id}. Chave: {Key}, Tipo: {Tipo}, Valor: {Valor}", 
            transacao.Id, transacao.IdempotencyKey, transacao.Tipo, transacao.Valor);

        return Task.CompletedTask;
    }
}