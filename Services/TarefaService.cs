using GerenciadorDeTarefas.Enums;
using GerenciadorDeTarefas.Exceptions;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Repositories.Interfaces;
using GerenciadorDeTarefas.Services.Interfaces;

namespace GerenciadorDeTarefas.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public TarefaService(ITarefaRepository tarefaRepository, IUsuarioRepository usuarioRepository)
        {
            _tarefaRepository = tarefaRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Tarefa> ObterPorIdAsync(Guid usuarioId, Guid id)
        {
            var tarefa = await _tarefaRepository.ObterPorIdAsync(id);

            if (tarefa == null || tarefa.UsuarioId != usuarioId)
                throw new NotFoundException("Tarefa não encontrada para este usuário.");

            return tarefa;
        }

        public async Task<IEnumerable<Tarefa>> ObterPorUsuarioAsync(Guid usuarioId)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);

            if (usuario == null)
                throw new NotFoundException("Usuário não encontrado.");
            
            return await _tarefaRepository.ObterPorUsuarioIdAsync(usuarioId);
        }

        public async Task<Tarefa> AtualizarAsync(Guid usuarioId, Guid id, Tarefa tarefa)
        {
            var tarefaExistente = await ObterPorIdAsync(usuarioId, id);
            
            if (tarefaExistente.Status == StatusTarefa.Concluida)
                throw new ConflictException("Não é possível atualizar uma tarefa concluída.");

            if (!string.IsNullOrWhiteSpace(tarefa.Titulo))
            {
                await ValidarTituloDuplicadoAsync(usuarioId, tarefa.Titulo.Trim(), id);
                tarefaExistente.Titulo = tarefa.Titulo.Trim();
            }

            if (!string.IsNullOrWhiteSpace(tarefa.Descricao))
                tarefaExistente.Descricao = tarefa.Descricao.Trim();

            if (tarefa.DataDeVencimento != default(DateTime))
            {
                ValidarDataDeVencimento(tarefa.DataDeVencimento);
                tarefaExistente.DataDeVencimento = tarefa.DataDeVencimento;
            }

            await _tarefaRepository.AtualizarAsync(tarefaExistente);

            return tarefaExistente;
        }

        public async Task<bool> ConcluirAsync(Guid usuarioId, Guid Id)
        {
            var tarefaExistente = await ObterPorIdAsync(usuarioId, Id);

            if (tarefaExistente.Status == StatusTarefa.Concluida)
                throw new ConflictException("A tarefa já está concluída.");

            tarefaExistente.Status = StatusTarefa.Concluida;
            await _tarefaRepository.AtualizarAsync(tarefaExistente);

            return true;
        }

        public async Task<Tarefa> CriarAsync(Guid usuarioId, Tarefa tarefa)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);

            if (usuario == null)
                throw new NotFoundException("Usuário não encontrado.");

            ValidarDataDeVencimento(tarefa.DataDeVencimento);
            
            await ValidarTituloDuplicadoAsync(usuarioId, tarefa.Titulo.Trim());

            tarefa.UsuarioId = usuarioId;
            tarefa.Usuario = usuario;
            tarefa.Descricao = tarefa.Descricao.Trim();
            tarefa.Titulo = tarefa.Titulo.Trim();

            await _tarefaRepository.AdicionarAsync(tarefa);
            return tarefa;
        }

        public async Task<bool> RemoverAsync(Guid usuarioId, Guid Id)
        {
            var tarefaExistente = await ObterPorIdAsync(usuarioId, Id);
            await _tarefaRepository.RemoverAsync(tarefaExistente);
            return true;
        }

        private void ValidarDataDeVencimento(DateTime dataDeVencimento)
        {
            if (dataDeVencimento < DateTime.Today)
                throw new BadRequestException("A data de vencimento não pode ser anterior à data atual.");
        }

        private async Task ValidarTituloDuplicadoAsync(Guid usuarioId, string titulo, Guid? tarefaId = null)
        {
            var tarefasUsuario = await _tarefaRepository.ObterPorUsuarioIdAsync(usuarioId);

            bool tarefaDuplicada = tarefasUsuario.Any(t => 
                t.Id != tarefaId && 
                string.Equals(
                    t.Titulo.Trim(), 
                    titulo.Trim(), 
                    StringComparison.OrdinalIgnoreCase));

            if (tarefaDuplicada)
                throw new ConflictException("Já existe uma tarefa com este título para este usuário.");
        }
    }
}
