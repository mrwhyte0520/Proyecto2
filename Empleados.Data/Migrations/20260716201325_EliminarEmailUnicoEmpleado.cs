using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Empleados.Data.Migrations
{
    /// <inheritdoc />
    public partial class EliminarEmailUnicoEmpleado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Empleados_Email",
                table: "Empleados");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Empleados_Email",
                table: "Empleados",
                column: "Email",
                unique: true);
        }
    }
}
