using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental.Infrastructure.Migrations
{
    public partial class UpdateDictionariesParameter2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Dictionaries",
                keyColumn: "Name",
                keyValue: "You created new account. This is your activation code {code}",
                column: "Name",
                value: "RegisterEmail");

            migrationBuilder.UpdateData(
                table: "Dictionaries",
                keyColumn: "Name",
                keyValue: "RegisterEmail",
                column: "Value",
                value: "You created new account. This is your activation code {code}");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
