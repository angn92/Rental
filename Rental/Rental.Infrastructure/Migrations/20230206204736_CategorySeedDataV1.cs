using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental.Infrastructure.Migrations
{
    public partial class CategorySeedDataV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "SessionId",
            //    table: "Sessions",
            //    newName: "SessionIdentifier");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { "5f25a717-8daf-48eb-ae76-de6b529fb26d", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sprzęt budowlany", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "8d0d5a28-db66-41c4-a55f-d919d8bca506", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sport i rekreacja", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "ed976ad2-154f-4d59-9829-b1a7c3856231", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sprzęt elektroniczny", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "5f25a717-8daf-48eb-ae76-de6b529fb26d");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "8d0d5a28-db66-41c4-a55f-d919d8bca506");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "ed976ad2-154f-4d59-9829-b1a7c3856231");

            migrationBuilder.RenameColumn(
                name: "SessionIdentifier",
                table: "Sessions",
                newName: "SessionId");
        }
    }
}
