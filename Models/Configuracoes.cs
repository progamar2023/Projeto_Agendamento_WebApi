namespace WebApi.Models
{
    public class Configuracoes
    {


        public int? Id { get; set; }
        public string? Local { get; set; }
        public int? Posicao { get; set; }
        public string? Tipo { get; set; }
        public string? Descricao { get; set; }
        public string? Valor { get; set; }
        public TimeSpan? DataInicial { get; set; }
        public TimeSpan? DataFinal { get; set; }

        public Configuracoes(int id, string? local, int? posicao, string? tipo, string? descricao, string? valor, TimeSpan? dataInicial, TimeSpan? dataFinal)
        {
            Id = id;
            Local = local;
            Posicao = posicao;
            Tipo = tipo;
            Descricao = descricao;
            Valor = valor;
            DataInicial = dataInicial;
            DataFinal = dataFinal;
        }

        public Configuracoes(int? id, string? local, int? posicao, string? tipo, string? descricao, string? valor, TimeSpan? dataInicial, TimeSpan? dataFinal)
        {
            Id = id;
            Local = local;
            Posicao = posicao;
            Tipo = tipo;
            Descricao = descricao;
            Valor = valor;
            DataInicial = dataInicial;
            DataFinal = dataFinal;
        }
    }
}
