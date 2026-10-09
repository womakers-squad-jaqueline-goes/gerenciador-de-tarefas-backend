using GerenciadorDeTarefas.Models;

namespace GerenciadorDeTarefas.Repositories.Interfaces;

public interface ITarefaRepository
{
    Task<IEnumerable<Tarefa>> ObterPorUsuarioIdAsync(Guid usuarioId);
    Task<Tarefa?> ObterPorIdAsync(Guid id);
    Task<bool> ExisteAsync(Guid usuarioId, string titulo);
    Task AdicionarAsync(Tarefa tarefa);
    Task AtualizarAsync(Tarefa tarefa);
    // ITarefaRepository.cs
    Task RemoverAsync(Tarefa tarefa);
}