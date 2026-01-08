using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameStateToProvince : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                table: "Professionals",
                newName: "Province");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Customers",
                newName: "Province");

            // Update default country values from "US" to "CA"
            migrationBuilder.Sql(@"
                UPDATE Professionals SET Country = 'CA' WHERE Country = 'US';
                UPDATE Customers SET Country = 'CA' WHERE Country = 'US';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Province",
                table: "Professionals",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "Province",
                table: "Customers",
                newName: "State");

            // Revert country values from "CA" to "US"
            migrationBuilder.Sql(@"
                UPDATE Professionals SET Country = 'US' WHERE Country = 'CA';
                UPDATE Customers SET Country = 'US' WHERE Country = 'CA';
            ");
        }
    }
}
