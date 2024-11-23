using Clinica_Vet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

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
            optionsBuilder.UseSqlite($"Data Source={databasePath}")
                .EnableSensitiveDataLogging() // Habilita o logging de dados sensíveis
                .LogTo(Console.WriteLine, LogLevel.Information); // Loga no console
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cliente 1:N Animal
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Animais)
                .WithOne(a => a.Cliente)
                .HasForeignKey(a => a.ClienteId);

            // Animal 1:N Consulta
            modelBuilder.Entity<Animal>()
                .HasMany(a => a.Consultas)
                .WithOne(c => c.Animal)
                .HasForeignKey(c => c.AnimalId)
                .OnDelete(DeleteBehavior.Restrict);

            // Veterinario 1:N Consulta
            modelBuilder.Entity<Veterinario>()
                .HasMany(v => v.Consultas)
                .WithOne(c => c.Veterinario)
                .HasForeignKey(c => c.VeterinarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cliente 1:N Consulta
            modelBuilder.Entity<Cliente>()
                .HasMany(cl => cl.Consultas)
                .WithOne(c => c.Cliente)
                .HasForeignKey(c => c.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Consulta 1:N Exame
            modelBuilder.Entity<Exame>()
                .HasOne(e => e.Consulta)
                .WithMany(c => c.Exames)
                .HasForeignKey(e => e.ConsultaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tratamento N:1 Animal
            modelBuilder.Entity<Tratamento>()
                .HasOne(t => t.Animal)
                .WithMany(a => a.Tratamentos)
                .HasForeignKey(t => t.AnimalId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Consulta N:1 Tratamento
            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Tratamento)
                .WithMany(t => t.Consultas)
                .HasForeignKey(c => c.TratamentoId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
