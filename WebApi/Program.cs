using WebApi._2_Modules.Customers._1_Ports.Outbound;
using WebApi._2_Modules.Customers._3_Domain.Entities;
using WebApi._2_Modules.Customers._3_Domain.ValueObjects;
using WebApi._3_Infrastructure._2_Persistence;
using WebApi._3_Infrastructure._2_Persistence.Database;
namespace WebApi;

public class Program {
   
   public static void Main(string[] args) {
      
      var builder = WebApplication.CreateBuilder(args);


      Console.WriteLine("Hier ist die API");
      // Add services to the container.

      // builder.Services.AddControllers();
      // // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
      // builder.Services.AddOpenApi();
      
      var app = builder.Build();

      SeedData(app);
      
      //RunTest();
      
      
      // // Configure the HTTP request pipeline.
      // if (app.Environment.IsDevelopment()) {
      //    app.MapOpenApi();
      // }
      //
      // app.UseHttpsRedirection();
      //
      // app.UseAuthorization();
      //
      // app.MapControllers();

      // app.Run();
   }

   public static void RunTest() {

      var firstname = "Bernd";
      var lastname = "Rogalla";
      var email = "b-u.rogalla@ostfalia.de";

      Customer1 c1a = new Customer1(firstname, lastname, null, email);
      
      Customer1 c1b = new Customer1(firstname, lastname, null, email);

      Customer1 c1c = c1a;
      
      Console.WriteLine($"Refernzvariable c1a {c1a.GetHashCode()}");
      Console.WriteLine(c1a.AsString());
      Console.WriteLine($"\nRefernzvariable c1b {c1b.GetHashCode()}");;
      Console.WriteLine(c1b.AsString());
      Console.WriteLine($"\nRefernzvariable c1c {c1c.GetHashCode()}");;
      Console.WriteLine(c1c.AsString());

      
      // OOP Equal
      Console.WriteLine("\nOOP Equal");
      
      Console.WriteLine($"\nc1a == c1b: {c1a == c1b}");
      Console.WriteLine($"c1a.Equals(c1b): {c1a.Equals(c1b)}");
      
      Console.WriteLine($"\nc1a == c1c: {c1a == c1c}");
      Console.WriteLine($"c1a.Equals(c1c): {c1a.Equals(c1c)}");


      // Value Objects (VO)
      var adr1 = new AddressAlt("Hauptstrasse 1", "Braunschweig", "38100");
      var adr2 = new AddressAlt("Hauptstrasse 1", "Braunschweig", "38100");
      var adr3 = adr1;
      
      Console.WriteLine("\nValue Objects Equal");
      Console.WriteLine($"\nadr1 == adr2: {adr1 == adr2}");
      Console.WriteLine($"adr1.Equals(adr2): {adr1.Equals(adr2)}");
      
      Console.WriteLine($"\nadr1 == adr3: {adr1 == adr3}");
      Console.WriteLine($"adr1.Equals(adr3): {adr1.Equals(adr3)}");
      
      
      var customer2 = Customer2.Create(firstname, lastname, null, email);
      Console.WriteLine("\nCustomer2");
      Console.WriteLine(customer2.AsString());
      
      var customer3 = Customer3.Create(firstname, lastname, null, email);
      Console.WriteLine("\nCustomer3");
      Console.WriteLine(customer3.AsString());
      
      
      
     

      
   }
   
   private static void SeedData(WebApplication app) {
      // Seed the database in development
      if (app.Environment.IsDevelopment()) {
         using var scope = app.Services.CreateScope();
         var services = scope.ServiceProvider;
         var db = services.GetRequiredService<BankingDbContext>();
         var unitOfWork = services.GetRequiredService<IUnitOfWork>();

      
         // Ensure database is created
         db.Database.EnsureCreated();
      
         // Seed if empty
         if (!db.Customers.Any()) {
            
            var seed = new Seed();
            db.Customers.AddRange(seed.Customers);
            unitOfWork.SaveAllChangesAsync("");
         }
      }
   }
   
}