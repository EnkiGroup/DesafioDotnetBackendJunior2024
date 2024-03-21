using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using TesteBackendEnContact.Models;
using TesteBackendEnContact.Repositories.Interface;

namespace MeuProjeto.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AgendaController : ControllerBase
    {
        private readonly IAgendaRepository _agendaAppService;

        public AgendaController(IAgendaRepository agendaAppService)
        {
            _agendaAppService = agendaAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAgendasAsync()
        {
            try
            {
                var agendas = await _agendaAppService.GetAgendasAsync();
                return Ok(agendas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter todas as agendas: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAgendaByIdAsync(int id)
        {
            try
            {
                var agenda = await _agendaAppService.GetAgendaByIdAsync(id);
                if (agenda == null)
                {
                    return NotFound();
                }
                return Ok(agenda);
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro ao obter a agenda!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAgendaAsync(Agenda agenda)
        {
            try
            {
                if (agenda == null)
                {
                    return BadRequest("A agenda enviada está vazia.");
                }

                var createdAgenda = await _agendaAppService.AddAgendaAsync(agenda);
                return CreatedAtAction(nameof(AddAgendaAsync), new { id = createdAgenda.Id }, createdAgenda);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("A agenda enviada está vazia.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro ao criar a agenda!");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAgendaAsync(int id, Agenda agenda)
        {
            if (id != agenda.Id)
            {
                return BadRequest("ID da agenda não corresponde ao ID fornecido nos parâmetros.");
            }

            try
            {
                await _agendaAppService.UpdateAgendaAsync(agenda);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro ao atualizar a agenda!");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgendaAsync(int id)
        {
            try
            {
                await _agendaAppService.DeleteAgendaAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro ao excluir a agenda!");
            }
        }

        [HttpGet("{id}/export")]
        public async Task<IActionResult> ExportAgendaAsync(int id)
        {
            try
            {
                var agenda = await _agendaAppService.ExportAgendaAsync(id);
                if (agenda == null)
                {
                    return NotFound();
                }

                var agendaJson = JsonConvert.SerializeObject(agenda, Newtonsoft.Json.Formatting.Indented);
                var tempFilePath = Path.GetTempFileName();
                await System.IO.File.WriteAllTextAsync(tempFilePath, agendaJson);

                var fileBytes = await System.IO.File.ReadAllBytesAsync(tempFilePath);
                var fileName = $"agenda_{id}.json";

                return File(fileBytes, "application/json", fileName);
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro ao exportar a agenda!");
            }
        }
    }
}
