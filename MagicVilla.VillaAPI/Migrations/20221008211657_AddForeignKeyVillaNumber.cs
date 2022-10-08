using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla.VillaAPI.Migrations
{
    public partial class AddForeignKeyVillaNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VillaId",
                table: "VillaNumber",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Villa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2022, 10, 8, 16, 16, 57, 259, DateTimeKind.Local).AddTicks(4732));

            migrationBuilder.UpdateData(
                table: "Villa",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2022, 10, 8, 16, 16, 57, 259, DateTimeKind.Local).AddTicks(4787));

            migrationBuilder.UpdateData(
                table: "Villa",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2022, 10, 8, 16, 16, 57, 259, DateTimeKind.Local).AddTicks(4792));

            migrationBuilder.CreateIndex(
                name: "IX_VillaNumber_VillaId",
                table: "VillaNumber",
                column: "VillaId");

            migrationBuilder.AddForeignKey(
                name: "FK_VillaNumber_Villa_VillaId",
                table: "VillaNumber",
                column: "VillaId",
                principalTable: "Villa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VillaNumber_Villa_VillaId",
                table: "VillaNumber");

            migrationBuilder.DropIndex(
                name: "IX_VillaNumber_VillaId",
                table: "VillaNumber");

            migrationBuilder.DropColumn(
                name: "VillaId",
                table: "VillaNumber");

            migrationBuilder.UpdateData(
                table: "Villa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2022, 10, 6, 14, 57, 15, 396, DateTimeKind.Local).AddTicks(2339));

            migrationBuilder.UpdateData(
                table: "Villa",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2022, 10, 6, 14, 57, 15, 396, DateTimeKind.Local).AddTicks(2372));

            migrationBuilder.UpdateData(
                table: "Villa",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2022, 10, 6, 14, 57, 15, 396, DateTimeKind.Local).AddTicks(2375));
        }
    }
}
