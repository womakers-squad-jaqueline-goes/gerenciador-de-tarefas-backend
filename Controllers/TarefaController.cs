using GerenciadorDeTarefas.DTOs.Tarefa;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GerenciadorDeTarefas.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/usuarios/{usuarioId:guid}/tarefas")]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaService _tarefaService;

        public TarefaController(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        [HttpGet]
        [EndpointSummary("Listar tarefas de um usuário")]
        [EndpointDescription("Retorna todas as tarefas do usuário informado. Retorna uma lista vazia caso o usuário não possua tarefas.")]
        [ProducesResponseType(typeof(IEnumerable<TarefaResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TarefaResponseDto>>> ObterPorUsuarioAsync(Guid usuarioId)
        {
            if (!UsuarioAutenticado(usuarioId))
                return Forbid();

            var tarefas = await _tarefaService.ObterPorUsuarioAsync(usuarioId);
            var response = tarefas.Select(MapearParaResponseDto);
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [EndpointSummary("Obter tarefa por ID")]
        [EndpointDescription("Retorna os dados de uma tarefa específica, desde que ela pertença ao usuário informado.")]
        [ProducesResponseType(typeof(TarefaResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TarefaResponseDto>> ObterPorIdAsync(Guid usuarioId, Guid id)
        {
            if (!UsuarioAutenticado(usuarioId))
                return Forbid();

            var tarefa = await _tarefaService.ObterPorIdAsync(usuarioId, id);
            var response = MapearParaResponseDto(tarefa);
            return Ok(response);
        }

        [HttpPost]
        [EndpointSummary("Criar tarefa")]
        [EndpointDescription("Cadastra uma tarefa para o usuário informado. A data de vencimento não pode ser anterior à data atual, e o título não pode estar duplicado entre as tarefas do mesmo usuário.")]
        [ProducesResponseType(typeof(TarefaResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TarefaResponseDto>> CriarAsync(Guid usuarioId, [FromBody] CriarTarefaDto criarTarefaDto)
        {
            if (!UsuarioAutenticado(usuarioId))
                return Forbid();
            
            var tarefa = new Tarefa
            {
                Titulo = criarTarefaDto.Titulo,
                Descricao = criarTarefaDto.Descricao,
                DataDeVencimento = criarTarefaDto.DataDeVencimento
            };

            var tarefaCadastrada = await _tarefaService.CriarAsync(usuarioId, tarefa);
            var response = MapearParaResponseDto(tarefaCadastrada);

            return Created(
                $"/api/usuarios/{usuarioId}/tarefas/{tarefaCadastrada.Id}",
                response
            );
        }

        [HttpPatch("{id:guid}")]
        [EndpointSummary("Atualizar uma tarefa")]
        [EndpointDescription("Atualiza os campos informados de uma tarefa. Os campos omitidos permanecem inalterados. Não permite atualizar tarefas concluídas, utilizar um título duplicado para o mesmo usuário ou informar uma data de vencimento anterior à data atual.")]
        [ProducesResponseType(typeof(TarefaResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TarefaResponseDto>> AtualizarAsync(Guid usuarioId, Guid id, [FromBody] AtualizarTarefaDto atualizarTarefaDto)
        {
            if (!UsuarioAutenticado(usuarioId))
                return Forbid();

            var tarefa = new Tarefa
            {
                Titulo = atualizarTarefaDto.Titulo ?? string.Empty,
                Descricao = atualizarTarefaDto.Descricao ?? string.Empty,
                DataDeVencimento = atualizarTarefaDto.DataDeVencimento ?? default
            };

            var tarefaAtualizada = await _tarefaService.AtualizarAsync(usuarioId, id, tarefa);
            var response = MapearParaResponseDto(tarefaAtualizada);

            return Ok(response);
        }

        [HttpPatch("{id:guid}/concluir")]
        [EndpointSummary("Concluir uma tarefa")]
        [EndpointDescription("Altera o status da tarefa para Concluida. Uma tarefa já concluída não pode ser concluída novamente.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ConcluirAsync(Guid usuarioId, Guid id)
        {
            if (!UsuarioAutenticado(usuarioId))
                return Forbid();

            await _tarefaService.ConcluirAsync(usuarioId, id);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [EndpointSummary("Remover uma tarefa")]
        [EndpointDescription("Remove uma tarefa pertencente ao usuário informado.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RemoverAsync(Guid usuarioId, Guid id)
        {
            if (!UsuarioAutenticado(usuarioId))
                return Forbid();

            await _tarefaService.RemoverAsync(usuarioId, id);
            return NoContent();
        }

        private bool UsuarioAutenticado(Guid usuarioId)
        {
            var usuarioAutenticadoId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(usuarioAutenticadoId, out var parsedId)
                && parsedId == usuarioId;
        }
        private static TarefaResponseDto MapearParaResponseDto(Tarefa tarefa)
        {
            return new TarefaResponseDto
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                DataDeVencimento = tarefa.DataDeVencimento,
                Status = tarefa.Status,
                UsuarioId = tarefa.UsuarioId
            };
        }

    }
}
