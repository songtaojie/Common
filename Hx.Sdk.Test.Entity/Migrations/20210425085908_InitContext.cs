using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hx.Sdk.Test.Entity.Migrations
{
    public partial class InitContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreaterId = table.Column<string>(maxLength: 36, nullable: true),
                    Creater = table.Column<string>(maxLength: 36, nullable: true),
                    LastModifyTime = table.Column<DateTime>(nullable: true),
                    LastModifier = table.Column<string>(maxLength: 36, nullable: true),
                    LastModifierId = table.Column<string>(maxLength: 36, nullable: true),
                    UserName = table.Column<string>(maxLength: 36, nullable: false),
                    PassWord = table.Column<string>(maxLength: 36, nullable: false),
                    NickName = table.Column<string>(maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "UserInfo",
                columns: new[] { "Id", "CreateTime", "Creater", "CreaterId", "LastModifier", "LastModifierId", "LastModifyTime", "NickName", "PassWord", "UserName" },
                values: new object[] { "f1f69f0d-c546-470a-ba52-8a35b306626e", new DateTime(2021, 4, 25, 16, 59, 8, 313, DateTimeKind.Local).AddTicks(8455), "SuperAdmin", "SuperAdmin", "", "", null, "宋", "123456", "songtaojie" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInfo");
        }
    }
}
