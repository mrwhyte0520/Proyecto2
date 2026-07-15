using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Empleados.Data.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarCorreosGmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 1,
                column: "Email",
                value: "juan.perez@gmail.com");

            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 2,
                column: "Email",
                value: "maria.gomez@gmail.com");

            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 3,
                column: "Email",
                value: "carlos.rodriguez@gmail.com");

            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 4,
                column: "Email",
                value: "ana.martinez@gmail.com");

            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 5,
                column: "Email",
                value: "luis.fernandez@gmail.com");

            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 6,
                column: "Email",
                value: "sofia.castillo@gmail.com");

            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 7,
                column: "Email",
                value: "pedro.ramirez@gmail.com");

            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 8,
                column: "Email",
                value: "laura.jimenez@gmail.com");

            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 9,
                column: "Email",
                value: "miguel.torres@gmail.com");

            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 10,
                column: "Email",
                value: "daniela.vargas@gmail.com");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 1,
                column: "Email",
                value: "juan.perez@empresa.com");

            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 2,
                column: "Email",
                value: "maria.gomez@empresa.com");

            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 3,
                column: "Email",
                value: "carlos.rodriguez@empresa.com");

            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 4,
                column: "Email",
                value: "ana.martinez@empresa.com");

            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 5,
                column: "Email",
                value: "luis.fernandez@empresa.com");

            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 6,
                column: "Email",
                value: "sofia.castillo@empresa.com");

            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 7,
                column: "Email",
                value: "pedro.ramirez@empresa.com");

            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 8,
                column: "Email",
                value: "laura.jimenez@empresa.com");

            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 9,
                column: "Email",
                value: "miguel.torres@empresa.com");

            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 10,
                column: "Email",
                value: "daniela.vargas@empresa.com");
        }
    }
}
