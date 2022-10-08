using System.ComponentModel.DataAnnotations;

namespace MagicVilla.VillaAPI.Models.Dto
{
    public class VillaNumberUpdateDTO
    {
        [Required]
        public int VillaNo { get; set; }
        public string Detalles { get; set; }
    }
}
