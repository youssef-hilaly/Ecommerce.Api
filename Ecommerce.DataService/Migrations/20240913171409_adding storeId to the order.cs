using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ecommerce.DataService.Migrations
{
    /// <inheritdoc />
    public partial class addingstoreIdtotheorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0bea7696-0a2a-496f-a527-379c4adaedae");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4abb86f0-d020-41f5-9999-0bad33ee8068");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c20eb90-b982-4aea-bedc-17c60e29e49b");

            migrationBuilder.AddColumn<Guid>(
                name: "StoreId",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2956a90b-2389-4055-bd41-f890f5b3ce48", null, "SuperAdmin", "SUPERADMIN" },
                    { "84d7e75e-77c8-4892-b289-8619a157a0fd", null, "StoreOwner", "STOREOWNER" },
                    { "ee54eb7c-3976-47d2-bcad-fdf736237eb0", null, "Customer", "CUSTOMER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StoreId",
                table: "Orders",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Stores_StoreId",
                table: "Orders",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Stores_StoreId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_StoreId",
                table: "Orders");

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

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0bea7696-0a2a-496f-a527-379c4adaedae", null, "StoreOwner", "STOREOWNER" },
                    { "4abb86f0-d020-41f5-9999-0bad33ee8068", null, "SuperAdmin", "SUPERADMIN" },
                    { "9c20eb90-b982-4aea-bedc-17c60e29e49b", null, "Customer", "CUSTOMER" }
                });
        }
    }
}
