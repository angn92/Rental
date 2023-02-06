using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental.Infrastructure.Migrations
{
    public partial class NewCategoryDataV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { "25b2e301-5e45-48f6-8a13-9b7ccb04a428", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sprzęt elektroniczny", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "7281b68a-5357-45d7-8e00-f93298186098", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sprzęt budowlany", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "91c2c7ef-5c77-485b-8b48-9d7a661ea857", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sprzęt domowy", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "e570e177-56e0-4784-a540-ac465f6748ad", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sport i rekreacja", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "25b2e301-5e45-48f6-8a13-9b7ccb04a428");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "7281b68a-5357-45d7-8e00-f93298186098");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "91c2c7ef-5c77-485b-8b48-9d7a661ea857");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: "e570e177-56e0-4784-a540-ac465f6748ad");

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
    }
}
