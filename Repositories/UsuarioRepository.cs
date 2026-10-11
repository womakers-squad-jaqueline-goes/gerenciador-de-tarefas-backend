using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace GerenciadorDeTarefas.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public UsuarioRepository(AppDbContext context, IPasswordHasher<Usuario> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<Usuario?> ObterPorIdAsync(Guid id)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(usuario => usuario.Id == id);
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(usuario => usuario.Email == email);
    }

    public async Task AdicionarAsync(Usuario usuario)
    {
        usuario.Senha = _passwordHasher.HashPassword(usuario, usuario.Senha);
       
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();
    }
}