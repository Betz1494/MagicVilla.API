using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto
{
    public class VillaUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Nombre { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public string Ubicacion { get; set; }

        [Required]
        public int Capacidad { get; set; }

        [Required]
        public double Precio { get; set; }

        [Required]
        public string Comodidad { get; set; }

        [Required]
        public string ImagenUrl { get; set; }
    }
}
