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
        public string Name { get; set; }
        public string Title { get; set;}
        public string Revision { get; set;}
        public int Version { get; set;}
        public string Document_Type { get; set;}
        public string Discipline_Description { get; set;}
        public string File_UID { get; set;}
        public string File_OBID { get; set;}
        public string File_Name { get; set;}
        //[JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime File_Last_Updated_Date { get; set;}
        public string Plant_Code { get; set;}
        public string Unit { get; set;}
        public string Sub_Unit { get; set;}
        //[JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Document_Last_Updated_Date { get; set;}
        public string FileName_Path { get; set;}
        public string Document_Rendition { get; set;}
        public string File_Rendition { get; set;}
        public string Rendition_OBID { get; set;}
        public string Rendition_Path { get; set;}
        public string BCP_Flag { get; set;}
        public string Primary_File { get; set;}
        public string Config { get; set;}
        public string Id { get; set;}
        
    }
}
