using Financeiro.Domain.Entities;
using Financeiro.Domain.Repositories;
using MongoDB.Driver;

namespace Financeiro.Infrastructure.Repositories
{
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly IMongoCollection<Transacao> _collection;

        public TransacaoRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("FinanceiroDB");
            _collection = database.GetCollection<Transacao>("Transacoes");

            var indexKeysDefinition = Builders<Transacao>.IndexKeys.Ascending(t => t.IdempotencyKey);
            var indexOptions = new CreateIndexOptions { Unique = true };
            _collection.Indexes.CreateOne(new CreateIndexModel<Transacao>(indexKeysDefinition, indexOptions));
        }

        public async Task AdicionarAsync(Transacao transacao)
        {
            await _collection.InsertOneAsync(transacao);
        }

        public async Task<Transacao?> ObterPorChaveIdempotenciaAsync(string chave)
        {
            return await _collection.Find(t => t.IdempotencyKey == chave).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Transacao>> ListarTodasAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }
    }
}
