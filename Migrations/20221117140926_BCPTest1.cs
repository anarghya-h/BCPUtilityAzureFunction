using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BCPUtilityAzureFunction.Migrations
{
    /// <inheritdoc />
    public partial class BCPTest1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SPM_JOB_DETAILS",
                columns: table => new
                {
                    DocId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Revision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    DocumentType = table.Column<string>(name: "Document_Type", type: "nvarchar(max)", nullable: true),
                    DisciplineDescription = table.Column<string>(name: "Discipline_Description", type: "nvarchar(max)", nullable: true),
                    FileUID = table.Column<string>(name: "File_UID", type: "nvarchar(max)", nullable: true),
                    FileOBID = table.Column<string>(name: "File_OBID", type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(name: "File_Name", type: "nvarchar(max)", nullable: true),
                    FileLastUpdatedDate = table.Column<string>(name: "File_Last_Updated_Date", type: "nvarchar(max)", nullable: true),
                    PlantCode = table.Column<string>(name: "Plant_Code", type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubUnit = table.Column<string>(name: "Sub_Unit", type: "nvarchar(max)", nullable: true),
                    DocumentLastUpdatedDate = table.Column<string>(name: "Document_Last_Updated_Date", type: "nvarchar(max)", nullable: true),
                    FileNamePath = table.Column<string>(name: "FileName_Path", type: "nvarchar(max)", nullable: true),
                    DocumentRendition = table.Column<string>(name: "Document_Rendition", type: "nvarchar(max)", nullable: true),
                    FileRendition = table.Column<string>(name: "File_Rendition", type: "nvarchar(max)", nullable: true),
                    RenditionOBID = table.Column<string>(name: "Rendition_OBID", type: "nvarchar(max)", nullable: true),
                    RenditionPath = table.Column<string>(name: "Rendition_Path", type: "nvarchar(max)", nullable: true),
                    BCPFlag = table.Column<string>(name: "BCP_Flag", type: "nvarchar(max)", nullable: true),
                    Config = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPM_JOB_DETAILS", x => x.DocId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SPM_JOB_DETAILS");
        }
    }
}
