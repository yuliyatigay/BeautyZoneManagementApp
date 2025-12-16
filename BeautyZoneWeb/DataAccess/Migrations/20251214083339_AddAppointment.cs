using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerProcedure");

            migrationBuilder.AddColumn<Guid>(
                name: "ProcedureId",
                table: "Customers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcedureId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Procedures_ProcedureId",
                        column: x => x.ProcedureId,
                        principalTable: "Procedures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PhoneNumber",
                table: "Employees",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ProcedureId",
                table: "Customers",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CustomerId",
                table: "Appointments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Date",
                table: "Appointments",
                column: "Date",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ProcedureId",
                table: "Appointments",
                column: "ProcedureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Procedures_ProcedureId",
                table: "Customers",
                column: "ProcedureId",
                principalTable: "Procedures",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Procedures_ProcedureId",
                table: "Customers");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PhoneNumber",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Customers_ProcedureId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ProcedureId",
                table: "Customers");

            migrationBuilder.CreateTable(
                name: "CustomerProcedure",
                columns: table => new
                {
                    CustomersId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProceduresId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProcedure", x => new { x.CustomersId, x.ProceduresId });
                    table.ForeignKey(
                        name: "FK_CustomerProcedure_Customers_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerProcedure_Procedures_ProceduresId",
                        column: x => x.ProceduresId,
                        principalTable: "Procedures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProcedure_ProceduresId",
                table: "CustomerProcedure",
                column: "ProceduresId");
        }
    }
}
