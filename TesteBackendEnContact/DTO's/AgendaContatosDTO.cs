using System.Collections.Generic;
using TesteBackendEnContact.Models;

namespace TesteBackendEnContact.DTO_s
{
    public class AgendaContatosDTO
    {
        public int IdDaAgenda { get; set; }
        public string? NomeDaAgenda { get; set; }
        public List<Contato>? Contatos { get; set; }
    }
}
