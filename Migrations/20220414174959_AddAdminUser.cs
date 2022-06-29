using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AddAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUsers] ([Id], [Names], [FirstLastName], [SecondLastName], [Title], [MobileNumber], [Sex], [Address], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsActive]) VALUES (N'812e6d6b-0fd3-4285-9936-fe300a43ff04', NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'admin@simas.torreon.com', N'ADMIN@SIMAS.TORREON.COM', N'admin@simas.torreon.com', N'ADMIN@SIMAS.TORREON.COM', 1, N'AQAAAAEAACcQAAAAEI3YYAWl3hab6pdKCAF5MbLY302ul1VXX5SZyHTFzH/PpsYlfhiA+6ykGkG/Wc3PPQ==', N'ADD4I5PSUNKPAYAYB2ZIF5NWVPSJVN4O', N'44253ff2-f00a-4299-ac1c-f3d18fd64e53', NULL, 0, 0, NULL, 1, 0, 1)");

            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUsers] ([Id], [Names], [FirstLastName], [SecondLastName], [Title], [MobileNumber], [Sex], [Address], [IsActive], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'adc23d8a-f3c0-4542-a7c6-5d204de9cdc4', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, N'text@gmail.com', N'TEXT@GMAIL.COM', N'text@gmail.com', N'TEXT@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEHRT7kW2J3Vm97xyK5PU5SmNWz03UoykIZVKNRqMaE7b8Fbiyp/ap/DZUX/J4OPc0Q==', N'D3B3TWKKGZOP5Q7THQJWY4U26OV7LVMK', N'23f3f531-c9b1-44a2-aea3-98987dd2ff57', NULL, 0, 0, NULL, 1, 0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetUsers] WHERE Id = '812e6d6b-0fd3-4285-9936-fe300a43ff04'");
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetUsers] WHERE Id = 'adc23d8a-f3c0-4542-a7c6-5d204de9cdc4'");
        }
    }
}
