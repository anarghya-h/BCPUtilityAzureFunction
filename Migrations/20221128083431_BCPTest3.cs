using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BCPUtilityAzureFunction.Migrations
{
    /// <inheritdoc />
    public partial class BCPTest3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Primary_File",
                table: "SPM_JOB_DETAILS",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Primary_File",
                table: "SPM_JOB_DETAILS");
        }
    }
}
