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

    public class VeterinarioDAO : IDataAccess<Veterinario>
    {
        private readonly AppDbContext _context;

        public VeterinarioDAO(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarAsync(Veterinario entity)
        {
            _context.Veterinarios.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Veterinario entity)
        {
            _context.Veterinarios.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> RemoverAsync(Veterinario entity)
        {
            _context.Veterinarios.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Veterinario>> ConsultarAsync()
        {
            return await _context.Veterinarios.ToListAsync();
        }
    }
}
