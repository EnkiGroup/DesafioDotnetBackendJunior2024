using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.DTO_s;
using TesteBackendEnContact.PagedList;
using TesteBackendEnContact.Repositories.Interface;
using TesteBackendEnContact.Models;

namespace TesteBackendEnContact.Service
{
    public class ContatoAppService
    {
        private readonly IContatoRepository _contatoRepository;

        public ContatoAppService(IContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;
        }

        public async Task<PagedList<Contato>> SearchContatosPaginadoAsync(string searchTerm, int pageNumber, int pageSize)
        {
            return await _contatoRepository.SearchContatosAsync(searchTerm, pageNumber, pageSize);
        }

        public async Task<List<AgendaContatosDTO>> SearchContatosDaEmpresaAsync(string nomeEmpresa)
        {
            return await _contatoRepository.SearchContatosDaEmpresaAsync(nomeEmpresa);
        }
    }
}
