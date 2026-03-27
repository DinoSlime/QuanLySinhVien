using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLiSinhVien.Migrations
{
    /// <inheritdoc />
    public partial class TaoBangMonTienQuyet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MonTienQuyet",
                table: "MonTienQuyet");

            migrationBuilder.RenameTable(
                name: "MonTienQuyet",
                newName: "MonTienQuyets");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonTienQuyets",
                table: "MonTienQuyets",
                columns: new[] { "MaMH", "MaMHTQ" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MonTienQuyets",
                table: "MonTienQuyets");

            migrationBuilder.RenameTable(
                name: "MonTienQuyets",
                newName: "MonTienQuyet");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonTienQuyet",
                table: "MonTienQuyet",
                columns: new[] { "MaMH", "MaMHTQ" });
        }
    }
}
