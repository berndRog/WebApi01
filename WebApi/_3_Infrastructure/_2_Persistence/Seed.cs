using System.Security.Cryptography.X509Certificates;
using WebApi._2_Core.BuildingBlocks._1_Ports.Outbound;
using WebApi._2_Core.BuildingBlocks._3_Domain.ValueObjects;
using WebApi._2_Core.Customers._3_Domain.Entities;
namespace WebApi._3_Infrastructure._2_Persistence;

public sealed class Seed(
   IClock clock
) {
   public IClock Clock => clock;
   
   #region -------------- Test Addresses (Value Objects) -------------------------------------
   public AddressVo Address1
      => AddressVo.Create("Hauptstr. 23", "29556", "Suderburg", "DE").GetValueOrThrow();
   public AddressVo Address2
      => AddressVo.Create("Bahnhofstr.10", "10115", "Berlin").GetValueOrThrow();
   public AddressVo Address3
      => AddressVo.Create("Schillerstr. 1", "30123", "Hannover", "DE").GetValueOrThrow();
   #endregion

   #region -------------- Test Customers (Entities) ------------------------------------------
   public string customer1Id = "10000000-0000-0000-0000-000000000000";
   public string customer2Id = "20000000-0000-0000-0000-000000000000";
   public string customer3Id = "30000000-0000-0000-0000-000000000000";
   public string customer4Id = "40000000-0000-0000-0000-000000000000";
   public string customer5Id = "50000000-0000-0000-0000-000000000000";
   public string customer6Id = "60000000-0000-0000-0000-000000000000";
   
   public Customer Customer1() => CreateCustomer(
      id: customer1Id,
      firstname: "Erika",
      lastname: "Mustermann",
      companyName: null,
      emailString: "erika.mustermann@t-online.de",
      subject: "a00090ad-d9df-486a-8757-4a649e26a54e",
      addressVo: Address1
   );

   public Customer Customer2() => CreateCustomer(
      id: customer2Id,
      firstname: "Max",
      lastname: "Mustermann",
      companyName: null,
      emailString: "max.mustermann@gmail.com",
      subject: "b0000640-161e-4228-9729-d6b142C2dfad",
      addressVo: null
   );

   public Customer Customer3() => CreateCustomer(
      id: customer3Id,
      firstname: "Arno",
      lastname: "Arndt",
      companyName: null,
      emailString: "a.arndt@t-online.com",
      subject: "c0004e61-ba7a-4d2a-977f-766b42bb79a9",
      addressVo: Address2
   );

   public Customer Customer4() => CreateCustomer(
      id: customer4Id,
      firstname: "Benno",
      lastname: "Bauer",
      companyName: null,
      emailString: "b.bauer@gmail.com",
      subject: "d0024ab-43c5-4c64-872d-6ca05f66756b",
      addressVo: null
   );

   public Customer Customer5() => CreateCustomer(
      id: customer5Id,
      firstname: "Christine",
      lastname: "Conrad",
      companyName: "Conrad Consulting GmbH",
      emailString: "c.conrad@gmx.de",
      subject: "e00050fb-a381-4e3f-a44b-81ffa7610b72",
      addressVo: Address3
   );

   public Customer Customer6() => CreateCustomer(
      id: customer6Id,
      firstname: "Dana",
      lastname: "Deppe",
      companyName: null,
      emailString: "d.deppe@icloud.com",
      subject: "f0004f67-72a3-4449-af1f-803dcfaddb7f",
      addressVo: null
   );
   public IReadOnlyList<Customer> Customers => [
      Customer1(), Customer2(), Customer3(), Customer4(), Customer5(), Customer6()
   ];
   #endregion

   #region -------------- Test Iban (Value Objects) ------------------------------------------
   // public IbanVo Iban1Vo
   //    => IbanVo.Create("DE10 1000 0000 0000 0000 42").GetValueOrThrow();
   // public IbanVo Iban2Vo
   //    => IbanVo.Create("DE10 2000 0000 0000 0000 04").GetValueOrThrow();
   // public IbanVo Iban3Vo
   //    => IbanVo.Create("DE20 1000 0000 0000 0000 56").GetValueOrThrow();
   // public IbanVo Iban4Vo
   //    => IbanVo.Create("DE30 1000 0000 0000 0000 70").GetValueOrThrow();
   // public IbanVo Iban5Vo
   //    => IbanVo.Create("DE40 1000 0000 0000 0000 84").GetValueOrThrow();
   // public IbanVo Iban6Vo
   //    => IbanVo.Create("DE50 1000 0000 0000 0000 01").GetValueOrThrow();
   // public IbanVo Iban7Vo
   //    => IbanVo.Create("DE50 2000 0000 0000 0000 60").GetValueOrThrow();
   // public IbanVo Iban8Vo
   //    => IbanVo.Create("DE60 1000 0000 0000 0000 15").GetValueOrThrow();
   #endregion

   #region -------------- Test Accounts (Entities) -------------------------------------------
   public string account1Id = "01000000-0000-0000-0000-000000000000";
   public string account2Id = "02000000-0000-0000-0000-000000000000";
   public string account3Id = "03000000-0000-0000-0000-000000000000";
   public string account4Id = "04000000-0000-0000-0000-000000000000";
   public string account5Id = "05000000-0000-0000-0000-000000000000";
   public string account6Id = "06000000-0000-0000-0000-000000000000";
   public string account7Id = "07000000-0000-0000-0000-000000000000";
   public string account8Id = "08000000-0000-0000-0000-000000000000";
   
   
   // public Account Account1() => CreateAccount(
   //    id: account1Id,
   //    customerId: Guid.Parse(customer1Id),
   //    ibanVo: Iban1Vo,
   //    balanceDecimal: 2100.0m
   // );
   //
   // public Account Account2() => CreateAccount(
   //    id: account2Id,
   //    customerId: Guid.Parse(customer1Id),
   //    ibanVo: Iban2Vo,
   //    balanceDecimal: 2000.0m
   // );
   //
   // public Account Account3() => CreateAccount(
   //    id: account3Id,
   //    customerId: Guid.Parse(customer2Id),
   //    ibanVo: Iban3Vo,
   //    balanceDecimal: 3000.0m
   // );
   //
   // public Account Account4() => CreateAccount(
   //    id: account4Id,
   //    customerId: Guid.Parse(customer3Id),
   //    ibanVo: Iban4Vo,
   //    balanceDecimal: 2500.0m
   // );
   //
   // public Account Account5() => CreateAccount(
   //    id: account5Id,
   //    customerId: Guid.Parse(customer4Id),
   //    ibanVo: Iban5Vo,
   //    balanceDecimal: 1900.0m
   // );
   //
   // public Account Account6() => CreateAccount(
   //    id: account6Id,
   //    customerId: Guid.Parse(customer5Id),
   //    ibanVo: Iban6Vo,
   //    balanceDecimal: 3500.0m
   // );
   //
   // public Account Account7() => CreateAccount(
   //    id: account7Id,
   //    customerId: Guid.Parse(customer5Id),
   //    ibanVo: Iban7Vo,
   //    balanceDecimal: 3100.0m
   // );
   //
   // public Account Account8() => CreateAccount(
   //    id: account8Id,
   //    customerId: Guid.Parse(customer6Id),
   //    ibanVo: Iban8Vo,
   //    balanceDecimal: 4300.0m
   // );
   // public IReadOnlyList<Account> Accounts => [
   //    Account1(), Account2(), Account3(), Account4(),
   //    Account5(), Account6(), Account7(), Account8(),
   // ];
   #endregion
   

   // ---------- Helper ----------
   private Customer CreateOwner(
      string id,
      string firstname,
      string lastname,
      string? companyName,
      string subject,
      string emailString,
      AddressVo? addressVo
   ) {
      
      var resultEmail = EmailVo.Create(emailString);
      if (resultEmail.IsFailure)
         throw new Exception($"Invalid email in seed data: {emailString}");
      var email = resultEmail.Value;
      
      var result = Customer.Create(
         firstname: firstname,
         lastname: lastname,
         companyName: companyName,
         subject: subject,
         emailVo: email,
         id: id,
         createdAt:clock.UtcNow,
         addressVo: addressVo
      );
      return result.Value!;
   }
   
      // ---------- Helper ----------
   // private Employee CreateEmployee(
   //    string id,
   //    string firstname,
   //    string lastname,
   //    string emailString,
   //    string phoneString,
   //    string subject,
   //    string personnelNumber,
   //    AdminRights adminRights
   // ) {
   //    var resultEmail = EmailVo.Create(emailString);
   //    if (resultEmail.IsFailure)
   //       throw new Exception($"Invalid email in test seed: {emailString}");
   //    var email = resultEmail.Value;
   //
   //    var resultPhone = PhoneVo.Create(phoneString);
   //    if (resultPhone.IsFailure)
   //       throw new Exception($"Invalid phone number in test seed: {phoneString}");
   //    var phone = resultPhone.Value;
   //
   //    var result = Employee.Create(
   //       firstname: firstname,
   //       lastname: lastname,
   //       emailVo: email,
   //       phone: phone,
   //       subject: subject,
   //       personnelNumber: personnelNumber,
   //       adminRights: adminRights,
   //       id: id
   //    );
   //    return result.Value!;
   // }

   private Customer CreateCustomer(
      string id,
      string firstname,
      string lastname,
      string? companyName,
      string emailString,
      string subject,
      AddressVo? addressVo
   ) {
      var resultEmail = EmailVo.Create(emailString);
      if (resultEmail.IsFailure)
         throw new Exception($"Invalid email in test seed: {emailString}");
      var emailVo = resultEmail.Value;

      var result = Customer.Create(
         firstname: firstname,
         lastname: lastname,
         companyName: companyName,
         emailVo: emailVo,
         subject: subject,
         id: id,
         createdAt: clock.UtcNow,
         addressVo: addressVo
      );

      return result.Value!;
   }

   // private Account CreateAccount(
   //    Guid customerId,
   //    string id,
   //    IbanVo ibanVo,
   //    decimal balanceDecimal
   // ) {
   //    var resultBalance = MoneyVo.Create(balanceDecimal, Currency.EUR);
   //    if (resultBalance.IsFailure)
   //       throw new Exception($"Invalid balance in test seed: {balanceDecimal}");
   //    var balance = resultBalance.Value;
   //
   //    var result = Account.Create(
   //       customerId: customerId,
   //       ibanVo: ibanVo,
   //       balanceVo: balance,
   //       createdAt: _clock.UtcNow,
   //       id: id
   //    );
   //    return result.Value!;
   // }

   // private Beneficiary CreateBeneficiary(
   //    string id,
   //    Guid accountId,
   //    string name,
   //    IbanVo ibanVo
   // ) {
   //    var result = Beneficiary.Create(
   //       accountId: accountId,
   //       name: name,
   //       ibanVo: ibanVo,
   //       id: id
   //    );
   //    return result.Value!;
   // }
}