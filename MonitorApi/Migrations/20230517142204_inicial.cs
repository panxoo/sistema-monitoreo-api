using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonitorApi.Migrations
{
    public partial class inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agrupacions",
                columns: table => new
                {
                    AgrupacionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agrupacions", x => x.AgrupacionID);
                });

            migrationBuilder.CreateTable(
                name: "JobMonitors",
                columns: table => new
                {
                    JobMonitorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobMonitors", x => x.JobMonitorID);
                });

            migrationBuilder.CreateTable(
                name: "Monitores",
                columns: table => new
                {
                    MonitoreID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Procedimiento = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    Alerta = table.Column<bool>(type: "bit", nullable: false),
                    Job_MonitorID = table.Column<int>(type: "int", nullable: false),
                    JobMonitorID = table.Column<int>(type: "int", nullable: false),
                    AgrupacionID = table.Column<int>(type: "int", nullable: false),
                    KeyMonitorProce = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monitores", x => x.MonitoreID);
                    table.ForeignKey(
                        name: "FK_Monitores_Agrupacions_AgrupacionID",
                        column: x => x.AgrupacionID,
                        principalTable: "Agrupacions",
                        principalColumn: "AgrupacionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Monitores_JobMonitors_JobMonitorID",
                        column: x => x.JobMonitorID,
                        principalTable: "JobMonitors",
                        principalColumn: "JobMonitorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonitorEstadoHists",
                columns: table => new
                {
                    MonitorEstadoHistID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MonitorID = table.Column<int>(type: "int", nullable: false),
                    FalsoPositivo = table.Column<bool>(type: "bit", nullable: false),
                    Nota = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcesoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitorEstadoHists", x => x.MonitorEstadoHistID);
                    table.ForeignKey(
                        name: "FK_MonitorEstadoHists_Monitores_MonitorID",
                        column: x => x.MonitorID,
                        principalTable: "Monitores",
                        principalColumn: "MonitoreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonitorEstados",
                columns: table => new
                {
                    MonitorEstadoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MonitorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitorEstados", x => x.MonitorEstadoID);
                    table.ForeignKey(
                        name: "FK_MonitorEstados_Monitores_MonitorID",
                        column: x => x.MonitorID,
                        principalTable: "Monitores",
                        principalColumn: "MonitoreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonitorEstadoUltimos",
                columns: table => new
                {
                    MonitorEstadoUltimoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeriodoError = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaEstado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MonitorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitorEstadoUltimos", x => x.MonitorEstadoUltimoID);
                    table.ForeignKey(
                        name: "FK_MonitorEstadoUltimos_Monitores_MonitorID",
                        column: x => x.MonitorID,
                        principalTable: "Monitores",
                        principalColumn: "MonitoreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Monitores_AgrupacionID",
                table: "Monitores",
                column: "AgrupacionID");

            migrationBuilder.CreateIndex(
                name: "IX_Monitores_JobMonitorID",
                table: "Monitores",
                column: "JobMonitorID");

            migrationBuilder.CreateIndex(
                name: "IX_MonitorEstadoHists_MonitorID",
                table: "MonitorEstadoHists",
                column: "MonitorID");

            migrationBuilder.CreateIndex(
                name: "IX_MonitorEstados_MonitorID",
                table: "MonitorEstados",
                column: "MonitorID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonitorEstadoUltimos_MonitorID",
                table: "MonitorEstadoUltimos",
                column: "MonitorID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonitorEstadoHists");

            migrationBuilder.DropTable(
                name: "MonitorEstados");

            migrationBuilder.DropTable(
                name: "MonitorEstadoUltimos");

            migrationBuilder.DropTable(
                name: "Monitores");

            migrationBuilder.DropTable(
                name: "Agrupacions");

            migrationBuilder.DropTable(
                name: "JobMonitors");
        }
    }
}
