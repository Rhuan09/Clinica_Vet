using Clinica_Vet.DbContexts;
using Clinica_Vet.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Clinica_Vet.DataAccess
{
    public class ProdutoHistoricoDAO : IDataAccess<ProdutoHistorico>
    {
        private readonly AppDbContext _context;

        public ProdutoHistoricoDAO(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarAsync(ProdutoHistorico entity)
        {
            _context.ProdutosHistorico.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(ProdutoHistorico entity)
        {
            _context.ProdutosHistorico.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> RemoverAsync(ProdutoHistorico entity)
        {
            _context.ProdutosHistorico.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ProdutoHistorico>> ConsultarAsync()
        {
            return await _context.ProdutosHistorico.Include(ph => ph.Veterinario).ToListAsync();
        }

        public async Task<List<ProdutoHistorico>> ConsultarAsync(Expression<Func<ProdutoHistorico, bool>> filtro)
        {
            return await _context.ProdutosHistorico.Include(ph => ph.Veterinario).Where(filtro).ToListAsync();
        }
    }
}
