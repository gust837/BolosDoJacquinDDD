using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace BolosDoJacquin.Applications.DTOs
{
    public class ProdutoResponseDTO
    {
        public Guid IdProduto { get; set; }
        public string NomeProduto { get; set; }
        public decimal Preco { get; set; }
        public string? DescricaoCurta { get; set; }
        public string? DescricaoLonga { get; set; }
        public bool Disponibilidade { get; set; }
        public string? ImagemUrl { get; set; }
        public string NomeCategoria { get; set; }
        public double NotaMedia { get; set; }
        public int TotalAvaliacoes { get; set; }
    }
}
