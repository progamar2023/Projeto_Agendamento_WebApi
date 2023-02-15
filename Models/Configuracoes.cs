namespace WebApi.Models
{
    public class Configuracoes
    {
        public int Id { get; set; }
        public string? Local { get; set; }
        public int? Posicao { get; set; }
        public string? Tipo { get; set; }
        public string? Descricao { get; set; }
        public string? Valor { get; set; }
    }
}
