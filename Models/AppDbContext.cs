using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Contato> Contatos { get; set; }
        public DbSet<TipoServico> TipoServicos { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<WebApi.Models.Configuracoes> Configuracoes { get; set; }
    }
}
