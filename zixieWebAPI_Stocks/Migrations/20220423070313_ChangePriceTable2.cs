using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace zixieWebAPI_Stocks.Migrations
{
    public partial class ChangePriceTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Isin",
                table: "Prices",
                newName: "Figi");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Figi",
                table: "Prices",
                newName: "Isin");
        }
    }
}
