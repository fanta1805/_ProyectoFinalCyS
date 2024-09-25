using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BE_ProyectoFinal.Migrations
{
    /// <inheritdoc />
    public partial class NuevaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Horario_Salas_SalaId",
                table: "Horario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Horario",
                table: "Horario");

            migrationBuilder.RenameTable(
                name: "Horario",
                newName: "Horarios");

            migrationBuilder.RenameIndex(
                name: "IX_Horario_SalaId",
                table: "Horarios",
                newName: "IX_Horarios_SalaId");

            migrationBuilder.AddColumn<int>(
                name: "capacidad",
                table: "Salas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Horarios",
                table: "Horarios",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Horarios_Salas_SalaId",
                table: "Horarios",
                column: "SalaId",
                principalTable: "Salas",
                principalColumn: "IdSala",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Horarios_Salas_SalaId",
                table: "Horarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Horarios",
                table: "Horarios");

            migrationBuilder.DropColumn(
                name: "capacidad",
                table: "Salas");

            migrationBuilder.RenameTable(
                name: "Horarios",
                newName: "Horario");

            migrationBuilder.RenameIndex(
                name: "IX_Horarios_SalaId",
                table: "Horario",
                newName: "IX_Horario_SalaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Horario",
                table: "Horario",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Horario_Salas_SalaId",
                table: "Horario",
                column: "SalaId",
                principalTable: "Salas",
                principalColumn: "IdSala",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
