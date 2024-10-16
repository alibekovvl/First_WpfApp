using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WpfApp1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "categories",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        Name = table.Column<string>(type: "text", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_categories", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Expenses",
            //    columns: table => new
            //    {
            //        ExpensesId = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        CategoryId = table.Column<int>(type: "integer", nullable: false),
            //        ExpenseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        Cost = table.Column<decimal>(type: "numeric", nullable: false),
            //        Coment = table.Column<string>(type: "text", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Expenses", x => x.ExpensesId);
            //        table.ForeignKey(
            //            name: "FK_Expenses_categories_CategoryId",
            //            column: x => x.CategoryId,
            //            principalTable: "categories",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CategoryId",
                table: "Expenses",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
