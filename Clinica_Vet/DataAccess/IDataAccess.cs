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
    public interface IDataAccess<T> where T : class
    {
        Task RegistrarAsync(T entity);      // Inserir um novo registro
        Task AtualizarAsync(T entity);     // Atualizar um registro existente
        Task<int> RemoverAsync(T entity);  // Remover um registro (retorna número de linhas afetadas)
        Task<List<T>> ConsultarAsync();    // Consultar todos os registros
        Task<List<T>> ConsultarAsync(Expression<Func<T, bool>> filtro); // Consultar com filtro

    }

}
