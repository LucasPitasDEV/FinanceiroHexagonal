using Financeiro.Application.Services;
using Financeiro.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Financeiro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransacaoController : ControllerBase
{
    private readonly TransacaoAppService _appService;

    public TransacaoController(TransacaoAppService appService)
    {
        _appService = appService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TransacaoRequest request)
    {
        await _appService.RegistrarTransacao(request.Valor, request.Tipo, request.IdempotencyKey);
        return Ok(new { mensagem = "Transação processada com sucesso." });
    }
}

public record TransacaoRequest(decimal Valor, TipoTransacao Tipo, string IdempotencyKey);