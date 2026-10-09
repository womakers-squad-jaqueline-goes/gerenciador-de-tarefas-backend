using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeTarefas.Repositories;

public class TarefaRepository : ITarefaRepository
{
    private readonly AppDbContext _context;

    public TarefaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tarefa>> ObterPorUsuarioIdAsync(Guid usuarioId)
    {
        return await _context.Tarefas
            .Where(tarefa => tarefa.UsuarioId == usuarioId)
            .ToListAsync();
    }

    public async Task<Tarefa?> ObterPorIdAsync(Guid id)
    {
        return await _context.Tarefas
            .FirstOrDefaultAsync(tarefa => tarefa.Id == id);
    }

    public async Task<bool> ExisteAsync(Guid usuarioId, string titulo)
    {
        return await _context.Tarefas
            .AnyAsync(tarefa =>
                tarefa.UsuarioId == usuarioId &&
                tarefa.Titulo == titulo);
    }

    public async Task AdicionarAsync(Tarefa tarefa)
    {
        await _context.Tarefas.AddAsync(tarefa);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Tarefa tarefa)
    {
        _context.Tarefas.Update(tarefa);
        await _context.SaveChangesAsync();
    }

    // TarefaRepository.cs
    public async Task RemoverAsync(Tarefa tarefa)
    {
        _context.Tarefas.Remove(tarefa);
        await _context.SaveChangesAsync();
    }
}