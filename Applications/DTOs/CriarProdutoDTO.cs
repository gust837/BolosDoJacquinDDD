namespace BolosDoJacquin.Applications.DTOs
{
    public class CriarProdutoDTO
    {
        public string NomeProduto { get; set; }
        public decimal Preco { get; set; }
        public string DescricaoCurta { get; set; }
        public string DescricaoLonga { get; set; }
        public Guid? IdCategoria { get; set; }
        public IFormFile? ArquivoDeImagem { get; set; }
    }
}
