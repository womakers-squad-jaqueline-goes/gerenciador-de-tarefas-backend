using GerenciadorDeTarefas.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GerenciadorDeTarefas.DTOs.Login;
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
    [EndpointSummary("Autenticar usuário")]
    [EndpointDescription("Autentica um usuário utilizando e-mail e senha. Em caso de sucesso, cria uma sessão autenticada por cookie.")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto login)
    {
        var usuario = await _autenticacaoService.AutenticarAsync(login);

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

    [Authorize]
    [HttpPost("logout")]
    [EndpointSummary("Encerrar sessão")]
    [EndpointDescription("Encerra a sessão autenticada do usuário removendo o cookie de autenticação.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return NoContent();
    }

}