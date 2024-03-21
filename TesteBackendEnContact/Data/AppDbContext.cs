using Microsoft.EntityFrameworkCore;
using System;
using TesteBackendEnContact.Models;



namespace TesteBackendEnContact.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
            public DbSet<Agenda> Agendas { get; set; }
            public DbSet<Empresa> Empresas { get; set; }
            public DbSet<Contato> Contatos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        internal void CreateEmpresa(Empresa empresa)
        {
            throw new NotImplementedException();
        }

    }
}
