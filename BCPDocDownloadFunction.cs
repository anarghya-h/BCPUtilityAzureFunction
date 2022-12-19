using System;
using BCPUtilityAzureFunction.Models.Configs;
using BCPUtilityAzureFunction.Models;
using BCPUtilityAzureFunction.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using X14 = DocumentFormat.OpenXml.Office2010.Excel;
using RestSharp;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Mvc;
using RestSharp.Serializers.NewtonsoftJson;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Serilog.Context;
using Newtonsoft.Json;

namespace BCPUtilityAzureFunction
{
    public class BCPDocDownloadFunction
    {
        #region Private members
        readonly SdxConfig sdxConfig;
        readonly AuthenticationService authService;
        readonly StorageAccountConfig storageConfig;
        readonly BCPUtilityDBContext dBContext;
        readonly BlobStorageService storageService;
        readonly ILogger logger;
        DateTime tokenObtainedAt;
        WorkerConfig workerConfig;
        #endregion

        #region Constructor
        public BCPDocDownloadFunction(SdxConfig config, StorageAccountConfig storageConfig, BCPUtilityDBContext dBContext, ILogger log, WorkerConfig wconfig)
        {
            sdxConfig = config;
            logger = log;
            authService = new AuthenticationService(sdxConfig, logger);
            this.storageConfig = storageConfig;
            this.dBContext = dBContext;
            storageService = new BlobStorageService(storageConfig.ConnectionString, storageConfig.Container);
            workerConfig = wconfig;
        }
        #endregion

        #region Private methods
        private static Stylesheet CreateStylesheet()
        {
            Stylesheet styleSheet = new Stylesheet();
            //Cell Fonts
            Fonts fonts = new Fonts() { Count = (UInt32Value)0U, KnownFonts = true };

            Font font1 = new Font();
            FontSize fontSize1 = new FontSize() { Val = 11D };
            Color color1 = new Color() { Theme = (UInt32Value)1U };
            FontName fontName1 = new FontName() { Val = "Calibri" };
            FontFamilyNumbering fontFamilyNumbering1 = new FontFamilyNumbering() { Val = 2 };
            FontScheme fontScheme1 = new FontScheme() { Val = FontSchemeValues.Minor };

            font1.AppendChild(fontSize1);
            font1.AppendChild(color1);
            font1.AppendChild(fontName1);
            font1.AppendChild(fontFamilyNumbering1);
            font1.AppendChild(fontScheme1);
            fonts.AppendChild(font1);
            fonts.Count++;

            Font font2 = new Font();
            FontSize fontSize2 = new FontSize() { Val = 11D };
            Color color2 = new Color() { Rgb = HexBinaryValue.FromString("0065B3") };
            FontName fontName2 = new FontName() { Val = "Calibri" };
            FontFamilyNumbering fontFamilyNumbering2 = new FontFamilyNumbering() { Val = 2 };
            FontScheme fontScheme2 = new FontScheme() { Val = FontSchemeValues.Minor };
            Underline fontUnderline2 = new Underline();

            font2.AppendChild(fontSize2);
            font2.AppendChild(color2);
            font2.AppendChild(fontName2);
            font2.AppendChild(fontFamilyNumbering2);
            font2.AppendChild(fontScheme2);
            font2.AppendChild(fontUnderline2);
            fonts.AppendChild(font2);
            fonts.Count++;

            //Cell Fills
            Fills fills = new Fills() { Count = (UInt32Value)0U };

            // FillId = 0
            Fill fill1 = new Fill();
            PatternFill patternFill1 = new PatternFill() { PatternType = PatternValues.None };
            fill1.AppendChild(patternFill1);

            // FillId = 1
            Fill fill2 = new Fill();
            PatternFill patternFill2 = new PatternFill() { PatternType = PatternValues.Gray125 };
            fill2.AppendChild(patternFill2);

            fills.AppendChild(fill1);
            fills.Count++;
            fills.AppendChild(fill2);
            fills.Count++;

            //Cell Borders
            Borders borders = new Borders() { Count = (UInt32Value)1U };

            Border border1 = new Border();
            LeftBorder leftBorder1 = new LeftBorder();
            RightBorder rightBorder1 = new RightBorder();
            TopBorder topBorder1 = new TopBorder();
            BottomBorder bottomBorder1 = new BottomBorder();
            DiagonalBorder diagonalBorder1 = new DiagonalBorder();

            border1.AppendChild(leftBorder1);
            border1.AppendChild(rightBorder1);
            border1.AppendChild(topBorder1);
            border1.AppendChild(bottomBorder1);
            border1.AppendChild(diagonalBorder1);

            borders.AppendChild(border1);

            //Cell Formats
            CellStyleFormats cellStyleFormats1 = new CellStyleFormats() { Count = (UInt32Value)1U };
            CellFormat cellFormat1 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };

