using System.Collections.Generic;
using TesteBackendEnContact.Models;

namespace TesteBackendEnContact.Models
{
    public class Agenda 
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Contato> Contatos { get; set; }

        
    }
}
