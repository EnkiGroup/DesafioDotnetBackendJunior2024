using CsvHelper.Configuration;

namespace TesteBackendEnContact.Models
{
    public class ContatoCSVModel
    {
        public string? NomeAgenda { get; set; }
        public string? NomeContato { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? NomeEmpresa { get; set; }
    }
    public sealed class ContatoCSVMap : ClassMap<ContatoCSVModel>
    {
        public ContatoCSVMap()
        {
            Map(m => m.NomeAgenda).Name("Nome da Agenda");
            Map(m => m.NomeContato).Name("Nome do Contato");
            Map(m => m.Email).Name("Email");
            Map(m => m.Telefone).Name("Telefone");
            Map(m => m.NomeEmpresa).Name("Nome da Empresa");
        }
    }

}
