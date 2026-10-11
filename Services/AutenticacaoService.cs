using GerenciadorDeTarefas.Repositories.Interfaces;
using GerenciadorDeTarefas.DTOs;
using GerenciadorDeTarefas.Services.Interfaces;
using GerenciadorDeTarefas.Models;
using Microsoft.AspNetCore.Identity;
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

    public async Task<LoginResponseDto?> AutenticarAsync(LoginDto login)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(login.Email);

        if (usuario is null)
        {
            return null;
        }

        var resultado = _passwordHasher.VerifyHashedPassword(
     usuario,
     usuario.Senha,
     login.Senha);

        if (resultado == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return new LoginResponseDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email
        };
    }
}