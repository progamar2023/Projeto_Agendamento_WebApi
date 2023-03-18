namespace WebApi.Models
{
    public class ServicoRequest
    {
        public Servico Servico { get; set; }
        public TipoServico TipoServico { get; set; }
        public string? ImagemBase64 { get; set; }
    }
}
