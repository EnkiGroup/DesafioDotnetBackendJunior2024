using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using TesteBackendEnContact.Repositories.Interface;
using TesteBackendEnContact.Models;


namespace TesteBackendEnContact.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContatoController : ControllerBase
    {
        private readonly IContatoRepository _contatoAppService;

        public ContatoController(IContatoRepository contatoAppService)
        {
            _contatoAppService = contatoAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetContatosAsync()
        {
            try
            {
                var contatos = await _contatoAppService.GetContatosAsync();
                return Ok(contatos);
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro ao obter todos os contatos!");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContatoByIdAsync(int id)
        {
            try
            {
                var contato = await _contatoAppService.GetContatoByIdAsync(id);
                if (contato == null)
                {
                    return NotFound();
                }
                return Ok(contato);
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro ao obter o contato!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddContatoAsync(Contato contato)
        {
            try
            {
                await _contatoAppService.AddContatoAsync(contato);
                
                return CreatedAtAction(nameof(GetContato), new { id = contato.Id }, contato);
            }
            catch (Exception)
            {
                
                return StatusCode(500, $"Erro ao criar o contato!");
            }

        }

        [HttpGet("{id}/GetContato")]
        public async Task<IActionResult> GetContato(int id)
        {
            try
            {
                var contato = await _contatoAppService.GetContatoByIdAsync(id);
                if (contato == null)
                {
                    return NotFound();
                }
                return Ok(contato);
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro ao obter o contato!");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContatoAsync(int id, Contato contato)
        {
            if (id != contato.Id)
            {
                return BadRequest("ID do contato não corresponde ao ID fornecido nos parâmetros.");
            }

            try
            {
                await _contatoAppService.UpdateContatoAsync(contato);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro ao atualizar o contato!");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContatoAsync(int id)
        {
            try
            {
                await _contatoAppService.DeleteContatoAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro ao excluir o contato!");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchContatos(string searchTerm, int pageNumber, int pageSize)
        {
            try
            {
                var contatos = await _contatoAppService.SearchContatosAsync(searchTerm, pageNumber, pageSize);
                return Ok(contatos);
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro ao pesquisar contatos!");
            }
        }

        [HttpGet("{id}/export")]
        public async Task<IActionResult> ExportContato(int id)
        {
            try
            {
                var contato = await _contatoAppService.GetContatosAsync();
                if (contato == null)
                {
                    return NotFound();
                }

                var contatoJson = JsonConvert.SerializeObject(contato, Newtonsoft.Json.Formatting.Indented);
                var tempFilePath = Path.GetTempFileName();
                await System.IO.File.WriteAllTextAsync(tempFilePath, contatoJson);

                var fileBytes = await System.IO.File.ReadAllBytesAsync(tempFilePath);
                var fileName = $"contato_{id}.json";

                return File(fileBytes, "application/json", fileName);
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro ao exportar o contato!");
            }
        }
    }
}
