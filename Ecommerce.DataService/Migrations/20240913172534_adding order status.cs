using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ecommerce.DataService.Migrations
{
    /// <inheritdoc />
    public partial class addingorderstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2956a90b-2389-4055-bd41-f890f5b3ce48");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "84d7e75e-77c8-4892-b289-8619a157a0fd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee54eb7c-3976-47d2-bcad-fdf736237eb0");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "09fb8f77-4a3c-4edf-9d21-8e17b7bf1314", null, "Customer", "CUSTOMER" },
                    { "54197490-a9c9-4264-a767-a6e92e84223b", null, "StoreOwner", "STOREOWNER" },
                    { "69efdc2d-9851-4855-b86c-cf72fe71d719", null, "SuperAdmin", "SUPERADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "09fb8f77-4a3c-4edf-9d21-8e17b7bf1314");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "54197490-a9c9-4264-a767-a6e92e84223b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69efdc2d-9851-4855-b86c-cf72fe71d719");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2956a90b-2389-4055-bd41-f890f5b3ce48", null, "SuperAdmin", "SUPERADMIN" },
                    { "84d7e75e-77c8-4892-b289-8619a157a0fd", null, "StoreOwner", "STOREOWNER" },
                    { "ee54eb7c-3976-47d2-bcad-fdf736237eb0", null, "Customer", "CUSTOMER" }
                });
        }
    }
}
