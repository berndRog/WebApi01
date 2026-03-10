using WebApi._2_Modules.BuildingBlocks._1_Ports.Outbound;
namespace WebApi._3_Infrastructure._2_Persistence.Database;

internal class SeedDatabase(
   WebDbContext  dbContext,
   IUnitOfWork unitOfWork,
   IClock clock
): ISeedDatabase {


   public void Run() {
      // Ensure database is created
      dbContext.Database.EnsureCreated();
      
      // Seed if empty
      if (!dbContext.Customers.Any()) {
            
         var seed = new Seed(clock);
         dbContext.Customers.AddRange(seed.Customers);
         unitOfWork.SaveAllChangesAsync("");
      }
   }

}