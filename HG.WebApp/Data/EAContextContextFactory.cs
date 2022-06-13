using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HG.WebApp.Data
{
    public class EAContextContextFactory : IDesignTimeDbContextFactory<EAContext>
    {
        public EAContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("EAContext");

            var optionsBuilder = new DbContextOptionsBuilder<EAContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new EAContext(optionsBuilder.Options);
        }
    }
}
