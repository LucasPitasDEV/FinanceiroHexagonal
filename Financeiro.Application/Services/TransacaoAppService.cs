using Financeiro.Domain.Entities;
using Financeiro.Domain.Repositories;
using MassTransit;

namespace Financeiro.Application.Services
{
    public class TransacaoAppService
    {
        private readonly ITransacaoRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;

        public TransacaoAppService(ITransacaoRepository repository, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task RegistrarTransacao(decimal valor, TipoTransacao tipo, string idempotencyKey)
        {
            // 1. Verificar idempotência
            var transacaoExistente = await _repository.ObterPorChaveIdempotenciaAsync(idempotencyKey);
            if (transacaoExistente != null)
            {
                // Já processado, podemos retornar com sucesso ou lançar um log
                return; 
            }

            // 2. Executar Lógica de Negócio (Criar Entidade)
            // A própria entidade valida se o valor é positivo
            var transacao = new Transacao(valor, tipo, idempotencyKey);

            // 3. Persistir através do adaptador
            await _repository.AdicionarAsync(transacao);

            // 4. Notificar outros sistemas (RabbitMQ)
            await _publishEndpoint.Publish(transacao);
        }
    }
}
