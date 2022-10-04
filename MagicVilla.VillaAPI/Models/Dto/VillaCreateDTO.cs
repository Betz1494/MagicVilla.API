using System.ComponentModel.DataAnnotations;

namespace MagicVilla.VillaAPI.Models.Dto
{
    public class VillaCreateDTO
    {
        [Required]
        [MaxLength(30)]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Ubicacion { get; set; }
        public int Capacidad { get; set; }
        public double Precio { get; set; }
        public string Comodidad { get; set; }
        public string ImagenUrl { get; set; }
    }
}
