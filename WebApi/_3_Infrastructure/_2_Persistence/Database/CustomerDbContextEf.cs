using System.Runtime.CompilerServices;
using WebApi._2_Core.Customers._1_Ports.Outbound;
using WebApi._2_Core.Customers._3_Domain.Entities;
[assembly: InternalsVisibleTo("WebApiTest")]
namespace WebApi._3_Infrastructure._2_Persistence.Database;

internal sealed class CustomerDbContextEf(
   WebDbContext db
) : ICustomerDbContext {
   
   public IQueryable<Customer> Customers 
      => db.Set<Customer>();

   public void Add(Customer customer) 
      => db.Set<Customer>().Add(customer);
   public void AddRange(IEnumerable<Customer> entities) 
      => db.Set<Customer>().AddRange(entities);
   public void Update(Customer customer)
      => db.Set<Customer>().Update(customer);
}

