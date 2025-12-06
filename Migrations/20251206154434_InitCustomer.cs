using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspCoreFirstApp.Migrations
{
    /// <inheritdoc />
    public partial class InitCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountRate",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MembershipName",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountRate",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "MembershipName",
                table: "Customers");
        }
    }
}
