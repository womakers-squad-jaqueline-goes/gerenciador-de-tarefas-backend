using GerenciadorDeTarefas.Models;

namespace GerenciadorDeTarefas.Services.Interfaces
{
    public interface ITarefaService
    {
        Task<IEnumerable<Tarefa>> ObterPorUsuarioAsync(Guid usuarioId);
        Task<Tarefa> ObterPorIdAsync(Guid usuarioId, Guid id);
        Task<Tarefa> CriarAsync(Guid usuarioId, Tarefa tarefa);
        Task<Tarefa> AtualizarAsync(Guid usuarioId, Guid id, Tarefa tarefa);
        Task<bool> ConcluirAsync(Guid usuarioId, Guid Id);
        Task<bool> RemoverAsync(Guid usuarioId, Guid Id);
    }
}
