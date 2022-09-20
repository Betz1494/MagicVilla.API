using MagicVilla.VillaAPI.VillaAPI.Models.Dto;

namespace MagicVilla.VillaAPI.Data
{
    public static  class VillaStore
    {
        public static List<VillaDTO> GetVillas =
            new List<VillaDTO>
            {
                new VillaDTO { Id = 1, Nombre = "Villa con Alberca", Capacidad = 5, Precio = 1500 },
                new VillaDTO { Id = 2, Nombre = "Villa en la playa", Capacidad = 3, Precio = 1000}
            };
    }
}
