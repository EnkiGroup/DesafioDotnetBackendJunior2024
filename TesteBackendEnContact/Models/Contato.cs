using TesteBackendEnContact.Models;

namespace TesteBackendEnContact.Models
{
    public class Contato 
    {
        public int Id { get; set; } 
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public int AgendaId { get; set; }
        public Agenda Agenda { get; set; }
        public int? EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        
    }
}
