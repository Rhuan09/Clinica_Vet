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
    public class ConsultaDAO : IDataAccess<Consulta>
    {
        private readonly AppDbContext _context;

        public ConsultaDAO(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarAsync(Consulta entity)
        {
            _context.Consultas.Add(entity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Erro ao salvar alterações: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Exceção interna: {ex.InnerException.Message}");
                }
                throw;
            }
        }

        public async Task AtualizarAsync(Consulta entity)
        {
            _context.Consultas.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> RemoverAsync(Consulta entity)
        {
            _context.Consultas.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Consulta>> ConsultarAsync()
        {
            return await _context.Consultas.ToListAsync();
        }

        public async Task<List<Consulta>> ConsultarAsync(Expression<Func<Consulta, bool>> filtro)
        {
            return await _context.Consultas.Where(filtro).ToListAsync();
        }
    }

}
