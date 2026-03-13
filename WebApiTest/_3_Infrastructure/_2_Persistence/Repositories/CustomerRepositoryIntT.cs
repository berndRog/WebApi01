using Microsoft.Extensions.DependencyInjection;
using WebApi._2_Core.BuildingBlocks._1_Ports.Outbound;
using WebApi._2_Core.Customers._1_Ports.Outbound;
using WebApi._2_Core.Customers._3_Domain.Entities;
using WebApi._3_Infrastructure._2_Persistence;
using WebApi._3_Infrastructure._2_Persistence.Database;
namespace WebApiTest._3_Infrastructure._2_Persistence.Repositories;

public sealed class CustomerRepositoryIntTests : IClassFixture<TestCompositionRoot> {
   private readonly TestCompositionRoot _root;
   
   public CustomerRepositoryIntTests(TestCompositionRoot root) {
      _root = root;
   }

   [Fact]
   public async Task Add_customer_ok() {
      
      using var scope = _root.CreateDefaultScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<WebDbContext>();
      var repository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
      var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
      var seed = scope.ServiceProvider.GetRequiredService<TestSeed>();

      // Arrange
      var customer = seed.Customer1();

      // Act
      repository.Add(customer);
      await unitOfWork.SaveAllChangesAsync("Add a customer", CancellationToken.None);

      // Assert
      var actual = await dbContext.Customers.FindAsync(customer.Id);
      NotNull(actual);
      Equal(customer.Id, actual.Id);
      Equal(customer.Firstname, actual.Firstname);
      Equal(customer.Lastname, actual.Lastname);
      Equal(customer.EmailVo, actual.EmailVo);
      Equal(customer.AddressVo, actual.AddressVo);
   }


   [Fact]
   public async Task FindByIdAsync_returns_Customer1() {
      using var scope = _root.CreateDefaultScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<WebDbContext>();
      var repository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
      var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
      var seed = scope.ServiceProvider.GetRequiredService<Seed>();

      // Arrange
      var customers = seed.Customers;
      dbContext.Customers.AddRange(customers);
      await unitOfWork.SaveAllChangesAsync();
      dbContext.ChangeTracker.Clear();

      var customer = customers[0]; // Customer1
      var id = customer.Id;

      // Act
      var actual = await repository.FindByIdAsync(id, CancellationToken.None);

      // Assert
      NotNull(actual);
      Equal(id, actual!.Id);
      Equal(customer.Id, actual.Id);
      Equal(customer.Firstname, actual.Firstname);
      Equal(customer.Lastname, actual.Lastname);
      Equal(customer.EmailVo, actual.EmailVo);
      Equal(customer.AddressVo, actual.AddressVo);
   }
   
   
   [Fact]
   public async Task FindByEmailAsync_returns_Customer3() {
      using var scope = _root.CreateDefaultScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<WebDbContext>();
      var repository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
      var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
      var seed = scope.ServiceProvider.GetRequiredService<Seed>();

      // Arrange
      var customers = seed.Customers;
      dbContext.Customers.AddRange(customers);
      await unitOfWork.SaveAllChangesAsync();
      dbContext.ChangeTracker.Clear();

      var customer = customers[2]; // Customer3
      var emailVo = customer.EmailVo;

      // Act
      var actual = await repository.FindByEmailAsync(emailVo, CancellationToken.None);

      // Assert
      NotNull(actual);
      Equal(customer.Id, actual.Id);
      Equal(customer.Firstname, actual.Firstname);
      Equal(customer.Lastname, actual.Lastname);
      Equal(customer.CompanyName, actual.CompanyName);
      Equal(customer.AddressVo, actual.AddressVo);
      Equal(customer.EmailVo, actual.EmailVo);
      Equal(customer.AddressVo, actual.AddressVo);
   }
   
   [Fact]
   public async Task SelectAsync_returns_all_customers() {
      using var scope = _root.CreateDefaultScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<WebDbContext>();
      var repository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
      var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
      var seed = scope.ServiceProvider.GetRequiredService<Seed>();

      // Arrange
      dbContext.Customers.AddRange(seed.Customers);
      await unitOfWork.SaveAllChangesAsync();
      dbContext.ChangeTracker.Clear();

      // Act
      var customers = await repository.SelectAllAsync(CancellationToken.None);
      
      // Assert
      Equal(6, customers.Count()); 
      Equals(seed.Customers, customers);
   }
   
   [Fact]
   public async Task SelectByName_returns_all_customers() {
      using var scope = _root.CreateDefaultScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<WebDbContext>();
      var repository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
      var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
      var seed = scope.ServiceProvider.GetRequiredService<Seed>();

      // Arrange
      dbContext.Customers.AddRange(seed.Customers);
      await unitOfWork.SaveAllChangesAsync();
      dbContext.ChangeTracker.Clear();
      var expected = new List<Customer>() { seed.Customers[0], seed.Customers[1] };

      // Act
      var customers = await repository.SelectByDisplayNameAsync("Mustermann",CancellationToken.None);
      
      // Assert
      Equal(2, customers.Count()); 
      Equals(expected, customers);
   }


}