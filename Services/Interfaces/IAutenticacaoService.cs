using GerenciadorDeTarefas.DTOs;

namespace GerenciadorDeTarefas.Services.Interfaces;

public interface IAutenticacaoService
{
    Task<LoginResponseDto?> AutenticarAsync(LoginDto login);
}