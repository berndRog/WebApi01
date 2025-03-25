using Microsoft.AspNetCore.HttpLogging;
using WebApi.Core;
using WebApi.Data;
using WebApi.Data.Repositories;
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
      builder.Services.AddControllers()
         .AddJsonOptions(options => {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
//            options.JsonSerializerOptions.PropertyNamingPolicy = new LowerCaseNamingPolicy();
            
         });
         
      builder.Services.AddSingleton<IPeopleRepository, PeopleRepository>();
      builder.Services.AddSingleton<IDataContext, DataContext>();
      
      var app = builder.Build();

      // Configure the HTTP request pipeline.
      app.UseHttpLogging();
      app.MapControllers();
      app.Run();
   }
}

public class LowerCaseNamingPolicy : System.Text.Json.JsonNamingPolicy {
   public override string ConvertName(string name) => name.ToLower();
}