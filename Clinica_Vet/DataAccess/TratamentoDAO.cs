using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Clinica_Vet.DbContexts;
using Clinica_Vet.Models;
using Microsoft.EntityFrameworkCore;


namespace Clinica_Vet.DataAccess
{
    public class TratamentoDAO : IDataAccess<Tratamento>
    {
        private readonly AppDbContext _context;

        public TratamentoDAO(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarAsync(Tratamento entity)
        {
            _context.Tratamentos.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Tratamento entity)
        {
            _context.Tratamentos.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> RemoverAsync(Tratamento entity)
        {
            _context.Tratamentos.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Tratamento>> ConsultarAsync()
        {
            return await _context.Tratamentos.ToListAsync();
        }

        public async Task<List<Tratamento>> ConsultarAsync(Expression<Func<Tratamento, bool>> filtro)
        {
            return await _context.Tratamentos.Where(filtro).ToListAsync();
        }

    }

}
