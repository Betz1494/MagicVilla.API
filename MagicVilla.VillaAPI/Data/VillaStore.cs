using MagicVilla.VillaAPI.VillaAPI.Models.Dto;

namespace MagicVilla.VillaAPI.Data
{
    public static  class VillaStore
    {
        public static List<VillaUpdateDTO> GetVillas =
            new List<VillaUpdateDTO>
            {
                new VillaUpdateDTO { Id = 1, Nombre = "Villa con Alberca", Capacidad = 5, Precio = 1500 },
                new VillaUpdateDTO { Id = 2, Nombre = "Villa en la playa", Capacidad = 3, Precio = 1000}
            };
    }
}
