using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AspCoreFirstApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-4a11-98ef-2345abcd1111"), "Action" },
                    { new Guid("aa789fff-4444-4e33-ccdd-998877665544"), "Romance" },
                    { new Guid("b234abcd-1234-46ff-8890-888899990000"), "Science-Fiction" },
                    { new Guid("bb90aaaa-5555-4f44-ddaa-aabbccddeeff"), "Thriller" },
                    { new Guid("c345bbbb-5678-45aa-9900-abcd12340000"), "Drame" },
                    { new Guid("cc01bbbb-6666-4a55-eeff-223344556677"), "Comédie-Dramatique" },
                    { new Guid("d456cccc-1111-42bb-8899-aa00ff223344"), "Crime" },
                    { new Guid("dd12cccc-7777-4b66-ff00-334455667788"), "Animation" },
                    { new Guid("e567dddd-2222-4c11-aabb-112233445566"), "Super-Héros" },
                    { new Guid("ee23dddd-8888-4c77-aa11-445566778899"), "Horreur" },
                    { new Guid("f678eeee-3333-4d22-bbcc-778899001122"), "Historique" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "DateAjoutMovie", "GenreId", "ImageFile", "Name" },
                values: new object[] { 1, new DateTime(2024, 1, 15, 14, 20, 0, 0, DateTimeKind.Unspecified), new Guid("a1b2c3d4-e5f6-4a11-98ef-2345abcd1111"), "dark_knight.jpg", "The Dark Knight" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("aa789fff-4444-4e33-ccdd-998877665544"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("b234abcd-1234-46ff-8890-888899990000"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("bb90aaaa-5555-4f44-ddaa-aabbccddeeff"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("c345bbbb-5678-45aa-9900-abcd12340000"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("cc01bbbb-6666-4a55-eeff-223344556677"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("d456cccc-1111-42bb-8899-aa00ff223344"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("dd12cccc-7777-4b66-ff00-334455667788"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("e567dddd-2222-4c11-aabb-112233445566"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("ee23dddd-8888-4c77-aa11-445566778899"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("f678eeee-3333-4d22-bbcc-778899001122"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-4a11-98ef-2345abcd1111"));
        }
    }
}
