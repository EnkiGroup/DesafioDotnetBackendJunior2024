using Microsoft.EntityFrameworkCore;
using TesteBackendEnContact.Data;
using TesteBackendEnContact.DTO_s;
using TesteBackendEnContact.Repositories.Interface;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using TesteBackendEnContact.Models;
using TesteBackendEnContact.PagedList;
using System.IO;
using System;
using System.Linq;

namespace TesteBackendEnContact.Repositories
{
    public class ContatoRepository : IContatoRepository
    {
        private readonly AppDbContext _appDbContext;

        public ContatoRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddContatoAsync(Contato contato)
        {
            _appDbContext.Contatos.Add(contato);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteContatoAsync(int id)
        {
            var contato = await _appDbContext.Contatos.FindAsync(id);
            if (contato != null)
            {
                _appDbContext.Contatos.Remove(contato);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task ExportContato(int id)
        {
            var contato = await _appDbContext.Contatos.FindAsync(id);

            if (contato == null)
            {
                throw new ArgumentException("Contato não encontrado", nameof(id));
            }


            var filePath = "contato_exportado.csv";

            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                await writer.WriteLineAsync("Nome,Email,Telefone");

                await writer.WriteLineAsync($"{contato.Nome},{contato.Email},{contato.Telefone}");
            }
        }

        public async Task<Contato> GetContatoByIdAsync(int id)
        {
            return await _appDbContext.Contatos.FindAsync(id);
        }

        public async Task<IEnumerable<Contato>> GetContatosAsync()
        {
            return await _appDbContext.Contatos.ToListAsync();
        }

        public async Task<PagedList<Contato>> SearchContatosAsync(string searchTerm, int pageNumber, int pageSize)
        {
            var query = _appDbContext.Contatos
                .Include(c => c.Agenda)
                .Include(c => c.Empresa)
                .Where(c =>
                    c.Nome.Contains(searchTerm) ||
                    c.Email.Contains(searchTerm) ||
                    c.Telefone.Contains(searchTerm) ||
                    c.Agenda.Nome.Contains(searchTerm) ||
                    c.Empresa.Nome.Contains(searchTerm)
                )
                .OrderBy(c => c.Nome);

            return await PagedList<Contato>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<List<AgendaContatosDTO>> SearchContatosDaEmpresaAsync(string nomeEmpresa)
        {
            var contatosDaEmpresa = await _appDbContext.Contatos
               .Include(c => c.Agenda)
               .Include(c => c.Empresa)
               .Where(c => c.Empresa.Nome.Contains(nomeEmpresa))
               .ToListAsync();

            var contatosAgrupadosPorAgenda = contatosDaEmpresa
                .GroupBy(c => new { c.Agenda.Id, c.Agenda.Nome })
                .Select(g => new AgendaContatosDTO
                {
                    IdDaAgenda = g.Key.Id,
                    NomeDaAgenda = g.Key.Nome,
                    Contatos = g.ToList()
                })
                .ToList();

            return contatosAgrupadosPorAgenda;
        }

        public async Task UpdateContatoAsync(Contato contato)
        {
            _appDbContext.Entry(contato).State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();
        }
    }
}



