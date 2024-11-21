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
    public class ExameDAO : IDataAccess<Exame>
    {
        private readonly AppDbContext _context;

        public ExameDAO(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarAsync(Exame entity)
        {
            _context.Exames.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Exame entity)
        {
            _context.Exames.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> RemoverAsync(Exame entity)
        {
            _context.Exames.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Exame>> ConsultarAsync()
        {
            return await _context.Exames.ToListAsync();
        }
    }

}
