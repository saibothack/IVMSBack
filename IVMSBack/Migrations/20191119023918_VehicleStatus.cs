using Microsoft.EntityFrameworkCore.Migrations;

namespace IVMSBack.Migrations
{
    public partial class VehicleStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "VehicleStatus",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "VehicleStatus",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "IVMSBackUserID",
                table: "IVMSBackUserLines",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IVMSBackUserLines_IVMSBackUserID",
                table: "IVMSBackUserLines",
                column: "IVMSBackUserID");

            migrationBuilder.CreateIndex(
                name: "IX_IVMSBackUserLines_LineID",
                table: "IVMSBackUserLines",
                column: "LineID");

            migrationBuilder.AddForeignKey(
                name: "FK_IVMSBackUserLines_AspNetUsers_IVMSBackUserID",
                table: "IVMSBackUserLines",
                column: "IVMSBackUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IVMSBackUserLines_Line_LineID",
                table: "IVMSBackUserLines",
                column: "LineID",
                principalTable: "Line",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IVMSBackUserLines_AspNetUsers_IVMSBackUserID",
                table: "IVMSBackUserLines");

            migrationBuilder.DropForeignKey(
                name: "FK_IVMSBackUserLines_Line_LineID",
                table: "IVMSBackUserLines");

            migrationBuilder.DropIndex(
                name: "IX_IVMSBackUserLines_IVMSBackUserID",
                table: "IVMSBackUserLines");

            migrationBuilder.DropIndex(
                name: "IX_IVMSBackUserLines_LineID",
                table: "IVMSBackUserLines");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "VehicleStatus");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "VehicleStatus");

            migrationBuilder.AlterColumn<string>(
                name: "IVMSBackUserID",
                table: "IVMSBackUserLines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
