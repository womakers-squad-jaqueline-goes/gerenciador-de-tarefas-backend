using GerenciadorDeTarefas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeTarefas.Data
{
    public static class DbInitializer
    {
        public static async Task InitializarAsync(AppDbContext context)
        {
            await context.Database.MigrateAsync();

            if (await context.Usuarios.AnyAsync())
                return;

            var usuario = new Usuario
            {
                Nome = "Juliana Matias",
                Email = "juliana.matias@example.com"
            };

            var senhaHasher = new PasswordHasher<Usuario>();
            usuario.Senha = senhaHasher.HashPassword(usuario, "SenhaForte123!");

            usuario.ListaDeTarefas = new List<Tarefa>
            {
                new Tarefa
                {
                    Titulo = "Estudar C#",
                    Descricao = "Revisar orientação a objetos e LINQ",
                    DataDeVencimento = DateTime.Now.AddDays(7),
                },

                new Tarefa
                {
                    Titulo = "Estudar Entity Framework Core",
                    Descricao = "Praticar migrations e consultas",
                    DataDeVencimento = DateTime.Now.AddDays(5),
                }
            };

            

            await context.Usuarios.AddAsync(usuario);
            await context.SaveChangesAsync();
        }
    }
}
