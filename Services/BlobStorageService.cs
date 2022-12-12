using Azure.Storage.Blobs;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.IO;

namespace BCPUtilityAzureFunction.Services
{
    public class BlobStorageService
    {
        #region Private members
        string AccessKey { get; set; }
        string ContainerName { get; set; }
        BlobContainerClient client { get; set; }
        #endregion

        #region Public methods
        public BlobStorageService(string Key, string container)
        {
            this.AccessKey = Key;
            this.ContainerName = container;
            client = new BlobContainerClient(AccessKey, ContainerName);
        }

        //Method to upload files to the blob storage
        public Uri UploadFileToBlob(string FileName, MemoryStream fileData)
        {
            var blob = client.GetBlobClient(FileName);
            var task = blob.UploadAsync(fileData, overwrite: true);
            task.Wait();
            return blob.Uri;
        }

        //Method to delete the files from the blob storage
        public void DeleteBlob(string fileUrl)
        {
            var blob = client.GetBlobClient(fileUrl);
            var task = blob.DeleteAsync();
            task.Wait();
        }

        //Method to check if a particular file exists or not
        public bool CheckExists(string FileUrl)
        {
            var blob = client.GetBlobClient(FileUrl);
            var task = blob.ExistsAsync();
            task.Wait();
            return task.Result.Value;
        }
        #endregion
    }
}
