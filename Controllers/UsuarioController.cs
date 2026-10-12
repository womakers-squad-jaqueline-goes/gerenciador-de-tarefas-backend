using GerenciadorDeTarefas.DTOs.Usuario;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorDeTarefas.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("{id:guid}")]
        [EndpointSummary("Obter usuário por ID")]
        [EndpointDescription("Retorna os dados de um usuário específico.")]
        [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterPorIdAsync(Guid id)
        {
            var usuario = await _usuarioService.ObterPorIdAsync(id);

            var response = MapearParaResponseDto(usuario);

            return Ok(response);
        }

        [HttpPost]
        [EndpointSummary("Criar novo usuário")]
        [EndpointDescription("Cadastra um novo usuário com os dados fornecidos. O e-mail deve ser válido e não pode estar cadastrado para outro usuário.")]
        [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarAsync([FromBody] CriarUsuarioDto usuarioDto)
        {
            var usuarioCadastrado = await _usuarioService.CadastrarAsync(new Usuario
            {
                Nome = usuarioDto.Nome.Trim(),
                Email = usuarioDto.Email.Trim(),
                Senha = usuarioDto.Senha
            });

            var response = MapearParaResponseDto(usuarioCadastrado);

            return Created(
                $"/api/usuarios/{usuarioCadastrado.Id}",
                response
            );
        }

        private static UsuarioResponseDto MapearParaResponseDto(Usuario usuario)
        {
            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };
        }
    }
}
