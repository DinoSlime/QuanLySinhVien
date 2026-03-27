using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLiSinhVien.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrationDatesToLHP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NgayBatDauDangKy",
                table: "LopHocPhans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayKetThucDangKy",
                table: "LopHocPhans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayBatDauDangKy",
                table: "LopHocPhans");

            migrationBuilder.DropColumn(
                name: "NgayKetThucDangKy",
                table: "LopHocPhans");
        }
    }
}
