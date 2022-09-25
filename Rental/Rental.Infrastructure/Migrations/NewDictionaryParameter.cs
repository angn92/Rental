using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Migrations
{
    public partial class NewDictionaryParameter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NewPasswordd",
                table: "Passwords",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
        //protected override void Up(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.InsertData(
        //        table: "Dictionaries",
        //        columns: new[] { "Id", "Name", "Value", "UpdatedAt", "CreatedAt" },
        //        values: new object[] { 1, "RegisterEmail", "Just now you created new account in our application. Thanks", DateTime.Now, DateTime.Now });
        //}

        //protected override void Up(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.CreateTable(
        //        name: "ActionHistories",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "integer", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            Name = table.Column<string>(type: "text", nullable: true),
        //            UpgradeVersion = table.Column<int>(type: "integer", nullable: false),
        //            UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
        //            CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_ActionHistories", x => x.Id);
        //        });
        //}

        //protected override void Down(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.DropTable(
        //        name: "ActionHistories");
        //}
    }
}
