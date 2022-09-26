using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Rental.Infrastructure.Migrations
{
    public partial class UpdateDictionariesParameter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Dictionaries",
                keyColumn: "Name",
                keyValue: "RegisterEmail",
                column: "Name",
                value: "You created new account. This is your activation code {code}");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
