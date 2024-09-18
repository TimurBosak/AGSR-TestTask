using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patient.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddedGenderToModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Patient",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Patient");
        }
    }
}
