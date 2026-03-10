using WebApi._2_Core.BuildingBlocks._1_Ports.Outbound;
using WebApi._2_Core.BuildingBlocks._3_Domain.ValueObjects;
using WebApi._2_Core.Customers._3_Domain.Entities;
namespace WebApi._3_Infrastructure._2_Persistence;

public sealed class Seed(
   IClock clock
) {
   
   // ---------- Test data for addresses ----------
   public AddressVo Address1 => AddressVo.Create("Hauptstr. 23", "29556", "Suderburg", "DE").GetValueOrThrow();
   public AddressVo Address2 =>  AddressVo.Create("Bahnhofstr.10", "10115", "Berlin").GetValueOrThrow();
   public AddressVo Address3 => AddressVo.Create("Schillerstr. 1", "30123", "Hannover", "DE").GetValueOrThrow();
   
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
         email: email,
         id: id,
         createdAt:clock.UtcNow,
         address: addressVo
      );
      return result.Value!;
   }
}