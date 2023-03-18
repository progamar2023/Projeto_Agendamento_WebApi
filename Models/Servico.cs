
using System.Reflection.Metadata;

namespace WebApi.Models
{
    public class Servico
    {

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string? Imagem { get; set; }
        public int TipoServicoId { get; set; }
        public DateTime? DataCriacao { get; set; } = DateTime.Now;
        public bool Habilitado { get; set; }



        public Servico(int id, string nome, string descricao, string? imagem, int tipoServicoId, DateTime? dataCriacao, bool habilitado)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
            Imagem = imagem;
            TipoServicoId = tipoServicoId;
            DataCriacao = dataCriacao;
            Habilitado = habilitado;
        }
    }
}



