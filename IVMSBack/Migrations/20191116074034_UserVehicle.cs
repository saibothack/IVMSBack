using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IVMSBack.Migrations
{
    public partial class UserVehicle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VehicleStatusID",
                table: "VehicleStatusStore",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VehicleID",
                table: "VehicleStatusStore",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VehicleID",
                table: "VehicleLines",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LineID",
                table: "VehicleLines",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LineID",
                table: "IVMSBackUserLines",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "IVMSBackUserVehicles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserCreate = table.Column<string>(nullable: true),
                    DateCreate = table.Column<DateTime>(nullable: true),
                    UserModified = table.Column<string>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    UserEnd = table.Column<string>(nullable: true),
                    DateEnd = table.Column<DateTime>(nullable: true),
                    IVMSBackUserID = table.Column<string>(nullable: true),
                    VehicleID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IVMSBackUserVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IVMSBackUserVehicles_AspNetUsers_IVMSBackUserID",
                        column: x => x.IVMSBackUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IVMSBackUserVehicles_Vehicle_VehicleID",
                        column: x => x.VehicleID,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleStatusStore_VehicleID",
                table: "VehicleStatusStore",
                column: "VehicleID");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleStatusStore_VehicleStatusID",
                table: "VehicleStatusStore",
                column: "VehicleStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLines_LineID",
                table: "VehicleLines",
                column: "LineID");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLines_VehicleID",
                table: "VehicleLines",
                column: "VehicleID");

            migrationBuilder.CreateIndex(
                name: "IX_IVMSBackUserVehicles_IVMSBackUserID",
                table: "IVMSBackUserVehicles",
                column: "IVMSBackUserID");

            migrationBuilder.CreateIndex(
                name: "IX_IVMSBackUserVehicles_VehicleID",
                table: "IVMSBackUserVehicles",
                column: "VehicleID");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleLines_Line_LineID",
                table: "VehicleLines",
                column: "LineID",
                principalTable: "Line",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleLines_Vehicle_VehicleID",
                table: "VehicleLines",
                column: "VehicleID",
                principalTable: "Vehicle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleStatusStore_Vehicle_VehicleID",
                table: "VehicleStatusStore",
                column: "VehicleID",
                principalTable: "Vehicle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleStatusStore_VehicleStatus_VehicleStatusID",
                table: "VehicleStatusStore",
                column: "VehicleStatusID",
                principalTable: "VehicleStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleLines_Line_LineID",
                table: "VehicleLines");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleLines_Vehicle_VehicleID",
                table: "VehicleLines");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleStatusStore_Vehicle_VehicleID",
                table: "VehicleStatusStore");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleStatusStore_VehicleStatus_VehicleStatusID",
                table: "VehicleStatusStore");

            migrationBuilder.DropTable(
                name: "IVMSBackUserVehicles");

            migrationBuilder.DropIndex(
                name: "IX_VehicleStatusStore_VehicleID",
                table: "VehicleStatusStore");

            migrationBuilder.DropIndex(
                name: "IX_VehicleStatusStore_VehicleStatusID",
                table: "VehicleStatusStore");

            migrationBuilder.DropIndex(
                name: "IX_VehicleLines_LineID",
                table: "VehicleLines");

            migrationBuilder.DropIndex(
                name: "IX_VehicleLines_VehicleID",
                table: "VehicleLines");

            migrationBuilder.AlterColumn<string>(
                name: "VehicleStatusID",
                table: "VehicleStatusStore",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "VehicleID",
                table: "VehicleStatusStore",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "VehicleID",
                table: "VehicleLines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "LineID",
                table: "VehicleLines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "LineID",
                table: "IVMSBackUserLines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
