using Microsoft.EntityFrameworkCore;


namespace AiderHubAtual.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Inscricao> Inscricoes { get; set; }
        public DbSet<Ong> Ongs { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Voluntario> Voluntarios { get; set; }
        public DbSet<Relatorio> Relatorios { get; set; }
        public DbSet<EventoInscricao> EventosInscricoes { get; set; }
        //public DbSet<Calendario> Calendarios { get; set; }
    }
}
