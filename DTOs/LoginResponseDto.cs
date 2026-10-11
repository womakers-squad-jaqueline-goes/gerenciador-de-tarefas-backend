namespace GerenciadorDeTarefas.DTOs;

public class LoginResponseDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}