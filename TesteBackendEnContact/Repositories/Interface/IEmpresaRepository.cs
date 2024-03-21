using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Models;

namespace TesteBackendEnContact.Repositories.Interface
{
    public interface IEmpresaRepository
    {
        Task<IEnumerable<Empresa>> GetEmpresaAsync();
        Task<Empresa> GetEmpresaByIdAsync(int id);
        Task<Empresa> AddEmpresaAsync(Empresa empresa);
        Task UpdateEmpresaAsync(Empresa empresa);
        Task DeleteEmpresaAsync(int id);
        Task ExportEmpresa(int id);
    }
}
