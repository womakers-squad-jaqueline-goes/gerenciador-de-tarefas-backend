using GerenciadorDeTarefas.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GerenciadorDeTarefas.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GerenciadorDeTarefas.Controllers;

[ApiController]
[Route("api/autenticacao")]
public class AutenticacaoController : ControllerBase
{
    private readonly IAutenticacaoService _autenticacaoService;

    public AutenticacaoController(IAutenticacaoService autenticacaoService)
    {
        _autenticacaoService = autenticacaoService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto login)
    {
        var usuario = await _autenticacaoService.AutenticarAsync(login);

        if (usuario is null)
        {
            return Unauthorized(new
            {
                mensagem = "E-mail ou senha inválidos."
            });
        }

        var claims = new[]
    {
    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
    new Claim(ClaimTypes.Name, usuario.Nome),
    new Claim(ClaimTypes.Email, usuario.Email)
};

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return Ok(usuario);
    }
}