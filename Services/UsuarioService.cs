using GerenciadorDeTarefas.Exceptions;
using GerenciadorDeTarefas.Models;
using Microsoft.AspNetCore.Identity;
using GerenciadorDeTarefas.Repositories.Interfaces;
using GerenciadorDeTarefas.Services.Interfaces;

namespace GerenciadorDeTarefas.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UsuarioService(IUsuarioRepository usuarioRepository, IPasswordHasher<Usuario> passwordHasher)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
        }
        public async Task<Usuario> CadastrarAsync(Usuario usuario)
        {
            var usuarioExistente = await _usuarioRepository.ObterPorEmailAsync(usuario.Email);

            if (usuarioExistente != null)
                throw new ConflictException("Existe um usuário cadastrado com este e-mail");

            usuario.Senha = _passwordHasher.HashPassword(usuario, usuario.Senha);

            await _usuarioRepository.AdicionarAsync(usuario);

            return usuario;
        }

        public async Task<bool> ExistePorEmailAsync(string email)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(email);

            return usuario != null;
        }

        public async Task<Usuario> ObterPorIdAsync(Guid id)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);

            if (usuario == null)
                throw new NotFoundException("Usuário não encontrado");
            
            return usuario;
        }
    }
}
