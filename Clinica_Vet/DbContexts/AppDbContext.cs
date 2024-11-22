using Clinica_Vet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml.Documents;
using Windows.System;

namespace Clinica_Vet.DbContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Veterinario> Veterinarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Animal> Animais { get; set; }
        public DbSet<Especie> Especies { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Tratamento> Tratamentos { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Exame> Exames { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string databasePath = @"C:\Users\rhuan\source\repos\Clinica_Vet\Clinica_Vet\veterinario.db";
            optionsBuilder.UseSqlite($"Data Source={databasePath}");
       
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cliente 1:N Animal
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Animais)
                .WithOne(a => a.Cliente)
                .HasForeignKey(a => a.ClienteId);

            // Animal 1:N Tratamento
            modelBuilder.Entity<Animal>()
                .HasMany(a => a.Tratamentos)
                .WithOne(t => t.Animal)
                .HasForeignKey(t => t.AnimalId);

            // Tratamento 1:N Consulta
            modelBuilder.Entity<Tratamento>()
                .HasMany(t => t.Consultas)
                .WithOne(c => c.Tratamento)
                .HasForeignKey(c => c.TratamentoId);

            // Consulta 1:N Exame
            modelBuilder.Entity<Consulta>()
                .HasMany(c => c.Exames)
                .WithOne(e => e.Consulta)
                .HasForeignKey(e => e.ConsultaId);

            // Consulta 1:1 Veterinario
            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Veterinario)
                .WithMany()
                .HasForeignKey(c => c.VeterinarioId);

            // Consulta 1:1 Cliente
            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.ClienteId);
        }
    }
}
