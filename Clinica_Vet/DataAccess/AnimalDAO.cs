using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Clinica_Vet.DbContexts;
using Clinica_Vet.Models;
using Microsoft.EntityFrameworkCore;

namespace Clinica_Vet.DataAccess
{
    public class AnimalDAO : IDataAccess<Animal>
    {
        private readonly AppDbContext _context;

        public AnimalDAO(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarAsync(Animal entity)
        {
            _context.Animais.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Animal entity)
        {
            _context.Animais.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> RemoverAsync(Animal entity)
        {
            _context.Animais.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Animal>> ConsultarComPropriedadesAsync(Expression<Func<Animal, bool>> filtro = null)
        {
            IQueryable<Animal> query = _context.Animais
                .Include(a => a.Especie)
                .Include(a => a.Tratamentos)
                .Include(a => a.Consultas)
                    .ThenInclude(c => c.Exames);



            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            return await query.ToListAsync();
        }

        public async Task<List<Animal>> ConsultarAsync()
        {
            return await _context.Animais.ToListAsync();
        }

        public async Task<List<Animal>> ConsultarAsync(Expression<Func<Animal, bool>> filtro = null)
        {
            IQueryable<Animal> query = _context.Animais
                .Include(a => a.Especie)         // Inclui a espécie associada
                .Include(a => a.Tratamentos)    // Inclui os tratamentos
                .Include(a => a.Consultas)      // Inclui as consultas
                    .ThenInclude(c => c.Exames); // Inclui os exames das consultas

            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            return await query.ToListAsync();
        }
    }
}
