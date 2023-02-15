namespace WebApi.Models
{
    public class Agendamento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public DateTime DataAgendamento { get; set; }
        public string Descricao { get; set; }
        public int ServicoId { get; set; }
        public int Status { get; set; }
        public int? UsuarioId { get; set; }
        public DateTime? DataCriacao { get; set; } = DateTime.Now;
    }
}
