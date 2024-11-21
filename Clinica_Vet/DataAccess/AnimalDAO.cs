using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<List<Animal>> ConsultarAsync()
        {
            return await _context.Animais.ToListAsync();
        }
    }

}
