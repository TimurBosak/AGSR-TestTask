using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patient.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddedBirthDateIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Patient_BirthDate",
                table: "Patient",
                column: "BirthDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patient_BirthDate",
                table: "Patient");
        }
    }
}
