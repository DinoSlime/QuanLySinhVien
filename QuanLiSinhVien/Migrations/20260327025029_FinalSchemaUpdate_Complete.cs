using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLiSinhVien.Migrations
{
    /// <inheritdoc />
    public partial class FinalSchemaUpdate_Complete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoaiMon",
                table: "MonHocs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoTietLyThuyet",
                table: "MonHocs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoTietThucHanh",
                table: "MonHocs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhongHoc",
                table: "LopHocPhans",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Thu",
                table: "LopHocPhans",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TietBatDau",
                table: "LopHocPhans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "KetQuaHocTaps",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoaiMon",
                table: "MonHocs");

            migrationBuilder.DropColumn(
                name: "SoTietLyThuyet",
                table: "MonHocs");

            migrationBuilder.DropColumn(
                name: "SoTietThucHanh",
                table: "MonHocs");

            migrationBuilder.DropColumn(
                name: "PhongHoc",
                table: "LopHocPhans");

            migrationBuilder.DropColumn(
                name: "Thu",
                table: "LopHocPhans");

            migrationBuilder.DropColumn(
                name: "TietBatDau",
                table: "LopHocPhans");

            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "KetQuaHocTaps");
        }
    }
}
