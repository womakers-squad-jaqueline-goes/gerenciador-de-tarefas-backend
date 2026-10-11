using System.ComponentModel.DataAnnotations;

namespace GerenciadorDeTarefas.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "Informe seu e-mail.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Informe sua senha.")]
    public string Senha { get; set; } = string.Empty;
}