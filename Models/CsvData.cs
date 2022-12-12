using Azure;
using System;
using Azure.Data.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCPUtilityAzureFunction.Models
{
    public class CsvData: ITableEntity
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Revision { get; set; }
        public string Version { get; set; }
        public string Classification { get; set; }
        public string Discipline { get; set; }
        public string FileUid { get; set; }
        public string FileObid { get; set; }
        public string FileName { get; set; }
        #nullable enable 
        public DateTime FileLastUpdatedDate { get; set; }
        #nullable disable
        public string? PlantCode { get; set; }
        public string? Unit { get; set; }
        public string? SubUnit { get; set; }
        #nullable enable
        public DateTime DocumentLastUpdatedDate { get; set; }
        #nullable disable
        public string FileNamePath { get; set; }
        public string? DocumentRendition { get; set; }
        public string? FileRendition { get; set; }
        public string? RenditionObid { get; set; }
        public string? RenditionPath { get; set; }
        public string BCPFlag { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
