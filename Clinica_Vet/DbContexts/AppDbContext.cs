using Clinica_Vet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Documents;
using System;
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
            optionsBuilder.UseSqlite($"Data Source={databasePath}")
            .LogTo(Console.WriteLine, LogLevel.Information); // Loga no console

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

            // Relação Consulta-Animal
            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Animal)
                .WithMany()
                .HasForeignKey(c => c.AnimalId)
                .OnDelete(DeleteBehavior.Cascade); // Deleção em cascata
            // Relação Consulta-Veterinário
            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Veterinario)
                .WithMany()
                .HasForeignKey(c => c.VeterinarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relação Consulta-Cliente
            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relação Consulta-Tratamento
            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Tratamento)
                .WithMany(t => t.Consultas)
                .HasForeignKey(c => c.TratamentoId)
                .IsRequired(false) // Permitir valores nulos
                .OnDelete(DeleteBehavior.SetNull);

            // Relação Consulta-Exame
            modelBuilder.Entity<Exame>()
                .HasOne(e => e.Consulta)
                .WithMany(c => c.Exames)
                .HasForeignKey(e => e.ConsultaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relação Tratamento-Animal
            modelBuilder.Entity<Tratamento>()
                .HasOne(t => t.Animal)
                .WithMany()
                .HasForeignKey(t => t.AnimalId)
                .IsRequired(false); // Permitir valores nulos

        }
    }
}
