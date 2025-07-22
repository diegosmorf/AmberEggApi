using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace AmberEggApi.Database.Migrations;

public partial class CreatePersona : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Persona",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                CreateDate = table.Column<DateTime>(nullable: false),
                ModifiedDate = table.Column<DateTime>(nullable: true),
                Version = table.Column<int>(nullable: false),
                Name = table.Column<string>(maxLength: 20, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Persona", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Persona");
    }
}
