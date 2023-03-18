namespace WebApi.Models
{
    public class ConfiguracoesRequest
    {

        public int? Id { get; set; }
        public string? Local { get; set; }
        public int? Posicao { get; set; }
        public string? Tipo { get; set; }
        public string? Descricao { get; set; }
        public string? Valor { get; set; }
        public TimeSpan? DataInicial { get; set; }
        public TimeSpan? DataFinal { get; set; }
        public string? ImagemBase64 { get; set; }

        public ConfiguracoesRequest(int? id, string? local, int? posicao, string? tipo, string? descricao, string? valor, TimeSpan? dataInicial, TimeSpan? dataFinal, string? imagemBase64)
        {
            Id = id;
            Local = local;
            Posicao = posicao;
            Tipo = tipo;
            Descricao = descricao;
            Valor = valor;
            DataInicial = dataInicial;
            DataFinal = dataFinal;
            ImagemBase64 = imagemBase64;
        }

    }
}
