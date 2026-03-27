using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLiSinhVien.Migrations
{
    /// <inheritdoc />
    public partial class AddBangMonTienQuyet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonTienQuyet",
                columns: table => new
                {
                    MaMH = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaMHTQ = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonTienQuyet", x => new { x.MaMH, x.MaMHTQ });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonTienQuyet");
        }
    }
}
