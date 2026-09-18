using System;

namespace BolosDoJacquin.Applications.DTOs
{
    public class CriarAvaliacaoDTO
    {
        public byte Nota { get; set; }
        public string? Comentario { get; set; }
        public Guid IdProduto { get; set; }
    }
}
