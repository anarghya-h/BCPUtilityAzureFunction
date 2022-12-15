using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BCPUtilityAzureFunction.Models
{
    public class BCPUtilityDBContext: DbContext
    {
        public BCPUtilityDBContext() { }

        public BCPUtilityDBContext(DbContextOptions<BCPUtilityDBContext> options)  : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("local.settings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        //Table
        public DbSet<BCPDocData> SPM_JOB_DETAILS { get; set; }


    }
}
