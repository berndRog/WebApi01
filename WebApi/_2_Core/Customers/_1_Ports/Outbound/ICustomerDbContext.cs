using WebApi._2_Core.Customers._3_Domain.Entities;
namespace WebApi._2_Core.Customers._1_Ports.Outbound;

public interface ICustomerDbContext {
   IQueryable<Customer> Customers { get; }
   void Add(Customer entity);
   void Update(Customer entity);
}
