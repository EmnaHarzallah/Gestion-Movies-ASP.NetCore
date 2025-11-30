using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspCoreFirstApp.Migrations
{
    /// <inheritdoc />
    public partial class AddMovieImageAndDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateAjoutMovie",
                table: "Movies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFile",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAjoutMovie",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ImageFile",
                table: "Movies");
        }
    }
}
