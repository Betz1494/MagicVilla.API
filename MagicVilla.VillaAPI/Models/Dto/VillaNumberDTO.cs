using System.ComponentModel.DataAnnotations;

namespace MagicVilla.VillaAPI.Models.Dto
{

    public class VillaNumberDTO
    {
        [Required]
        public int VillaNo { get; set; }
        public string Detalles { get; set; }
    }
}
