using System.ComponentModel.DataAnnotations;

namespace MagicVilla.VillaAPI.VillaAPI.Models.Dto
{
    public class VillaDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Nombre { get; set; }

        public int Capacidad { get; set; }

        public double Precio { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}
