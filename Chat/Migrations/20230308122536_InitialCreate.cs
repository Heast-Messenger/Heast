using System.Collections;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Chat.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "permissionchannels",
                columns: table => new
                {
                    PermissionChannelId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissionchannels", x => x.PermissionChannelId);
                });

            migrationBuilder.CreateTable(
                name: "permissionclients",
                columns: table => new
                {
                    PermissionClientId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissionclients", x => x.PermissionClientId);
                });

            migrationBuilder.CreateTable(
                name: "permissionroles",
                columns: table => new
                {
                    PermissionRoleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Hierarchy = table.Column<int>(type: "integer", nullable: false),
                    Permissions = table.Column<BitArray>(type: "bit varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissionroles", x => x.PermissionRoleId);
                });

            migrationBuilder.CreateTable(
                name: "permissionchannelpermissions",
                columns: table => new
                {
                    PermissionChannelId = table.Column<int>(type: "integer", nullable: false),
                    PermissionRoleId = table.Column<int>(type: "integer", nullable: false),
                    Permissions = table.Column<BitArray>(type: "bit varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissionchannelpermissions", x => new { x.PermissionRoleId, x.PermissionChannelId });
                    table.ForeignKey(
                        name: "FK_permissionchannelpermissions_permissionchannels_PermissionC~",
                        column: x => x.PermissionChannelId,
                        principalTable: "permissionchannels",
                        principalColumn: "PermissionChannelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_permissionchannelpermissions_permissionroles_PermissionRole~",
                        column: x => x.PermissionRoleId,
                        principalTable: "permissionroles",
                        principalColumn: "PermissionRoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "permissionclientroles",
                columns: table => new
                {
                    PermissionClientId = table.Column<int>(type: "integer", nullable: false),
                    PermissionRoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissionclientroles", x => new { x.PermissionRoleId, x.PermissionClientId });
                    table.ForeignKey(
                        name: "FK_permissionclientroles_permissionclients_PermissionClientId",
                        column: x => x.PermissionClientId,
                        principalTable: "permissionclients",
                        principalColumn: "PermissionClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_permissionclientroles_permissionroles_PermissionRoleId",
                        column: x => x.PermissionRoleId,
                        principalTable: "permissionroles",
                        principalColumn: "PermissionRoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_permissionchannelpermissions_PermissionChannelId",
                table: "permissionchannelpermissions",
                column: "PermissionChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_permissionclientroles_PermissionClientId",
                table: "permissionclientroles",
                column: "PermissionClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "permissionchannelpermissions");

            migrationBuilder.DropTable(
                name: "permissionclientroles");

            migrationBuilder.DropTable(
                name: "permissionchannels");

            migrationBuilder.DropTable(
                name: "permissionclients");

            migrationBuilder.DropTable(
                name: "permissionroles");
        }
    }
}
