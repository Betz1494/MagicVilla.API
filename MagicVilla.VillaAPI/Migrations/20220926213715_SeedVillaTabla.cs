using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla.VillaAPI.Migrations
{
    public partial class SeedVillaTabla : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villa",
                columns: new[] { "Id", "Capacidad", "Comodidad", "Descripcion", "FechaCreacion", "FechaModificada", "ImagenUrl", "Nombre", "Precio", "Ubicacion" },
                values: new object[] { 1, 5, "", "Combina el aspecto hogareño y acogedor de una cabaña con los lujos más extravagantes que puedas imaginar.", new DateTime(2022, 9, 26, 16, 37, 15, 268, DateTimeKind.Local).AddTicks(1242), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://www.mexicodesconocido.com.mx/wp-content/uploads/2019/08/cabanas-jalisco-portada.jpg", "Villa Imperial", 2500.0, "Puebla, Puebla." });

            migrationBuilder.InsertData(
                table: "Villa",
                columns: new[] { "Id", "Capacidad", "Comodidad", "Descripcion", "FechaCreacion", "FechaModificada", "ImagenUrl", "Nombre", "Precio", "Ubicacion" },
                values: new object[] { 2, 6, "", "Hermosa Cabaña con vista a un lago espectacular, ideal para desconectarte y descansar.", new DateTime(2022, 9, 26, 16, 37, 15, 268, DateTimeKind.Local).AddTicks(1275), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://q-xx.bstatic.com/xdata/images/hotel/840x460/297598963.jpg?k=c1e6c336fa88d15beb052eeb41a81065557876f820c8e21b7a328d14e00085fa&o=", "Villa Premium", 3000.0, "Atlixco, Puebla." });

            migrationBuilder.InsertData(
                table: "Villa",
                columns: new[] { "Id", "Capacidad", "Comodidad", "Descripcion", "FechaCreacion", "FechaModificada", "ImagenUrl", "Nombre", "Precio", "Ubicacion" },
                values: new object[] { 3, 6, "", "Hermosa Cabaña con alberca privada, tinas de hidromasaje para relajarte después de un día de playa, cómoda terraza para cenar en familia frente al mar, y todo con una vista privilegiada.", new DateTime(2022, 9, 26, 16, 37, 15, 268, DateTimeKind.Local).AddTicks(1278), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://www.dondeir.com/wp-content/uploads/2017/01/cabanas-sobre-el-agua-a.jpg", "Villa VIP", 4500.0, "Veracruz, Mexico." });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villa",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villa",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Villa",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
