using BankingApi._4_BuildingBlocks._1_Ports.Inbound;
using WebApi._2_Modules.Customers._3_Domain.Entities;
using WebApi._2_Modules.Customers._3_Domain.ValueObjects;
namespace WebApi._3_Infrastructure._2_Persistence;

public sealed class Seed {

   private IClock _clock = default!;
   
   // ---------- Test data for addresses ----------
   public Address Address1 => Address.Create("Hauptstr. 23", "29556", "Suderburg", "DE").GetValueOrThrow();
   public Address Address2 =>  Address.Create("Bahnhofstr.10", "10115", "Berlin").GetValueOrThrow();
   public Address Address3 => Address.Create("Schillerstr. 1", "30123", "Hannover", "DE").GetValueOrThrow();
   
   public Customer Customer1() => CreateOwner(
      id: "00000001-0000-0000-0000-000000000000",
      firstname: "Erika",
      lastname: "Mustermann",
      companyName: null,
      emailString: "erika.mustermann@t-online.de",
      addressVo: Address1
  );
   public Customer Customer2() => CreateOwner(
      id: "00000002-0000-0000-0000-000000000000",
      firstname: "Max",
      lastname: "Mustermann",
      companyName: null,
      emailString: "max.mustermann@gmail.com",
      null
      );
   public Customer Customer3() => CreateOwner(
      id: "00000003-0000-0000-0000-000000000000",
      firstname: "Arno",
      lastname: "Arndt",
      companyName: null,
      emailString: "a.arndt@t-online.com",
      addressVo: Address2
   );

   public Customer Customer4() => CreateOwner(
      id: "00000004-0000-0000-0000-000000000000",
      firstname: "Benno",
      lastname: "Bauer",
      companyName: null,
      emailString: "b.bauer@gmail.com",
      null
   );
   
   public Customer Customer5() => CreateOwner(
      id: "00000005-0000-0000-0000-000000000000",
      firstname: "Christine",
      lastname: "Conrad",
      companyName: "Conrad Consulting GmbH",
      emailString: "c.conrad@gmx.de",
      addressVo: Address3
   );

   public Customer Customer6() => CreateOwner(
      id: "00000006-0000-0000-0000-000000000000",
      firstname: "Dana",
      lastname: "Deppe",
      companyName: null,
      "d.deppe@icloud.com",
      addressVo: null
   );
   
   public IReadOnlyList<Customer> Customers => [
      Customer1(), Customer2(), Customer3(), Customer4(), Customer5(), Customer6()
   ];
   

   // ---------- Helper ----------
   private Customer CreateOwner(
      string id,
      string firstname,
      string lastname,
      string? companyName,
      string emailString,
      Address? addressVo
   ) {
      
      var resultEmail = Email.Create(emailString);
      if (resultEmail.IsFailure)
         throw new Exception($"Invalid email in seed data: {emailString}");
      var email = resultEmail.Value;
      
      var result = Customer.Create(
         firstname: firstname,
         lastname: lastname,
         companyName: companyName,
         emailVo: email,
         id: id,
         addressVo: addressVo
      );
      return result.Value!;
   }
}