using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmsProcessingService.Persistence.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sms",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    from = table.Column<string>(type: "text", nullable: true),
                    to = table.Column<string[]>(type: "text[]", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sms", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sms");
        }
    }
}
