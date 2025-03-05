using Microsoft.AspNetCore.HttpLogging;
using WebApi.Core;
using WebApi.Data;
using WebApi.Data.Persistence;
namespace WebApi;

public class Program {
   public static void Main(string[] args) {
      var builder = WebApplication.CreateBuilder(args);
      
      // Configure DI-Container aka builder.Services:IServiceCollection
      // ---------------------------------------------------------------------
      // Configure logging
      builder.Logging.ClearProviders();
      builder.Logging.AddConsole();
      builder.Logging.AddDebug();
      // add http logging 
      builder.Services.AddHttpLogging(opts =>
         opts.LoggingFields = HttpLoggingFields.All);
      
      // Add routing options if needed
      // builder.Services.AddRouting(options => {
      //    options.LowercaseUrls = true;
      //    options.LowercaseQueryStrings = true;
      // });
      
      // Add services to the container.
      builder.Services.AddControllers();
      builder.Services.AddScoped<IPeopleRepository, PeopleRepository>();
      builder.Services.AddScoped<IDataContext, DataContext>();
      
      var app = builder.Build();

      // Configure the HTTP request pipeline.
      app.MapControllers();
      app.Run();
   }
}