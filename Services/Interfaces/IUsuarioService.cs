using GerenciadorDeTarefas.Models;

namespace GerenciadorDeTarefas.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> ObterPorIdAsync(Guid id);
        Task<bool> ExistePorEmailAsync(string email);
        Task<Usuario> CadastrarAsync(Usuario usuario);

    }
}
