using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class SolicitudsInmuebles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AspNetUsers",
                type: "VARCHAR(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Sex",
                table: "AspNetUsers",
                type: "VARCHAR(9)",
                maxLength: 9,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(8)",
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PropertiesRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailOwner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertiesRequest", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertiesRequest");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AspNetUsers",
                type: "VARCHAR(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Sex",
                table: "AspNetUsers",
                type: "VARCHAR(8)",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(9)",
                oldMaxLength: 9,
                oldNullable: true);
        }
    }
}