            cellStyleFormats1.AppendChild(cellFormat1);

            CellFormats cellFormats = new CellFormats() { Count = (UInt32Value)0U };
            CellFormat cellFormat2 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };
            CellFormat cellFormat3 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)1U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyFill = true };
            CellFormat cellFormat4 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, Alignment = new Alignment { WrapText = true }, ApplyAlignment = true };


            cellFormats.AppendChild(cellFormat2);
            cellFormats.Count++;
            cellFormats.AppendChild(cellFormat3);
            cellFormats.Count++;
            cellFormats.AppendChild(cellFormat4);
            cellFormats.Count++;

            CellStyles cellStyles = new CellStyles() { Count = (UInt32Value)1U };
            CellStyle cellStyle1 = new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };

            cellStyles.AppendChild(cellStyle1);
            DifferentialFormats differentialFormats1 = new DifferentialFormats() { Count = (UInt32Value)0U };
            TableStyles tableStyles = new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleMedium9" };

            StylesheetExtensionList stylesheetExtensionList1 = new StylesheetExtensionList();

            StylesheetExtension stylesheetExtension1 = new StylesheetExtension() { Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}" };
            X14.SlicerStyles slicerStyles1 = new X14.SlicerStyles() { DefaultSlicerStyle = "SlicerStyleLight1" };

            stylesheetExtension1.AppendChild(slicerStyles1);

            stylesheetExtensionList1.AppendChild(stylesheetExtension1);

            //Appending the fonts, fills and cell formats to the style sheet
            styleSheet.AppendChild(fonts);
            styleSheet.AppendChild(fills);
            styleSheet.AppendChild(borders);
            styleSheet.AppendChild(cellStyleFormats1);
            styleSheet.AppendChild(cellFormats);
            styleSheet.AppendChild(cellStyles);
            styleSheet.AppendChild(differentialFormats1);
            styleSheet.AppendChild(tableStyles);
            styleSheet.AppendChild(stylesheetExtensionList1);
            return styleSheet;
        }

        private async Task CheckIfTokenIsValid()
        {
            CancellationToken cancellationToken = default;
            if (tokenObtainedAt.AddSeconds(authService.tokenResponse.ExpiresIn - 100) == DateTime.Now)
            {
                await authService.GetAccessTokenAsync(cancellationToken);
                tokenObtainedAt = DateTime.Now;
            }

        }

        private async Task UpdateDocumentFilesAsync(BCPDocData tdata, BCPDocData record, RestClient client)
        {
            logger.Information("Updating the files for the document");
            //Deleting the old files
            if(!tdata.IsFileDeleted)
            {
                storageService.DeleteBlob(/*StorageUrl +*/ "BCPDocuments/" + tdata.UID + "/" + tdata.File_Name);
                if (tdata.Rendition_OBID != null)
                    storageService.DeleteBlob(/*StorageUrl +*/ "BCPDocuments/" + tdata.UID + "/" + tdata.Rendition_File_Name);
            }

            //Downloading the updated file
            string DirectoryName = /*StorageUrl +*/ "BCPDocuments/" + record.UID;
            WebClient webClient = new();

            //Query to obtain the file details along with its URL
            string OdataQueryFileUri = sdxConfig.ServerBaseUri + "Files('" + record.File_OBID + "')/Intergraph.SPF.Server.API.Model.RetrieveFileUris";
            var request = new RestRequest(OdataQueryFileUri);

            //Checking if token is about to expire
            await CheckIfTokenIsValid();

            request.AddHeader("Authorization", "Bearer " + authService.tokenResponse.AccessToken);
            request.AddHeader("X-Ingr-OnBehalfOf", sdxConfig.OnBehalfOfUser);

            var response3 = await client.GetAsync<ApiResponse<FileData>>(request);
            //webClient.DownloadFile(response3.Value[0].Uri, DirectoryName + "\\" + record.File_Name);

            MemoryStream ms = new MemoryStream(webClient.DownloadData(response3.Value[0].Uri));
            var fileUrl = storageService.UploadFileToBlob(DirectoryName + "/" + record.File_Name, ms);
            record.Primary_File_Path = fileUrl.ToString();

            //record.FileName_Path = DirectoryName + "\\" + record.Name;

            if (record.Rendition_OBID != null)
            {
                //Downloading the updated rendition file
                logger.Information("Retrieving the file details for: {filename}", record.Rendition_File_Name);

                //DirectoryName = /*StorageUrl +*/ "BCPDocuments/" + record.UID;

                //Query to obtain the file details along with its URL
                OdataQueryFileUri = sdxConfig.ServerBaseUri + "Files('" + record.Rendition_OBID + "')/Intergraph.SPF.Server.API.Model.RetrieveFileUris";
                request = new RestRequest(OdataQueryFileUri);

                //Checking if token is about to expire
                await CheckIfTokenIsValid();

                request.AddHeader("Authorization", "Bearer " + authService.tokenResponse.AccessToken);
                request.AddHeader("X-Ingr-OnBehalfOf", sdxConfig.OnBehalfOfUser);

                response3 = await client.GetAsync<ApiResponse<FileData>>(request);
                //webClient.DownloadFile(response3.Value[0].Uri, DirectoryName + "\\" + record.File_Rendition);

                ms = new MemoryStream(webClient.DownloadData(response3.Value[0].Uri));
                fileUrl = storageService.UploadFileToBlob(DirectoryName + "/" + record.Rendition_File_Name, ms);
                record.Rendition_File_Path = fileUrl.ToString();
                
            }

            //Updating the value in the database
            record.DocId = tdata.DocId;
            dBContext.SPM_JOB_DETAILS.Update(record);
            dBContext.SaveChanges();

            logger.Information("Successfully updated the files for the document");

        }

        private async Task DownloadFileAsync(BCPDocData record, RestClient client)
        {
            logger.Information("Retrieving the file details for: {file_name}", record.File_Name);

            string DirectoryName = /*StorageUrl +*/ "BCPDocuments/" + record.UID;
            WebClient webClient = new();

            //Query to obtain the file details along with its URL
            string OdataQueryFileUri = sdxConfig.ServerBaseUri + "Files('" + record.File_OBID + "')/Intergraph.SPF.Server.API.Model.RetrieveFileUris";
            var request = new RestRequest(OdataQueryFileUri);

            //Checking if token is about to expire
            await CheckIfTokenIsValid();

            request.AddHeader("Authorization", "Bearer " + authService.tokenResponse.AccessToken);
            request.AddHeader("X-Ingr-OnBehalfOf", sdxConfig.OnBehalfOfUser);

            var response3 = await client.GetAsync<ApiResponse<FileData>>(request);

            //Downloading the file                        
            //webClient.DownloadFile(response3.Value[0].Uri, DirectoryName + "\\" + record.File_Name);
            MemoryStream ms = new MemoryStream(webClient.DownloadData(response3.Value[0].Uri));
            var fileUrl = storageService.UploadFileToBlob(DirectoryName + "/" + record.File_Name, ms);
            record.Primary_File_Path = fileUrl.ToString();

            //record.FileName_Path = DirectoryName + "\\" + record.Name;

            if (record.Rendition_OBID != null)
            {
                logger.Information("Retrieving the file details for: {filename}", record.Rendition_File_Name);

                //DirectoryName = /*StorageUrl + */"BCPDocuments/" + record.UID;

                //Query to obtain the file details along with its URL
                OdataQueryFileUri = sdxConfig.ServerBaseUri + "Files('" + record.Rendition_OBID + "')/Intergraph.SPF.Server.API.Model.RetrieveFileUris";
                request = new RestRequest(OdataQueryFileUri);

                //Checking if token is about to expire
                await CheckIfTokenIsValid();

                request.AddHeader("Authorization", "Bearer " + authService.tokenResponse.AccessToken);
                request.AddHeader("X-Ingr-OnBehalfOf", sdxConfig.OnBehalfOfUser);

                response3 = await client.GetAsync<ApiResponse<FileData>>(request);

                //Downloading the file                        
                //webClient.DownloadFile(response3.Value[0].Uri, DirectoryName + "\\" + record.File_Rendition);
                ms = new MemoryStream(webClient.DownloadData(response3.Value[0].Uri));
                fileUrl = storageService.UploadFileToBlob(DirectoryName + "/" + record.Rendition_File_Name, ms);
                record.Rendition_File_Path = fileUrl.ToString();
                //record.Rendition_Path = DirectoryName + "\\" + record.File_Rendition;
            }

            logger.Information("Successfully retrieved the file details");
        }

        [HttpGet]
        private async Task GetFilesAsync()
        {
            int i = 0;
            WorksheetPart worksheetPart = null;
            SpreadsheetDocument spreadsheetDocument = null;
            SheetData sheetData = null;
            List<BCPDocData> records = new();
            CancellationToken cancellationToken = default;

            char[] reference = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            try
            {
                var client = new RestClient().UseNewtonsoftJson();

                await authService.GetAccessTokenAsync(cancellationToken);
                tokenObtainedAt = DateTime.Now;

                logger.Information("Retrieving the details of BCP documents");
                //Query to obtain the BCP documents
                string OdataQueryBcpDocsCount = sdxConfig.ServerBaseUri + "BCPDocuments?$filter= BCP_Flag eq 'e1CFIHOS_yesno_yes' and Primary_File_Flag eq 'e1CFIHOS_yesno_yes'&$count=true";
                string OdataQueryBcpDocs = sdxConfig.ServerBaseUri + "BCPDocuments?$filter= BCP_Flag eq 'e1CFIHOS_yesno_yes' and Primary_File_Flag eq 'e1CFIHOS_yesno_yes'&$count=true&$top=";
                var request = new RestRequest(OdataQueryBcpDocsCount);

                //Checking if token is about to expire
                await CheckIfTokenIsValid();

                request.AddHeader("Authorization", "Bearer " + authService.tokenResponse.AccessToken);
                request.AddHeader("X-Ingr-OnBehalfOf", sdxConfig.OnBehalfOfUser);

                var countResponse = await client.GetAsync<ODataQueryResponse>(request);

                OdataQueryBcpDocs += countResponse.Count;
                request = new RestRequest(OdataQueryBcpDocs);

                //Checking if token is about to expire
                await CheckIfTokenIsValid();

                request.AddHeader("Authorization", "Bearer " + authService.tokenResponse.AccessToken);
                request.AddHeader("X-Ingr-OnBehalfOf", sdxConfig.OnBehalfOfUser);

                var DocDetailsResponse = await client.GetAsync<ODataQueryResponse>(request);
                records.AddRange(DocDetailsResponse.Value);

                while (DocDetailsResponse.NextLink != null)
                {
                    request = new RestRequest(DocDetailsResponse.NextLink);

                    await CheckIfTokenIsValid();

                    request.AddHeader("Authorization", "Bearer " + authService.tokenResponse.AccessToken);
                    request.AddHeader("X-Ingr-OnBehalfOf", sdxConfig.OnBehalfOfUser);

                    DocDetailsResponse = await client.GetAsync<ODataQueryResponse>(request);
                    records.AddRange(DocDetailsResponse.Value);
                }

                //Obtaining the existing data from the table
                //var tableData = tableClient.Query<CsvData>().ToList();
                logger.Information("Obtaining the existing data from the table");
                var DbTableData = dBContext.SPM_JOB_DETAILS.ToList();

                //Creating the index file
                MemoryStream ms = new();
                spreadsheetDocument = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.MacroEnabledWorkbook);

                WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();

                worksheetPart = workbookPart.AddNewPart<WorksheetPart>();

                Workbook workbook = new Workbook();
                FileVersion fileVersion = new FileVersion
                {
                    ApplicationName = "Microsoft Office Excel"
                };

                Worksheet worksheet = new Worksheet();
                WorkbookStylesPart workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
                workbookStylesPart.Stylesheet = CreateStylesheet();
                workbookStylesPart.Stylesheet.Save();

                var columnsList = records[0].GetType().GetProperties();
                Columns sheetColumns = new Columns();
                Column sheetColumn = new Column()
                {
                    BestFit = true,
                    Min = 1,
                    Max = Convert.ToUInt32(columnsList.Length),
                    CustomWidth = true,
                    Width = 30
                };
                sheetColumns.AppendChild(sheetColumn);
                worksheet.AppendChild(sheetColumns);

                sheetData = new SheetData();

                Hyperlinks hyperlinks = new Hyperlinks();


                //Adding the headers
                Row headerRow = new Row();
                foreach (var column in records[0].GetType().GetProperties())
                {
                    if (column.Name == "UID" || column.Name == "DocId" || column.Name == "Config" || column.Name == "Id" || column.Name == "IsFileUploaded" || column.Name == "IsFileDeleted")
                            continue;
                    Cell cell = new Cell()
                    {
                        CellValue = new CellValue(column.Name.ToUpper()),
                        DataType = CellValues.String
                    };
                    headerRow.AppendChild(cell);
                }
                sheetData.AppendChild(headerRow);


                foreach (var record in records)
                {
                    LogContext.PushProperty("DocumentNumber", record.Document_Number);

                    //record.File_Last_Updated_Date = record.File_Last_Updated_Date.ToUniversalTime();
                    //record.Document_Last_Updated_Date = record.Document_Last_Updated_Date.ToUniversalTime();
                    record.Primary_File_Path = "";
                    record.Rendition_File_Path = "";
                    record.IsFileUploaded = false;
                    record.IsFileDeleted = false;
                    Row r = new();

                    var tdata = DbTableData.Find(x => x.File_UID == record.File_UID);
                    if (tdata != null)
                    {
                        if (tdata.Revision == record.Revision && DateTime.Equals(tdata.File_Last_Updated_Date, record.File_Last_Updated_Date))
                        {
                            record.Primary_File_Path = tdata.Primary_File_Path;
                            record.Rendition_File_Path = tdata.Rendition_File_Path;
                            record.IsFileUploaded = tdata.IsFileUploaded;
                            if(tdata.IsFileDeleted)
                            {
                                tdata.IsFileDeleted = record.IsFileDeleted;
                                await DownloadFileAsync(record, client);
                            }
                            /*if (tdata.RenditionObid == record.RenditionObid)
                                record.RenditionPath = tdata.RenditionPath;
                            else
                            {
                                await UpdateRenditionFilesAsync(tdata, record, client);
                            }*/
                        }
                        else if (DateTime.Compare(record.File_Last_Updated_Date, tdata.File_Last_Updated_Date) > 0)
                        {
                            await UpdateDocumentFilesAsync(tdata, record, client);
                        }
                        else if (string.Compare(record.Revision, tdata.Revision) > 0)
                        {
                            await UpdateDocumentFilesAsync(tdata, record, client);
                        }
                        /*else if (record.Version > tdata.Version)
                        {
                            await UpdateDocumentFilesAsync(tdata, record, client);
                        }*/
                    }

                    else
                    {
                        if (record.File_OBID != null)
                        {
                            await DownloadFileAsync(record, client);
                        }

                        /*record.PartitionKey = Guid.NewGuid().ToString();
                        record.RowKey = Guid.NewGuid().ToString();                        
                        tableClient.AddEntity(record);*/
                        dBContext.SPM_JOB_DETAILS.Add(record);
                        dBContext.SaveChanges();
                    }

                    /*record.FileLastUpdatedDate = record.FileLastUpdatedDate.ToLocalTime();
                    record.DocumentLastUpdatedDate = record.DocumentLastUpdatedDate.ToLocalTime();*/

                    logger.Information("Adding the record to the index file");
                    foreach (var column in records[0].GetType().GetProperties())
                    {
                        if (column.Name == "UID" || column.Name == "DocId" || column.Name == "Config" || column.Name == "Id" || column.Name == "IsFileUploaded" || column.Name == "IsFileDeleted" )
                            continue;
                        Cell cell = new()
                        {
                            //CellValue = new CellValue(record.GetType().GetProperty(column.Name).GetValue(record).ToString()),
                            DataType = CellValues.String,
                            StyleIndex = 0
                        };
                        if (record.GetType().GetProperty(column.Name).GetValue(record) == null)
                            cell.CellValue = new CellValue("");
                        else
                            cell.CellValue = new CellValue(record.GetType().GetProperty(column.Name).GetValue(record).ToString());

                        if (column.Name == "Document_Number" && record.File_OBID != null)
                        {
                            //int index = Array.FindIndex(columnsList, x => x.Name == column.Name) - 1;
                            int index = 0;
                            Hyperlink hyperlink = new Hyperlink()
                            {
                                Reference = reference[index].ToString() + (records.IndexOf(record) + 2),
                                Id = "HYP" + i,
                                Display = record.Document_Number
                            };
                            hyperlinks.AppendChild(hyperlink);
                            cell.CellValue = new CellValue(hyperlink.Display.Value);
                            cell.StyleIndex = 1;
                            worksheetPart.AddHyperlinkRelationship(new Uri(record.UID + "/" + record.File_Name, UriKind.Relative), true, hyperlink.Id);
                            i++;
                        }

                        if (column.Name == "Rendition_File_Path" && record.Rendition_OBID != null)
                        {
                            //int index = Array.FindIndex(columnsList, x => x.Name == column.Name) - 1;
                            int index = 15;
                            Hyperlink hyperlink = new()
                            {
                                Reference = reference[index].ToString() + (records.IndexOf(record) + 2),
                                Id = "HYP" + i,
                                Display = "Click Here"
                            };
                            hyperlinks.AppendChild(hyperlink);
                            cell.CellValue = new CellValue(hyperlink.Display.Value);
                            cell.StyleIndex = 1;
                            worksheetPart.AddHyperlinkRelationship(new Uri(record.UID + "/" + record.Rendition_File_Name, UriKind.Relative), true, hyperlink.Id);
                            i++;
                        }
                        r.AppendChild(cell);
                    }
                    sheetData.AppendChild(r);

                }

                //appending the sheet data to the Worksheet
                worksheet.AppendChild(sheetData);
                worksheet.AppendChild(hyperlinks);
                worksheetPart.Worksheet = worksheet;
                worksheetPart.Worksheet.Save();

                //Creating new sheet
                Sheets sheets = new Sheets();
                Sheet sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "BCP_DOCUMENT_EXTRACT"
                };
                sheets.AppendChild(sheet);
                workbook.AppendChild(fileVersion);
                workbook.AppendChild(sheets);

                spreadsheetDocument.WorkbookPart.Workbook = workbook;
                spreadsheetDocument.WorkbookPart.Workbook.Save();
                spreadsheetDocument.Save();
                spreadsheetDocument.Close();

                ms.Position = 0;
                var fileUrl = storageService.UploadFileToBlob("BCPDocuments/SPM BCP DOCUMENTS EXTRACT.xlsm", ms);
                var existingIndexFile = dBContext.SPM_JOB_DETAILS.Where(x => x.Primary_File_Path == fileUrl.ToString()).FirstOrDefault();
                if(existingIndexFile != null)
                {
                    existingIndexFile.File_Last_Updated_Date = DateTime.Now;
                    existingIndexFile.IsFileUploaded = false;
                    dBContext.SPM_JOB_DETAILS.Update(existingIndexFile);
                    dBContext.SaveChanges();
                }
                else
                {
                    BCPDocData bCPIndexFileData = new BCPDocData()
                    {
                        Primary_File_Path = fileUrl.ToString(),
                        Document_Number = "SPM BCP DOCUMENTS EXTRACT.xlsm",
                        File_Name = "SPM BCP DOCUMENTS EXTRACT.xlsm",
                        Title = "Index file of BCP Documents",
                        File_Last_Updated_Date = DateTime.Now,
                        IsFileUploaded = false
                    };
                    dBContext.SPM_JOB_DETAILS.Add(bCPIndexFileData);
                    dBContext.SaveChanges();

                }                
                
                logger.Information("Index file created successfully");

                logger.Information("Deleting old document files");
                //tableData = tableClient.Query<CsvData>().ToList();
                DbTableData = dBContext.SPM_JOB_DETAILS.ToList();

                List<BCPDocData> oldRecords = DbTableData.Where(x => !records.Any(y => x.File_UID == y.File_UID) && !x.IsFileDeleted).ToList();
                foreach (var record in oldRecords)
                {
                    if (record.Title == "Index file of BCP Documents")
                        continue;
                    
                    var path = records.Find(x => x.Primary_File_Path == record.Primary_File_Path);
                    if (path == null)
                    {
                        if (storageService.CheckExists("BCPDocuments/" + record.UID + "/" + record.File_Name))
                        {
                            LogContext.PushProperty("DocumentNumber", record.Document_Number);
                            logger.Information("Deleting file: {file_name}", record.File_Name);
                            storageService.DeleteBlob(/*StorageUrl +*/ "BCPDocuments/" + record.UID + "/" + record.File_Name);
                            if (storageService.CheckExists("BCPDocuments/" + record.UID + "/" + record.Rendition_File_Name))
                            {
                                logger.Information("Deleting file: {file_name}", record.File_Name);
                                storageService.DeleteBlob(/*StorageUrl +*/ "BCPDocuments/" + record.UID + "/" + record.Rendition_File_Name);
                            }
                                
                        }
                    }
                    record.IsFileDeleted = true;
                    dBContext.SPM_JOB_DETAILS.Update(record);
                    dBContext.SaveChanges();
                }

                /*foreach (var tdata in DbTableData)
                {
                    var record = records.Find(x => x.File_UID == tdata.File_UID);
                    if (record == null && tdata.Title != "Index file of BCP Documents")
                    {
                        var path = records.Find(x => x.Primary_File_Path == tdata.Primary_File_Path);
                        if (path == null)
                        {
                            if (storageService.CheckExists("BCPDocuments/" + tdata.Document_Number + "/" + "Design_Files_" + tdata.Document_Number + "/" + tdata.File_Name))
                            {
                                LogContext.PushProperty("DocumentNumber", tdata.Document_Number);
                                logger.Information("Deleting file: {file_name}", tdata.File_Name);
                                storageService.DeleteBlob(*//*StorageUrl +*//* "BCPDocuments/" + tdata.Document_Number + "/" + "Design_Files_" + tdata.Document_Number + "/" + tdata.File_Name);
                                if (storageService.CheckExists(tdata.Document_Number + "/" + "DRnd_" + tdata.Document_Number + "/" + tdata.Rendition_File_Name))
                                    storageService.DeleteBlob(*//*StorageUrl +*//* "BCPDocuments/" + tdata.Document_Number + "/" + "DRnd_" + tdata.Document_Number + "/" + tdata.Rendition_File_Name);

                            }
                        }
                        dBContext.SPM_JOB_DETAILS.Remove(tdata);
                        dBContext.SaveChanges();
                        //tableClient.DeleteEntity(tdata.PartitionKey, tdata.RowKey);                        
                        //if(Directory.Exists(StorageUrl + tdata.Name))


                    }
                }*/

                //Success
                logger.Information("File(s) downloaded successfully");

            }
            catch (Exception e)
            {
                logger.Error(e, "Could not download the BCP documents: {message}", e.Message);
            }
        }

        #endregion

        #region Public methods

        /*[FunctionName("BCPDocDownloadFunction")]
        public async Task Run([TimerTrigger(scheduleExpression: "%Schedule%")] TimerInfo myTimer)
        {
            logger.Information($"Timer trigger function executed at: {DateTime.Now}");
           
            await GetFilesAsync();
        }*/

        [FunctionName("BCPDocDownloadFunction")]
        public async Task Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            logger.Information("Request received, executing HTTP trigger function");
            await GetFilesAsync();
        }
        #endregion
    }
}
