using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Migrations
{
    public partial class InitialCreatetaget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "targetcustomerss",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_targetcustomerss", x => x.Id);
                });

            // Insert data with SoLuong = 0
            migrationBuilder.InsertData(
                table: "targetcustomerss",
                columns: new[] { "Id", "Name", "SoLuong" },
                values: new object[,]
                {
                    { "1", "Người dân địa phương Đà Lạt", 0 },
                    { "2", "Khách du lịch", 0 },
                    { "3", "Sinh viên", 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "targetcustomerss");
        }
    }
}
