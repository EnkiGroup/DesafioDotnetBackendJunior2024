using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using TesteBackendEnContact.Data;
using TesteBackendEnContact.Models;

namespace TesteBackendEnContact.Repositories.Interface
{
    public interface IContatoImportServiceRepository
    {
        Task<int> ImportContatosAsync(Stream stream);
    }

    public class ContatoImportService : IContatoImportServiceRepository
    {
        private readonly AppDbContext _appDbContext;

        public ContatoImportService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<int> ImportContatosAsync(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csvReader.Context.RegisterClassMap<ContatoCSVMap>();
                    var contatos = csvReader.GetRecords<ContatoCSVModel>();

                    int importedCount = 0;
                    foreach (var contato in contatos)
                    {
                        try
                        {
                            var novaAgenda = await GetOrCreateAgendaAsync(contato.NomeAgenda);
                            if (novaAgenda == null)
                            {
                                throw new Exception($"Agenda '{contato.NomeAgenda}' não encontrada.");
                            }

                            Empresa empresa = null;
                            if (!string.IsNullOrEmpty(contato.NomeEmpresa))
                            {
                                empresa = await _appDbContext.Empresas.FirstOrDefaultAsync(e => e.Nome == contato.NomeEmpresa);
                                if (empresa == null)
                                {
                                    
                                    throw new Exception($"Empresa '{contato.NomeEmpresa}' não encontrada.");
                                }
                            }

                            var novoContato = new Contato
                            {
                                Nome = contato.NomeContato,
                                Email = contato.Email,
                                Telefone = contato.Telefone,
                                Agenda = novaAgenda,
                                Empresa = empresa
                            };

                            _appDbContext.Contatos.Add(novoContato);
                            importedCount++;
                        }
                        catch (Exception ex)
                        {
                            
                            throw new Exception($"Erro ao importar contato!");
                            
                        }
                    }

                    await _appDbContext.SaveChangesAsync();
                    return importedCount;
                }
            }
        }

        private async Task<Agenda> GetOrCreateAgendaAsync(string nomeAgenda)
        {
            var agenda = await _appDbContext.Agendas.FirstOrDefaultAsync(a => a.Nome == nomeAgenda);
            if (agenda == null)
            {
                agenda = new Agenda { Nome = nomeAgenda };
                _appDbContext.Agendas.Add(agenda);
                await _appDbContext.SaveChangesAsync();
            }

            return agenda;
        }


    }
}