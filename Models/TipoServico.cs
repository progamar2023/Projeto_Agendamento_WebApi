namespace WebApi.Models
{
    public class TipoServico
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime? DataCriacao { get; set; } = DateTime.Now;
        public bool Habilitado { get; set; }
    }
}
