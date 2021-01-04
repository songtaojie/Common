using Microsoft.EntityFrameworkCore.Migrations;

namespace Hx.Sdk.NetCore.Test.Migrations
{
    public partial class InitTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Active = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    Deleted = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Blog",
                columns: new[] { "Id", "Active", "Deleted", "IsDeleted" },
                values: new object[] { "5ea1c35c-ac65-45cf-8c39-7cf7db8a11cd", 12m, "Y", "Y" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blog");
        }
    }
}
