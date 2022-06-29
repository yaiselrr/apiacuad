using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AddRoleAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES ('812e6d6b-0fd3-4285-9936-fe300a43ff04', '15a5e761-a0f4-46fc-bad3-fee26745342a')");
            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES ('adc23d8a-f3c0-4542-a7c6-5d204de9cdc4', 'f9567dd7-d14d-45d2-b294-629779c0f549')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetUserRoles] WHERE UserId = '812e6d6b-0fd3-4285-9936-fe300a43ff04'");
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetUserRoles] WHERE UserId = '812e6d6b-0fd3-4285-9936-fe300a43ff04'");
        }
    }
}
