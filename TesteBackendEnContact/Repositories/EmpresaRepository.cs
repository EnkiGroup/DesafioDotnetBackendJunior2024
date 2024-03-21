using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Data;
using TesteBackendEnContact.Models;
using TesteBackendEnContact.Repositories.Interface;

namespace TesteBackendEnContact.Repositories
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly AppDbContext _appDbContext;

        public EmpresaRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Empresa> AddEmpresaAsync(Empresa empresa)
        {
            _appDbContext.Empresas.Add(empresa);
            await _appDbContext.SaveChangesAsync();
            return empresa;
        }

        public async Task DeleteEmpresaAsync(int id)
        {
            var empresa = await _appDbContext.Empresas.FindAsync(id);
            if (empresa != null)
            {
                _appDbContext.Empresas.Remove(empresa);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task ExportEmpresa(int id)
        {
            var empresa = await _appDbContext.Empresas.FindAsync(id);
            if (empresa == null)
            {
                throw new ArgumentException("Empresa não encontrada.");
            }

            var empresaJson = JsonConvert.SerializeObject(empresa, Formatting.Indented);
            var tempFilePath = Path.GetTempFileName();
            await System.IO.File.WriteAllTextAsync(tempFilePath, empresaJson);

            var fileBytes = await File.ReadAllBytesAsync(tempFilePath);
            var fileName = $"empresa_{id}.json";
        }

        public async Task<IEnumerable<Empresa>> GetEmpresaAsync()
        {
            return await _appDbContext.Empresas.ToListAsync();
        }

        public async Task<Empresa> GetEmpresaByIdAsync(int id)
        {
            return await _appDbContext.Empresas.FindAsync
               (_appDbContext.Empresas.FirstOrDefault(x => x.Id == id));
        }

        public async Task UpdateEmpresaAsync(Empresa empresa)
        {
            _appDbContext.Entry(empresa).State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();
        }
    }
}