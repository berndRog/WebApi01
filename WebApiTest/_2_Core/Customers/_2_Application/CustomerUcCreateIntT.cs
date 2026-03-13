using Microsoft.Extensions.DependencyInjection;
using WebApi._2_Core.BuildingBlocks._1_Ports.Outbound;
using WebApi._2_Core.Customers._1_Ports.Outbound;
using WebApi._2_Core.Customers._2_Application.Mappings;
using WebApi._2_Core.Customers._2_Application.UseCases;
using WebApi._3_Infrastructure._2_Persistence.Database;
using WebApiTest._3_Infrastructure;
using WebApiTest._3_Infrastructure._3_Security;
namespace WebApiTest._2_Core.Customers._2_Application;

public sealed class CustomerUcCreateIntT : IClassFixture<TestCompositionRoot> {
   private readonly TestCompositionRoot _root;
   private readonly CancellationToken _ct = CancellationToken.None;

   public CustomerUcCreateIntT(TestCompositionRoot root) {
      _root = root;
   }

   [Fact]
   public async Task Create_customer_ok() {
      // build container with override
      await using var provider = _root.CreateCustomProvider(services => {
         TestCompositionRoot.ReplaceScoped<IIdentityGateway>(services, _ =>
            new FakeIdentityGateway(
               subject: "a00090ad-d9df-486a-8757-4a649e26a54e",
               username: "e.mustermann@t-online.de",
               createdAt: DateTimeOffset.UtcNow.AddDays(-10),
               adminRights: null
            )
         );
      });

      using var scope = provider.CreateScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<WebDbContext>();
      var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
      // var accountRepository = scope.ServiceProvider.GetRequiredService<IAccountRepository>();
      var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
      var seed = scope.ServiceProvider.GetRequiredService<TestSeed>();
      var sut = scope.ServiceProvider.GetRequiredService<CustomerUcCreate>();

      // Arrange
      var customer1 = seed.Customer1();
      var customer1Dto = customer1.ToCustomerDto();
      //var account1 = seed.Account1(); // for owner1, but not required for this test, as account creation is not part of this use case

      // Act
      await sut.ExecuteAsync(
         customerDto: customer1Dto,
         //accountIdString: account1.Id.ToString(),
         //ibanString: account1.IbanVo.Value,
         _ct
      );
      dbContext.ChangeTracker.Clear();

      // Assert
      var actualCustomer = await customerRepository.FindByIdAsync(customer1.Id, _ct);
      NotNull(actualCustomer);
      Equal(customer1.Id, actualCustomer!.Id);
      Equal(customer1.Firstname, actualCustomer.Firstname);
      Equal(customer1.Lastname, actualCustomer.Lastname);
      Equal(customer1.EmailVo, actualCustomer.EmailVo);
      Equal(customer1.Subject, actualCustomer.Subject);
      Equal(customer1.AddressVo, actualCustomer.AddressVo);
      // var actualAccounts = await accountRepository.SelelctByCustomerIdAsync(customer1.Id, _ct);
      // NotNull(actualAccounts);
      // var actualAccount = actualAccounts!.SingleOrDefault(a => a.Id == account1.Id);
      // NotNull(actualAccount);
   }
}