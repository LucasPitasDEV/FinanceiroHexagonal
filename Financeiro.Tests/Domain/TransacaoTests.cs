using Financeiro.Domain.Entities;
using Xunit;

namespace Financeiro.Tests.Domain;

public class TransacaoTests
{
    [Fact]
    public void Deve_Criar_Transacao_Com_Dados_Validos()
    {
        // Arrange
        var valor = 100.00m;
        var tipo = TipoTransacao.Credito;
        var idempotencyKey = "key-001";

        // Act
        var transacao = new Transacao(valor, tipo, idempotencyKey);

        // Assert
        Assert.Equal(valor, transacao.Valor);
        Assert.Equal(idempotencyKey, transacao.IdempotencyKey);
        Assert.NotEqual(Guid.Empty, transacao.Id);
    }

    [Fact]
    public void Nao_Deve_Permitir_Valor_Negativo()
    {
        // Arrange
        var valor = -50.00m;
        var tipo = TipoTransacao.Debito;
        var idempotencyKey = "key-002";

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Transacao(valor, tipo, idempotencyKey));
        Assert.Equal("O valor da transação deve ser positivo.", ex.Message);
    }
}
