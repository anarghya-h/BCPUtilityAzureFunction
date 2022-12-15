using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BCPUtilityAzureFunction.Migrations
{
    public partial class BCP1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SPM_JOB_DETAILS",
                columns: table => new
                {
                    DocId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Document_Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sub_Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sub_Unit_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Commissioning_System = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Commissioning_System_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Document_Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Verison_Last_Updated_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Primary_File = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Document_Rendition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rendition_File_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rendition_OBID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rendition_File = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Primary_File_Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rendition_File_Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Revision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    File_UID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    File_OBID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    File_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    File_Last_Updated_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BCP_Flag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Primary_File_Flag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Config = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFileUploaded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPM_JOB_DETAILS", x => x.DocId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SPM_JOB_DETAILS");
        }
    }
}
