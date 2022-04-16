using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace zixieWebAPI_Stocks.Migrations
{
    public partial class new_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ticker = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DivYieldFlag = table.Column<bool>(type: "bit", nullable: true),
                    Exchange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Figi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Isin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuyAvailableFlag = table.Column<bool>(type: "bit", nullable: true),
                    Nominal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sector = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shares", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shares");
        }
    }
}
