using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kakeibo.Migrations
{
    /// <inheritdoc />
    public partial class changeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIncome",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "MonthlySummaries",
                newName: "Shuusi");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "MonthlySummaries",
                type: "nvarchar(256)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsIncome",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_transactions_CategoryId",
                table: "Transactions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlySummaries_UserId",
                table: "MonthlySummaries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlySummaries_Users_UserId",
                table: "MonthlySummaries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_Categories_CategoryId",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthlySummaries_Users_UserId",
                table: "MonthlySummaries");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_Categories_CategoryId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_CategoryId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_MonthlySummaries_UserId",
                table: "MonthlySummaries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MonthlySummaries");

            migrationBuilder.DropColumn(
                name: "IsIncome",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Users",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "Shuusi",
                table: "MonthlySummaries",
                newName: "Total");

            migrationBuilder.AddColumn<bool>(
                name: "IsIncome",
                table: "Transactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
