using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace UtilizeJwtProvider.Migrations
{
    public partial class InitialUserStoreMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DebtorId = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Firstname = table.Column<string>(nullable: true),
                    Hash = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Lastname = table.Column<string>(nullable: true),
                    LoginCode = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(nullable: true),
                    TenantId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
