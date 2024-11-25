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
    public class ClienteDAO : IDataAccess<Cliente>
    {
        private readonly AppDbContext _context;

        public ClienteDAO(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarAsync(Cliente entity)
        {
            _context.Clientes.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Cliente entity)
        {
            _context.Clientes.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> RemoverAsync(Cliente entity)
        {
            _context.Clientes.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Cliente>> ConsultarAsync()
        {
            return await _context.Clientes.ToListAsync();
        }


        public async Task<List<Cliente>> ConsultarAsync(Expression<Func<Cliente, bool>> filtro)
        {
            return await _context.Clientes.Where(filtro).ToListAsync();
        }
    }

}
