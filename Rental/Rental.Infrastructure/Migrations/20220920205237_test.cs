using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Rental.Infrastructure.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Dictionaries",
                columns: new[] { "Name", "Value", "UpdatedAt", "CreatedAt" },
                values: new object[] { "RegisterEmail", "Just now you created new account in our application. Thanks", DateTime.UtcNow, DateTime.UtcNow });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
