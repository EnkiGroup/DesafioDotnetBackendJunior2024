using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TesteBackendEnContact.Data;
using TesteBackendEnContact.Models;
using TesteBackendEnContact.Repositories.Interface;

namespace TesteBackendEnContact.Repositories
{
    public class AgendaRepository : IAgendaRepository
    {
        private readonly AppDbContext _appDbContext;

        public AgendaRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
             

        async Task<Agenda> IAgendaRepository.AddAgendaAsync(Agenda agenda)
        {
            _appDbContext.Agendas.Add(agenda);
            await _appDbContext.SaveChangesAsync();
            return agenda;
        }

        async Task<IEnumerable<Agenda>> IAgendaRepository.GetAgendasAsync()
        {
            return await _appDbContext.Agendas.ToListAsync();
        }

        async Task<Agenda> IAgendaRepository.GetAgendaByIdAsync(int id)
        {
            return await _appDbContext.Agendas.FirstOrDefaultAsync(x => x.Id == id);
        }
              

        async Task IAgendaRepository.UpdateAgendaAsync(Agenda agenda)
        {
            _appDbContext.Entry(agenda).State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();
        }

        async Task IAgendaRepository.DeleteAgendaAsync(int id)
        {
            var agenda = await _appDbContext.Agendas.FindAsync(id);
            if (agenda != null)
            {
                _appDbContext.Agendas.Remove(agenda);
                await _appDbContext.SaveChangesAsync();
            }
        }

        async Task<IActionResult> IAgendaRepository.ExportAgendaAsync(int id)
        {
            var empresa = await _appDbContext.Empresas.FindAsync(id);
            if (empresa == null)
            {
                throw new ArgumentException("Empresa não encontrada.");
            }

            var empresaJson = JsonConvert.SerializeObject(empresa, Formatting.Indented);
            var fileBytes = Encoding.UTF8.GetBytes(empresaJson);
            var fileName = $"empresa_{id}.json";

            var contentType = "application/json";

            var fileContentResult = new FileContentResult(fileBytes, contentType)
            {
                FileDownloadName = fileName
            };

            return fileContentResult;
        }
    }
}
