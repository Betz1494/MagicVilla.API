using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla.VillaAPI.Models
{
    public class Villa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Ubicacion { get; set; }
        public int Capacidad { get; set; }
        public double Precio { get; set; }
        public string Comodidad { get; set; }
        public string ImagenUrl { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificada { get; set; }
    }
}
