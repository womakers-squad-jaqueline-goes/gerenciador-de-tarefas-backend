using GerenciadorDeTarefas.Enums;

namespace GerenciadorDeTarefas.DTO.Tarefa
{
    public class TarefaResponseDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataDeVencimento { get; set; }
        public StatusTarefa Status { get; set; }
        public Guid UsuarioId { get; set; }
    }
}
