using Microsoft.EntityFrameworkCore.Migrations;

namespace switch_api.Migrations
{
    public partial class InitialDbCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NetworkSwitches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Domain = table.Column<string>(nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkSwitches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vlans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Mode = table.Column<int>(nullable: false),
                    Shutdown = table.Column<bool>(nullable: false),
                    NetworkSwitchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ports_NetworkSwitches_NetworkSwitchId",
                        column: x => x.NetworkSwitchId,
                        principalTable: "NetworkSwitches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PortVlans",
                columns: table => new
                {
                    PortId = table.Column<int>(nullable: false),
                    VlanId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortVlans", x => new { x.PortId, x.VlanId });
                    table.ForeignKey(
                        name: "FK_PortVlans_Ports_PortId",
                        column: x => x.PortId,
                        principalTable: "Ports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PortVlans_Vlans_VlanId",
                        column: x => x.VlanId,
                        principalTable: "Vlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ports_NetworkSwitchId",
                table: "Ports",
                column: "NetworkSwitchId");

            migrationBuilder.CreateIndex(
                name: "IX_PortVlans_VlanId",
                table: "PortVlans",
                column: "VlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PortVlans");

            migrationBuilder.DropTable(
                name: "Ports");

            migrationBuilder.DropTable(
                name: "Vlans");

            migrationBuilder.DropTable(
                name: "NetworkSwitches");
        }
    }
}
