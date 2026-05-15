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
            var transacaoExistente = await _repository.ObterPorChaveIdempotenciaAsync(idempotencyKey);
            if (transacaoExistente != null)
            {
                return; 
            }

            var transacao = new Transacao(valor, tipo, idempotencyKey);

            await _repository.AdicionarAsync(transacao);

            await _publishEndpoint.Publish(transacao);
        }

        public async Task<IEnumerable<Transacao>> ListarTransacoes()
        {
            return await _repository.ListarTodasAsync();
        }
    }
}
