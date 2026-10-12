using GerenciadorDeTarefas.Repositories.Interfaces;
using GerenciadorDeTarefas.Services.Interfaces;
using GerenciadorDeTarefas.Models;
using Microsoft.AspNetCore.Identity;
using GerenciadorDeTarefas.DTOs.Login;
using GerenciadorDeTarefas.Exceptions;

namespace GerenciadorDeTarefas.Services;

public class AutenticacaoService : IAutenticacaoService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public AutenticacaoService(IUsuarioRepository usuarioRepository,
        IPasswordHasher<Usuario> passwordHasher)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponseDto> AutenticarAsync(LoginDto login)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(login.Email);

        if (usuario == null)
            throw new UnauthorizedException("E-mail ou senha inválidos");

        var senhaValida = _passwordHasher.VerifyHashedPassword(
            usuario,
            usuario.Senha,
            login.Senha);

        if (senhaValida == PasswordVerificationResult.Failed)
            throw new UnauthorizedException("E-mail ou senha inválidos");

        return new LoginResponseDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email
        };
    }
}