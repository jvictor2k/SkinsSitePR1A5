using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkinsSite.Models
{
    public class UsoCupom
    {
        [Key]
        public int UsoCupomId { get; set; }

        public string UserId { get; set; }
        public int CupomId { get; set; }

        [ForeignKey("CupomId")]
        public Cupom Cupom { get; set; }
    }
}
