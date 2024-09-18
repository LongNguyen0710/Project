using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcPhone.Migrations
{
    /// <inheritdoc />
    public partial class database10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Bill_BillId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_BillId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "BillId",
                table: "Product");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Bill",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bill_ProductId",
                table: "Bill",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_Product_ProductId",
                table: "Bill",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Product_ProductId",
                table: "Bill");

            migrationBuilder.DropIndex(
                name: "IX_Bill_ProductId",
                table: "Bill");

            migrationBuilder.AddColumn<int>(
                name: "BillId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "Bill",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_BillId",
                table: "Product",
                column: "BillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Bill_BillId",
                table: "Product",
                column: "BillId",
                principalTable: "Bill",
                principalColumn: "Id");
        }
    }
}
