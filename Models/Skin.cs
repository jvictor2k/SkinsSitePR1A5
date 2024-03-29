﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

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

        [Display(Name = "Descrição da skin")]
        public string DescricaoCurta { get; set; }

        [Required(ErrorMessage = "A descrição da skin deve ser informada")]
        [Display(Name = "Descrição Detalhada da skin")]
        [MinLength(20, ErrorMessage = "A descrição deve ter no mínimo {1} caracteres")]
        [MaxLength(300, ErrorMessage = "A descrição pode ter no máximo {1} caracteres")]
        public string DescricaoDetalhada { get; set; }

        [Required(ErrorMessage = "O preço da skin deve ser informado")]
        [Display(Name = "Preço")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }

        [NotMapped]
        public IFormFile Imagem { get; set; }

        public string ImagemNome { get; set; }

        [Display(Name = "Preferida?")]
        public bool IsSkinPreferida { get; set; }

        [Display(Name = "Estoque")]
        public int EmEstoque { get; set; }
        public bool Multiplas { get; set; }
        public int CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }
    }
}
