using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLiSinhVien.Migrations
{
    /// <inheritdoc />
    public partial class ThemKhoaNgoaiChoLopHocPhan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LopHocPhans_MaGV",
                table: "LopHocPhans",
                column: "MaGV");

            migrationBuilder.CreateIndex(
                name: "IX_LopHocPhans_MaMH",
                table: "LopHocPhans",
                column: "MaMH");

            migrationBuilder.AddForeignKey(
                name: "FK_LopHocPhans_GiangViens_MaGV",
                table: "LopHocPhans",
                column: "MaGV",
                principalTable: "GiangViens",
                principalColumn: "MaGV",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LopHocPhans_MonHocs_MaMH",
                table: "LopHocPhans",
                column: "MaMH",
                principalTable: "MonHocs",
                principalColumn: "MaMH",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LopHocPhans_GiangViens_MaGV",
                table: "LopHocPhans");

            migrationBuilder.DropForeignKey(
                name: "FK_LopHocPhans_MonHocs_MaMH",
                table: "LopHocPhans");

            migrationBuilder.DropIndex(
                name: "IX_LopHocPhans_MaGV",
                table: "LopHocPhans");

            migrationBuilder.DropIndex(
                name: "IX_LopHocPhans_MaMH",
                table: "LopHocPhans");
        }
    }
}
