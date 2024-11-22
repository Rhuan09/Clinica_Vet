using Clinica_Vet.DbContexts;
using Clinica_Vet.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;

namespace Clinica_Vet.DataAccess
{
    public class EspecieDAO : IDataAccess<Especie>
    {
        private readonly AppDbContext _context;

        public EspecieDAO(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarAsync(Especie entity)
        {
            _context.Especies.Add(entity); // Adiciona uma nova espécie
            await _context.SaveChangesAsync(); // Salva alterações no banco de dados
        }

        public async Task AtualizarAsync(Especie entity)
        {
            _context.Especies.Update(entity); // Atualiza a espécie existente
            await _context.SaveChangesAsync(); // Salva alterações no banco de dados
        }

        public async Task<int> RemoverAsync(Especie entity)
        {
            _context.Especies.Remove(entity); // Remove a espécie
            return await _context.SaveChangesAsync(); // Retorna o número de linhas afetadas
        }

        public async Task<List<Especie>> ConsultarAsync()
        {
            return await _context.Especies.ToListAsync(); // Retorna todas as espécies do banco
        }

        public async Task<List<Especie>> ConsultarAsync(Expression<Func<Especie, bool>> filtro)
        {
            return await _context.Especies.Where(filtro).ToListAsync();
        }
    }
}
