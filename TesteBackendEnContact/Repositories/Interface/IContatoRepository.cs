using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.DTO_s;
using TesteBackendEnContact.Models;
using TesteBackendEnContact.PagedList;

namespace TesteBackendEnContact.Repositories.Interface
{
    public interface IContatoRepository
    {
        Task<IEnumerable<Contato>> GetContatosAsync();
        Task<Contato> GetContatoByIdAsync(int id);
        Task AddContatoAsync(Contato contato);
        Task UpdateContatoAsync(Contato contato);
        Task DeleteContatoAsync(int id);
        Task ExportContato(int id);
        Task<PagedList<Contato>> SearchContatosAsync(string searchTerm, int pageNumber, int pageSize);
        Task<List<AgendaContatosDTO>> SearchContatosDaEmpresaAsync(string nomeEmpresa);
        
    }
}
