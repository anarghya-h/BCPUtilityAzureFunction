namespace BCPUtilityAzureFunction.Models.Configs
{
    public class StorageAccountConfig
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }
        public string[] propertyColumns { get; set; }
        public string Container { get; set; }
    }
}
