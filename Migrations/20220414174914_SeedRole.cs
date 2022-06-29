using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace API.Migrations
{
    public partial class SeedRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { "f9567dd7-d14d-45d2-b294-629779c0f549", "Customer", "Customer".ToUpper(), Guid.NewGuid().ToString() }
                );
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { "15a5e761-a0f4-46fc-bad3-fee26745342a", "Admin", "Admin".ToUpper(), Guid.NewGuid().ToString() }
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[AspNetRoles]");
        }
    }
}
