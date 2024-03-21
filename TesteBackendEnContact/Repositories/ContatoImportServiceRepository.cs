using TesteBackendEnContact.Repositories.Interface;
using System.Threading.Tasks;
using System.Collections.Generic;
using TesteBackendEnContact.Models;
using System.IO;
using System;

namespace TesteBackendEnContact.Repositories
{
    public interface IContatoImportService
    {
        Task<IEnumerable<Contato>> ImportContatosFromCSV(Stream stream);
    }

    public class ContatoImportService : IContatoImportService
    {
        private readonly IContatoRepository _contatoRepository;
        private readonly IEmpresaRepository _empresaRepository;

        public ContatoImportService(IContatoRepository contatoRepository, IEmpresaRepository empresaRepository)
        {
            _contatoRepository = contatoRepository;
            _empresaRepository = empresaRepository;
        }

        public async Task<IEnumerable<Contato>> ImportContatosFromCSV(Stream stream)
        {

            using (var reader = new StreamReader(stream))
            {
                var contatos = new List<Contato>();

                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var values = line.Split(',');
                    if (values.Length < 3)
                    {
                        throw new Exception("Linha CSV inválida: não contém informações suficientes.");
                    }

                    var contato = new Contato
                    {
                        Nome = values[0],
                        Email = values[1],
                        Telefone = values[2]
                    };

                    contatos.Add(contato);
                }

                return contatos;

            }
        }
    }
}