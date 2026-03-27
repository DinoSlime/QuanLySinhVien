using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLiSinhVien.Migrations
{
    /// <inheritdoc />
    public partial class AddTrangThaiAndEmailToSinhVien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "SinhViens",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "SinhViens",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "SinhViens");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "SinhViens");
        }
    }
}
