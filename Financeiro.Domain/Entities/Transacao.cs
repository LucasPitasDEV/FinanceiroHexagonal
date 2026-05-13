using System;

namespace Financeiro.Domain.Entities;

public enum TipoTransacao { Credito, Debito }

public class Transacao
{
    public Guid Id { get; private set; }
    public string IdempotencyKey { get; private set; }
    public decimal Valor { get; private set; }
    public TipoTransacao Tipo { get; private set; }
    public DateTime Data { get; private set; }

    public Transacao(decimal valor, TipoTransacao tipo, string idempotencyKey)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor da transação deve ser positivo.");

        if (string.IsNullOrWhiteSpace(idempotencyKey))
            throw new ArgumentException("A chave de idempotência é obrigatória.");

        Id = Guid.NewGuid();
        IdempotencyKey = idempotencyKey;
        Valor = valor;
        Tipo = tipo;
        Data = DateTime.UtcNow;
    }
}