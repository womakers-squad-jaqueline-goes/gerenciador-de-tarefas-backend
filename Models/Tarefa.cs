using GerenciadorDeTarefas.Enums;
using System.ComponentModel.DataAnnotations;


namespace GerenciadorDeTarefas.Models
{
    public class Tarefa
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 100 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "A descrição deve ter entre 3 e 500 caracteres.")]
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataDeVencimento { get; set; }
        public StatusTarefa Status { get; set; } = StatusTarefa.Pendente;
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}


