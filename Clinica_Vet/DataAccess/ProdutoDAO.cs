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
    public class ProdutoDAO : IDataAccess<Produto>
    {
        private readonly AppDbContext _context;

        public ProdutoDAO(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarAsync(Produto entity)
        {
            _context.Produtos.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Produto entity)
        {
            _context.Produtos.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> RemoverAsync(Produto entity)
        {
            _context.Produtos.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Produto>> ConsultarAsync()
        {
            return await _context.Produtos.ToListAsync();
        }

        public async Task<List<Produto>> ConsultarAsync(Expression<Func<Produto, bool>> filtro)
        {
            return await _context.Produtos.Where(filtro).ToListAsync();
        }
    }
    
}
