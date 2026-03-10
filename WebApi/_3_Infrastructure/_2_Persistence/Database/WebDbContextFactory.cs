using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
[assembly: InternalsVisibleTo("WebApiTest")]
namespace WebApi._3_Infrastructure._2_Persistence.Database;

internal class WebDbContextFactory : IDesignTimeDbContextFactory<WebDbContext> {
   public WebDbContext CreateDbContext(string[] args) {
      var configuration = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: false)
         .AddJsonFile("appsettings.Development.json", optional: true)
         .Build();

      var connectionString = configuration.GetConnectionString("WebApiDb");
      Console.WriteLine("---> Using SQLite connection string: " + connectionString);

      var optionsBuilder = new DbContextOptionsBuilder<WebDbContext>();
      // Passen Sie den Connection String an Ihre Umgebung an
      optionsBuilder.UseSqlite(connectionString);
      // Oder für SQL Server:
      // optionsBuilder.UseSqlServer("Server=localhost;Database=banking_dev;Trusted_Connection=True;");

      return new WebDbContext(optionsBuilder.Options);
   }
}