using BankingApiTest.TestInfrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WebApi._2_Core.BuildingBlocks._1_Ports.Outbound;
using WebApi._2_Core.BuildingBlocks._3_Domain.ValueObjects;
using WebApi._2_Core.Customers._1_Ports.Outbound;
using WebApi._2_Core.Customers._3_Domain.Entities;
using WebApi._3_Infrastructure._2_Persistence;
using WebApi._3_Infrastructure._2_Persistence.Database;
using WebApiTest.TestInfrastructure;
namespace WebApiTest._3_Infrastructure._2_Persistence.Repositories;

public sealed class CustomerRepositoryIntT : IClassFixture<TestCompositionRoot>, IAsyncLifetime {
   private readonly TestCompositionRoot _root;
   private static CancellationToken Ct => TestContext.Current.CancellationToken;
   
   public CustomerRepositoryIntT(TestCompositionRoot root) {
      _root = root;
   }

   public async ValueTask InitializeAsync() {
      using var scope = _root.CreateDefaultScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<WebDbContext>();
      await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM Customers;", Ct);
      dbContext.ChangeTracker.Clear();
   }

   public ValueTask DisposeAsync() => ValueTask.CompletedTask;

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
      await unitOfWork.SaveAllChangesAsync(ctToken: Ct);
      dbContext.ChangeTracker.Clear();

      var customer = customers[0]; // Customer1
      var id = customer.Id;

      // Act
      var actual = await repository.FindByIdAsync(id, Ct);

      // Assert
      NotNull(actual);
      Equal(id, actual!.Id);
      Equal(customer.Id, actual.Id);
      Equal(customer.Firstname, actual.Firstname);
      Equal(customer.Lastname, actual.Lastname);
      Equal(customer.CompanyName, actual.CompanyName);
      Equal(customer.DisplayName, actual.DisplayName);
      Equal(customer.Subject, actual.Subject);
      Equal(customer.Status, actual.Status);
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
      await unitOfWork.SaveAllChangesAsync(ctToken: Ct);
      dbContext.ChangeTracker.Clear();

      var customer = customers[2]; // Customer3
      var emailVo = customer.EmailVo;

      // Act
      var actual = await repository.FindByEmailAsync(emailVo, Ct);

      // Assert
      NotNull(actual);
      Equal(customer.Id, actual.Id);
      Equal(customer.Firstname, actual.Firstname);
      Equal(customer.Lastname, actual.Lastname);
      Equal(customer.CompanyName, actual.CompanyName);
      Equal(customer.DisplayName, actual.DisplayName);
      Equal(customer.Subject, actual.Subject);
      Equal(customer.Status, actual.Status);
      Equal(customer.EmailVo, actual.EmailVo);
      Equal(customer.AddressVo, actual.AddressVo);
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
      await unitOfWork.SaveAllChangesAsync(ctToken: Ct);
      dbContext.ChangeTracker.Clear();
      var expected = new List<Customer>() { seed.Customers[0], seed.Customers[1] };

      // Act
      var customers = (await repository.SelectByNameAsync("Mustermann", Ct)).ToList();
      
      // Assert
      Equal(2, customers.Count()); 
      Equal(expected.Select(c => c.Id), customers.Select(c => c.Id));
   }
   
   [Fact]
   public async Task ExistsActiveAsync_returns_Customer1() {
      using var scope = _root.CreateDefaultScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<WebDbContext>();
      var repository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
      var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
      var seed = scope.ServiceProvider.GetRequiredService<Seed>();

      // Arrange
      var customers = seed.Customers;
      dbContext.Customers.AddRange(customers);
      await unitOfWork.SaveAllChangesAsync(ctToken: Ct);
      dbContext.ChangeTracker.Clear();

      var customer = customers[0]; // Customer1
      var id = customer.Id;

      // Act
      var found = await repository.ExistsActiveAsync(id, Ct);

      // Assert
      True(found);
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
      await unitOfWork.SaveAllChangesAsync(ctToken: Ct);
      dbContext.ChangeTracker.Clear();

      // Act
      var customers = (await repository.SelectAllAsync(Ct)).ToList();
      
      // Assert
      Equal(6, customers.Count()); 
      Equal(seed.Customers.Select(c => c.Id), customers.Select(c => c.Id));
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
      await unitOfWork.SaveAllChangesAsync("Add a customer", Ct);

      // Assert
      var actual = await dbContext.Customers.FindAsync([customer.Id], Ct);
      NotNull(actual);
      Equal(customer.Id, actual.Id);
      Equal(customer.Firstname, actual.Firstname);
      Equal(customer.Lastname, actual.Lastname);
      Equal(customer.CompanyName, actual.CompanyName);
      Equal(customer.DisplayName, actual.DisplayName);
      Equal(customer.Subject, actual.Subject);
      Equal(customer.Status, actual.Status);
      Equal(customer.EmailVo, actual.EmailVo);
      Equal(customer.AddressVo, actual.AddressVo);
   }


   [Fact]
   public async Task Update_customer_ok() {
      using var scope = _root.CreateDefaultScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<WebDbContext>();
      var repository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
      var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
      var seed = scope.ServiceProvider.GetRequiredService<TestSeed>();

      // Arrange
      var customer = seed.Customer1();
      repository.Add(customer);
      await unitOfWork.SaveAllChangesAsync("Add a customer", Ct);
      unitOfWork.ClearChangeTracker();
      
      // Act
      var customerUpdate = await repository.FindByIdAsync(customer.Id, Ct);
      NotNull(customerUpdate);
      var customerToUpdate = customerUpdate!;
      customerToUpdate.Update(
         lastname: "Meier",
         companyName: null,
         emailVo: EmailVo.Create("e.meier@gmx.de").GetValueOrThrow(),
         addressVo: AddressVo.Create(
            street: "Allertalweg. 2",
            postalCode: "29227",
            city: "Celle",
            country: "DE"
         ).GetValueOrThrow(),
         updatedAt: seed.Clock.UtcNow.AddDays(7).AddHours(5)
      );
      
      repository.Update(customerToUpdate);   
      var records = await unitOfWork.SaveAllChangesAsync("Update a customer", Ct);
      Equal(1, records);
      
      // Assert
      var actual = await dbContext.Customers.FindAsync([customer.Id], Ct);
      NotNull(actual);
      Equal(customerToUpdate.Id, actual.Id);
      Equal(customerToUpdate.Firstname, actual.Firstname);
      Equal(customerToUpdate.Lastname, actual.Lastname);
      Equal(customerToUpdate.CompanyName, actual.CompanyName);
      Equal(customerToUpdate.DisplayName, actual.DisplayName);
      Equal(customerToUpdate.Subject, actual.Subject);
      Equal(customerToUpdate.Status, actual.Status);
      Equal(customerToUpdate.EmailVo, actual.EmailVo);
      Equal(customerToUpdate.AddressVo, actual.AddressVo);
   }


}