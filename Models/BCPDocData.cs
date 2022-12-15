using BCPUtilityAzureFunction.Models.Configs;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace BCPUtilityAzureFunction.Models
{
    public class BCPDocData
    {
        [Key]
        public int DocId { get; set; }
        public string UID { get; set; }
        [JsonProperty(PropertyName ="Name")]
        public string Document_Number { get; set; }
        public string Title { get; set; }
        public string Unit { get; set; }
        public string Sub_Unit { get; set; }
        public string Sub_Unit_Description { get; set; }
        public string Commissioning_System { get; set; }
        public string Commissioning_System_Description { get; set; }
        public string Document_Type { get; set; }
        public DateTime Verison_Last_Updated_Date { get; set; }
        public string Primary_File { get; set; }
        public string Document_Rendition { get; set; }
        public string Rendition_File_Name { get; set; }
        public string Rendition_OBID { get; set; }
        public string Rendition_File { get; set; }
        public string Primary_File_Path { get; set; }
        public string Rendition_File_Path { get; set; }
        public string Revision { get; set;}     
        public string File_UID { get; set;}
        public string File_OBID { get; set;}
        public string File_Name { get; set;}
        public DateTime File_Last_Updated_Date { get; set;}
        public string BCP_Flag { get; set;}
        public string Primary_File_Flag { get; set;}
        public string Config { get; set;}
        public string Id { get; set;}
        public bool IsFileUploaded { get; set; }
        public bool IsFileDeleted { get; set;}
        
    }
}
