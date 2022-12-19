using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BCPUtilityAzureFunction.Migrations
{
    public partial class BCP4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFileDeletedInSP",
                table: "SPM_JOB_DETAILS",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFileDeletedInSP",
                table: "SPM_JOB_DETAILS");
        }
    }
}
