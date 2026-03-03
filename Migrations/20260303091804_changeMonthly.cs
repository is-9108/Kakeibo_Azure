using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kakeibo.Migrations
{
    /// <inheritdoc />
    public partial class changeMonthly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonthlySummaries",
                columns: table => new
                {
                    Month = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false),
                    Shokuhi = table.Column<int>(type: "int", nullable: false),
                    Gaishokuhi = table.Column<int>(type: "int", nullable: false),
                    Kounetsuhi = table.Column<int>(type: "int", nullable: false),
                    Tsuusinhi = table.Column<int>(type: "int", nullable: false),
                    Suidouhi = table.Column<int>(type: "int", nullable: false),
                    Koutsuhi = table.Column<int>(type: "int", nullable: false),
                    Iryouhi = table.Column<int>(type: "int", nullable: false),
                    Zeikin = table.Column<int>(type: "int", nullable: false),
                    Yachin = table.Column<int>(type: "int", nullable: false),
                    Subscription = table.Column<int>(type: "int", nullable: false),
                    Sonota = table.Column<int>(type: "int", nullable: false),
                    Kyuryo = table.Column<int>(type: "int", nullable: false),
                    RinjiShunyu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlySummaries", x => x.Month);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthlySummaries");
        }
    }
}
