using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Models;

namespace TesteBackendEnContact.Repositories.Interface
{
    public interface IAgendaRepository
    {
        Task<IEnumerable<Agenda>> GetAgendasAsync();
        Task<Agenda> GetAgendaByIdAsync(int id);
        Task<Agenda> AddAgendaAsync(Agenda agenda);
        Task UpdateAgendaAsync(Agenda agenda);
        Task DeleteAgendaAsync(int id);
        Task<IActionResult> ExportAgendaAsync(int id);
        

    }
}
