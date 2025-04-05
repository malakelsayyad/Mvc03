using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Company.G02.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addingUserAndAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "C4D17A15-B5A6-41BD-9B6E-B10F86CB48EE", null, "Admin", "ADMIN" },
                    { "E11E65D8-18C4-493F-AF19-4A3B80F8AC01", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsAgree", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "4EA19E03-C84D-453C-A6B2-CF3A9981F594", 0, "bc2e710e-766c-4054-9e69-9b4cb6c8be46", "malakelsayyad@gmail.com", false, "malak", false, "elsayyad", false, null, null, null, "AQAAAAIAAYagAAAAEJC2O18ue5iizJSCG18MKdJMBf1b2cGgFrYrJC0QVLYATSg4xvQUCqxHSZCqIPeMiQ==", null, false, "b10f35f2-d117-4dd1-8757-102e8af66a3b", false, null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "C4D17A15-B5A6-41BD-9B6E-B10F86CB48EE", "4EA19E03-C84D-453C-A6B2-CF3A9981F594" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "E11E65D8-18C4-493F-AF19-4A3B80F8AC01");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "C4D17A15-B5A6-41BD-9B6E-B10F86CB48EE", "4EA19E03-C84D-453C-A6B2-CF3A9981F594" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "C4D17A15-B5A6-41BD-9B6E-B10F86CB48EE");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4EA19E03-C84D-453C-A6B2-CF3A9981F594");
        }
    }
}
