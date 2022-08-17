using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkinsSite.Models
{
    [Table("Skins")]
    public class Skin
    {
        [Key]
        public int SkinId { get; set; }

        [Required(ErrorMessage = "O tipo de skin deve ser informado")]
        [Display(Name = "Tipo de skin")]
        [StringLength(80, ErrorMessage = "O tamanho máximo é de 80 caracteres")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "O nome da skin deve ser informado")]
        [Display(Name = "Nome da skin")]
        [StringLength(80, MinimumLength = 5, ErrorMessage = "O {0} deve ter no mínimo {1} e no máximo {2}")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O estado da skin deve ser informado")]
        [Display(Name = "Estado da skin")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "O {0} deve ter no mínimo {1} e no máximo {2}")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "O float da skin deve ser informado")]
        [Display(Name = "Float da skin")]
        [Range(0,99999999999999, ErrorMessage ="O float deve estar entre 0.00000000000000 e 0.99999999999999")]
        public long SkinFloat { get; set; }

        [Required(ErrorMessage = "A descrição da skin deve ser informada")]
        [Display(Name = "Descrição da skin")]
        [MinLength(20, ErrorMessage = "A descrição deve ter no mínimo {1} caracteres")]
        [MaxLength(200, ErrorMessage = "A descrição pode ter no máximo {1} caracteres")]
        public string DescricaoCurta { get; set; }

        [Required(ErrorMessage = "A descrição da skin deve ser informada")]
        [Display(Name = "Descrição Detalhada da skin")]
        [MinLength(20, ErrorMessage = "A descrição deve ter no mínimo {1} caracteres")]
        [MaxLength(300, ErrorMessage = "A descrição pode ter no máximo {1} caracteres")]
        public string DescricaoDetalhada { get; set; }

        [Required(ErrorMessage = "O preço da skin deve ser informado")]
        [Display(Name = "Preço")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(1, 9999.99, ErrorMessage = "O preço deve estar entre 1.00 e 10000.00")]
        public decimal Preco { get; set; }

        [Display(Name = "Caminho Imagem Normal")]
        [StringLength(200, ErrorMessage = "O {0} deve ter no máximo {1}")]
        public string ImagemUrl { get; set; }

        [Display(Name = "Caminho Imagem Miniatura")]
        [StringLength(200, ErrorMessage = "O {0} deve ter no máximo {1}")]
        public string ImagemThumbnailUrl { get; set; }

        [Display(Name = "Preferida?")]
        public bool IsSkinPreferida { get; set; }

        [Display(Name = "Estoque")]
        public bool EmEstoque { get; set; }

        public int CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }
    }
}
