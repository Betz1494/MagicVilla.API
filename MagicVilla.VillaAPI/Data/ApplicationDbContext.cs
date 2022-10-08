using MagicVilla.VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.VillaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Villa> Villa { get; set; }
        public DbSet<VillaNumber> VillaNumber { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Nombre = "Villa Imperial",
                    Descripcion = "Combina el aspecto hogareño y acogedor de una cabaña con los lujos más extravagantes que puedas imaginar.",
                    Ubicacion = "Puebla, Puebla.",
                    Capacidad = 5,
                    Precio = 2500,
                    Comodidad = "",
                    ImagenUrl = "https://www.mexicodesconocido.com.mx/wp-content/uploads/2019/08/cabanas-jalisco-portada.jpg",
                    FechaCreacion = DateTime.Now
                },
                new Villa()
                {
                    Id = 2,
                    Nombre = "Villa Premium",
                    Descripcion = "Hermosa Cabaña con vista a un lago espectacular, ideal para desconectarte y descansar.",
                    Ubicacion = "Atlixco, Puebla.",
                    Capacidad = 6,
                    Precio = 3000,
                    Comodidad = "",
                    ImagenUrl = "https://q-xx.bstatic.com/xdata/images/hotel/840x460/297598963.jpg?k=c1e6c336fa88d15beb052eeb41a81065557876f820c8e21b7a328d14e00085fa&o=",
                    FechaCreacion = DateTime.Now
                },
                new Villa()
                {
                    Id = 3,
                    Nombre = "Villa VIP",
                    Descripcion = "Hermosa Cabaña con alberca privada, tinas de hidromasaje para relajarte después de un día de playa, cómoda terraza para cenar en familia frente al mar, y todo con una vista privilegiada.",
                    Ubicacion = "Veracruz, Mexico.",
                    Capacidad = 6,
                    Precio = 4500,
                    Comodidad = "",
                    ImagenUrl = "https://www.dondeir.com/wp-content/uploads/2017/01/cabanas-sobre-el-agua-a.jpg",
                    FechaCreacion = DateTime.Now
                }
                );
        }
    }
}
