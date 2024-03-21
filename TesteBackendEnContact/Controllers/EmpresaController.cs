
using Microsoft.AspNetCore.Mvc;
using TesteBackendEnContact.Models;
using Microsoft.AspNetCore.Authorization;
using TesteBackendEnContact.Repositories.Interface;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.IO;

namespace MeuProjeto.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaRepository _empresaAppService;

        public EmpresaController(IEmpresaRepository empresaAppService)
        {
            _empresaAppService = empresaAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmpresaAsync()
        {
            try
            {
                var empresas = await _empresaAppService.GetEmpresaAsync();
                return Ok(empresas);
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro ao obter todas as empresas!");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmpresaByIdAsync(int id)
        {
            try
            {
                var empresa = await _empresaAppService.GetEmpresaByIdAsync(id);
                if (empresa == null)
                {
                    return NotFound();
                }
                return Ok(empresa);
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro ao obter a empresa!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEmpresaAsync(Empresa empresa)
        {
            try
            {
                await _empresaAppService.AddEmpresaAsync(empresa);
                
                return CreatedAtAction(nameof(AddEmpresaAsync), new { id = empresa.Id }, empresa);
            }
            catch (Exception)
            {
                
                return StatusCode(500, $"Erro ao criar a empresa!");
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmpresaAsync(int id, Empresa empresa)
        {
            if (id != empresa.Id)
            {
                return BadRequest("ID da empresa não corresponde ao ID fornecido nos parâmetros.");
            }

            try
            {
                await _empresaAppService.UpdateEmpresaAsync(empresa);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro ao atualizar a empresa!");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpresaAsync(int id)
        {
            try
            {
                await _empresaAppService.DeleteEmpresaAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro ao excluir a empresa!");
            }
        }

        [HttpGet("{id}/export")]
        public async Task<IActionResult> ExportEmpresa(int id)
        {
            try
            {
                var empresa = await _empresaAppService.GetEmpresaAsync();
                if (empresa == null)
                {
                    return NotFound();
                }

                var empresaJson = JsonConvert.SerializeObject(empresa, Newtonsoft.Json.Formatting.Indented);
                var tempFilePath = Path.GetTempFileName();
                await System.IO.File.WriteAllTextAsync(tempFilePath, empresaJson);

                var fileBytes = await System.IO.File.ReadAllBytesAsync(tempFilePath);
                var fileName = $"empresa_{id}.json";

                return File(fileBytes, "application/json", fileName);
            }
            catch (Exception)
            {
                return StatusCode(500, $"Erro ao exportar a empresa!");
            }
        }
    }
}

