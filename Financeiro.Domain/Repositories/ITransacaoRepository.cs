using Financeiro.Domain.Entities;

namespace Financeiro.Domain.Repositories;

public interface ITransacaoRepository
{
    Task AdicionarAsync(Transacao transacao);
    Task<Transacao?> ObterPorChaveIdempotenciaAsync(string chave);
    Task<IEnumerable<Transacao>> ListarTodasAsync();
}