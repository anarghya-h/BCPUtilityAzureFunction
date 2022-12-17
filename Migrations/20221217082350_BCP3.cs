using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BCPUtilityAzureFunction.Migrations
{
    public partial class BCP3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Verison_Last_Updated_Date",
                table: "SPM_JOB_DETAILS",
                newName: "Version_Last_Updated_Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Version_Last_Updated_Date",
                table: "SPM_JOB_DETAILS",
                newName: "Verison_Last_Updated_Date");
        }
    }
}
