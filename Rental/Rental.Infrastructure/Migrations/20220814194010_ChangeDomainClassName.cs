using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental.Infrastructure.Migrations
{
    public partial class ChangeDomainClassName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "Orders");

            migrationBuilder.RenameColumn(
                name: "TransactionHash",
                table: "Orders",
                newName: "OrderHash");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "Orders",
                newName: "OrderId");

            migrationBuilder.AddColumn<int>(
                name: "OrderStatus",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Transactions");

            migrationBuilder.RenameColumn(
                name: "OrderHash",
                table: "Transactions",
                newName: "TransactionHash");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Transactions",
                newName: "TransactionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "TransactionId");
        }
    }
}
