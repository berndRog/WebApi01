using System.Runtime.CompilerServices;
using WebApi._2_Modules.Customers._1_Ports.Outbound;
using WebApi._2_Modules.Customers._3_Domain.Entities;
[assembly: InternalsVisibleTo("WebApiTest")]
namespace WebApi._3_Infrastructure._2_Persistence.Database;

internal sealed class CustomersDbContextEf(
   WebDbContext db
) : ICustomersDbContext {
   
   public IQueryable<Customer> Customers => db.Set<Customer>();

   public void Add<T>(T entity) where T : class => db.Set<T>().Add(entity);
   public void Remove<T>(T entity) where T : class => db.Set<T>().Remove(entity);

}