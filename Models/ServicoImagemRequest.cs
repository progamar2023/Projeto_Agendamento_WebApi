namespace WebApi.Models
{
    public class ServicoImagemRequest
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string? Imagem { get; set; }
        public string? ImagemBase64 { get; set; }
        public int TipoServicoId { get; set; }
        public DateTime? DataCriacao { get; set; } = DateTime.Now;
        public bool Habilitado { get; set; }
    }
}
