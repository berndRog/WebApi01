using System.Runtime.CompilerServices;
using WebApi._2_Core.Customers._1_Ports.Outbound;
using WebApi._2_Core.Customers._3_Domain.Entities;
[assembly: InternalsVisibleTo("WebApiTest")]
namespace WebApi._3_Infrastructure._2_Persistence.Database;

internal sealed class CustomerDbContextEf(
   WebDbContext dbContext
) : ICustomerDbContext {
   
   public IQueryable<Customer> Customers => dbContext.Set<Customer>();

   public void Add(Customer customer) => dbContext.Customers.Add(customer);
   public void Update(Customer customer) => dbContext.Customers.Update(customer);
   
   // public void Add<T>(T entity) where T : class => dbContext.Set<T>().Add(entity);
   // public void Update<T>(T entity) where T : class => dbContext.Set<T>().Update(entity);
   // public void Remove<T>(T entity) where T : class => dbContext.Set<T>().Remove(entity);
}

