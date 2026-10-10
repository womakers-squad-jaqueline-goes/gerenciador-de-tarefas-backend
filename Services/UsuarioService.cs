public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Usuario?> ObterPorIdAsync(Guid id)
    {
        return await _usuarioRepository.ObterPorIdAsync(id);
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        return await _usuarioRepository.ObterPorEmailAsync(email);
    }

    public async Task<Usuario> CadastrarAsync(Usuario usuario)
    {
        var existente = await _usuarioRepository.ObterPorEmailAsync(usuario.Email);

        if (existente != null)
            throw new InvalidOperationException("E-mail já cadastrado.");

        usuario.Id = Guid.NewGuid();

        await _usuarioRepository.AdicionarAsync(usuario);

        return usuario;
    }
}