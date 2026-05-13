using Financeiro.Domain.Entities;
using FluentAssertions;
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
        transacao.Valor.Should().Be(valor);
        transacao.IdempotencyKey.Should().Be(idempotencyKey);
        transacao.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Nao_Deve_Permitir_Valor_Negativo()
    {
        // Arrange
        var valor = -50.00m;
        var tipo = TipoTransacao.Debito;
        var idempotencyKey = "key-002";

        // Act
        Action act = () => new Transacao(valor, tipo, idempotencyKey);

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage("O valor da transação deve ser positivo.");
    }
}
