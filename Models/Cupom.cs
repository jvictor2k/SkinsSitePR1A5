using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkinsSite.Models
{
    public class Cupom
    {
        [Key]
        public int CupomId { get; set; }

        [MinLength(3, ErrorMessage = "O tamanho mínimo é de 3 caracteres")]
        [Required(ErrorMessage = "Informe o código do cupom")]
        [Display(Name = "Código")]
        public string CupomCodigo { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        [Range(1, 99.99, ErrorMessage = "O valor deve estar entre 1.00 e 100.00")]
        [Required(ErrorMessage = "Informe o valor do percentual de desconto")]
        [Display(Name = "Percentual de desconto")]
        public decimal ValorDesconto { get; set; }

        [Display(Name = "Ativo?")]
        public bool Ativo { get; set; }
        [Display(Name = "Limite de uso")]
        public int LimiteUso { get; set; }

        public int CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }
    }
}
