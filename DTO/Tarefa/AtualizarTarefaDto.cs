using System.ComponentModel.DataAnnotations;

namespace GerenciadorDeTarefas.DTO.Tarefa
{
    public class AtualizarTarefaDto
    {
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 100 caracteres.")]
        public string? Titulo { get; set; }

        [StringLength(500, MinimumLength = 3, ErrorMessage = "A descrição deve ter entre 3 e 500 caracteres.")]
        public string? Descricao { get; set; }
        public DateTime? DataDeVencimento { get; set; }
    }
}
