using WebApi._2_Core.BuildingBlocks._1_Ports.Outbound;
using WebApi._2_Core.BuildingBlocks._3_Domain.Errors;
using WebApi._2_Core.BuildingBlocks._3_Domain.ValueObjects;
using WebApi._2_Core.Customers._3_Domain.Entities;
using WebApi._2_Core.Customers._3_Domain.Errors;
using WebApi._3_Infrastructure._2_Persistence;
using WebApiTest._3_Infrastructure;
namespace WebApiTest._2_Core.Customers._3_Domain;

public sealed class CustomerUt {

   private readonly TestSeed _seed = default!;
   private readonly IClock _clock = default!;

   private readonly Guid _Id;
   private readonly string _firstname;
   private readonly string _lastname;
   private readonly string _companyName;
   private readonly string _subject;
   private readonly EmailVo _emailVo;
   private readonly string _id;
   private readonly AddressVo _addressVo = default!;

   public CustomerUt() {
      _seed = new TestSeed(
         new Seed(
            new FakeClock()
         )
      );
      _clock = _seed.Clock;
      
      _id = "11111111-0000-0000-0000-000000000000";
      _Id = Guid.Parse(_id);
      _firstname = "Bernd";
      _lastname = "Rogalla";
      _companyName = "BR Software GmbH";
      _subject = "system";
      _emailVo = EmailVo.Create("b.rogalla@mail.local").Value;
      _addressVo = _seed.Address1;
   }

   public static IEnumerable<object[]> InvalidLengths() {
      yield return new object[] { "A" };                         // too short (1)
      yield return new object[] { new string('A', 81) };         // too long (81)
   }
   
   #region--- CreatePerson tests ---------------------------
   [Fact]
   public void CreateCustomer_valid_input_and_id_creates_customer() {
      // Act
      var result = Customer.Create(
         firstname: _firstname,
         lastname: _lastname,
         companyName: null,
         emailVo: _emailVo,
         subject: _subject,
         id: _id,
         createdAt: _clock.UtcNow
      );

      // Assert
      True(result.IsSuccess);

      var customer = result.Value!;
      IsType<Customer>(customer);
      Equal(Guid.Parse(_id), customer.Id);
      Equal(_firstname, customer.Firstname);
      Equal(_lastname, customer.Lastname);
      Equal(_emailVo, customer.EmailVo);

      Null(customer.CompanyName);
   }

   [Fact]
   public void CreateCustomer_valid_input_and_without_id() {
      // Act
      var result = Customer.Create(
         firstname: _firstname,
         lastname: _lastname,
         companyName: null,
         subject: _subject,
         emailVo: _emailVo,
         id: null, // <== without id
         createdAt: _clock.UtcNow
      );

      // Assert
      True(result.IsSuccess);

      var customer = result.Value!;
      IsType<Customer>(customer);
      NotEqual(Guid.Empty, customer.Id);
      Equal(_firstname, customer.Firstname);
      Equal(_lastname, customer.Lastname);
      Equal(_emailVo, customer.EmailVo);
      Null(customer.CompanyName);
   }

   [Theory]
   [InlineData("")]
   [InlineData("   ")]
   public void CreateCustomer_invalid_firstname_fails(string firstname) {
      // Act
      var result = Customer.Create(
         firstname: firstname,
         lastname: _lastname,
         companyName: null,
         subject: _subject,
         emailVo: _emailVo,
         id: _id,
         createdAt: _clock.UtcNow
      );

      // Assert
      True(result.IsFailure);
      Equal(CustomerErrors.FirstnameIsRequired, result.Error);
   }

   [Theory]
   [MemberData(nameof(InvalidLengths))]
   public void CreateCutsomer_invalid_firstname_length_fails(string firstname) {
      var result = Customer.Create(
         firstname: firstname,
         lastname: _lastname,
         companyName: null,
         subject: _subject,
         emailVo: _emailVo,
         id: _id,
         createdAt: _clock.UtcNow
      );

      True(result.IsFailure);
      Equal(CustomerErrors.InvalidFirstname, result.Error);
   }

   [Theory]
   [InlineData("")]
   [InlineData("   ")]
   public void CreateCustomer_invalid_lastname_fails(string lastname) {
      // Act
      var result = Customer.Create(
         firstname: _firstname,
         lastname: lastname,
         companyName: null,
         subject: _subject,
         emailVo: _emailVo,
         id: _id,
         createdAt: _clock.UtcNow
      );

      // Assert
      True(result.IsFailure);
      Equal(CustomerErrors.LastnameIsRequired, result.Error);
   }

   [Theory]
   [MemberData(nameof(InvalidLengths))]
   public void CreateCustomer_invalid_lastname_length_fails(string lastname) {
      var result = Customer.Create(
         firstname: _firstname,
         lastname: lastname,
         companyName: null,
         subject: _subject,
         emailVo: _emailVo,
         id: _id,
         createdAt: _clock.UtcNow
      );

      True(result.IsFailure);
      Equal(CustomerErrors.InvalidLastname, result.Error);
   }

   [Theory]
   [InlineData("")]
   [InlineData("   ")]
   [InlineData("nonsense")]
   [InlineData("a.b.de")]
   public void CreateCustomer_invalid_email_fails(string email) {
      // Act
      var result = EmailVo.Create(email);
      // Assert
      True(result.IsFailure);
      // depending on your VO implementation this might be EmailIsRequired or CommonErrors.InvalidEmail
      // We assert failure is enough for teaching; refine if you want strict error matching.
   }

   [Fact]
   public void CreateCustomer_with_valid_id_string_sets_id() {
      // Arrange
      var id = "11111111-1111-1111-1111-111111111111";

      // Act
      var result = Customer.Create(
         firstname: _firstname,
         lastname: _lastname,
         companyName: null,
         subject: _subject,
         emailVo: _emailVo,
         id: id,
         createdAt: _clock.UtcNow
      );

      // Assert
      True(result.IsSuccess);
      Equal(Guid.Parse(id), result.Value!.Id);
   }

   [Fact]
   public void CreateCustomer_invalid_id_should_fail() {
      // Arrange
      var id = "not-a-guid";

      // Act
      var result = Customer.Create(
         firstname: _firstname,
         lastname: _lastname,
         companyName: null,
         subject: _subject,
         emailVo: _emailVo,
         id: id,
         createdAt: _clock.UtcNow
      );

      // Assert
      True(result.IsFailure);
      Equal(CustomerErrors.InvalidId, result.Error);
   }

   #endregion

   #region--- CreateCustomer with Address tests ---------------------------
   [Fact]
   public void CreateCustomer_valid_input_and_id_and_address() {
      // Act
      var result = Customer.Create(
         firstname: _firstname,
         lastname: _lastname,
         companyName: null,         
         subject: _subject,
         emailVo: _emailVo,
         id: _id,
         createdAt: _clock.UtcNow,
         addressVo: _addressVo
      );

      // Assert
      True(result.IsSuccess);

      var customer = result.Value!;
      Equal(Guid.Parse(_id), customer.Id);
      NotNull(customer.AddressVo);
      Equal(_addressVo.Street, customer.AddressVo!.Street);
      Equal(_addressVo.PostalCode, customer.AddressVo!.PostalCode);
      Equal(_addressVo.City, customer.AddressVo!.City);
      Equal(_addressVo.Country, customer.AddressVo!.Country);
   }

   [Theory]
   [InlineData("")]
   [InlineData("   ")]
   [MemberData(nameof(InvalidLengths))]
   public void CreateCustomer_with_address_invalid_street_fails(string street) {
      // Act      
      var ResultAddress = AddressVo.Create(
         street: street,
         postalCode: _addressVo.PostalCode,
         city: _addressVo.City,
         country: _addressVo.Country
      );
      
      // Assert
      True(ResultAddress.IsFailure);
      if(string.IsNullOrWhiteSpace(street))
         Equivalent(CommonErrors.StreetIsRequired, ResultAddress.Error);
      else
         Equal(CommonErrors.InvalidStreet, ResultAddress.Error);

   }

   [Theory]
   [InlineData("")]
   [InlineData("   ")]
   [InlineData("A")]
   [InlineData("AAAAAAAAAAA")]
   public void CreateCustomer_with_address_invalid_postal_code_fails(string postalCode) {
      // Act      
      var ResultAddress = AddressVo.Create(
         street: _addressVo.Street,
         postalCode: postalCode,
         city: _addressVo.City,
         country: _addressVo.Country
      );
      
      // Assert
      True(ResultAddress.IsFailure);
      if(string.IsNullOrWhiteSpace(postalCode))
         Equivalent(CommonErrors.PostalCodeIsRequired, ResultAddress.Error);
      else
         Equal(CommonErrors.InvalidPostalCode, ResultAddress.Error);
      
   }

   [Theory]
   [InlineData("")]
   [InlineData("   ")]
   [MemberData(nameof(InvalidLengths))]
   public void CreateCustomer_with_address_invalid_city_fails(string city) {
      // Act      
      var ResultAddress = AddressVo.Create(
         street: _addressVo.Street,
         postalCode: _addressVo.PostalCode,
         city: city,
         country: _addressVo.Country
      );
      
      // Assert
      True(ResultAddress.IsFailure);
      if(string.IsNullOrWhiteSpace(city))
         Equivalent(CommonErrors.CityIsRequired, ResultAddress.Error);
      else
         Equal(CommonErrors.InvalidCity, ResultAddress.Error);
   }
   #endregion
   
   #region --- CreateCompany tests ---------------------------
   [Fact]
   public void CreateCompany_valid_input_and_without_id() {
      // Act
      var result = Customer.Create(
         firstname: _firstname,
         lastname: _lastname,
         companyName: _companyName,
         subject: _subject,
         emailVo: _emailVo,
         id: null,
         createdAt: _clock.UtcNow
      );

      // Assert
      True(result.IsSuccess);
      var customer = result.Value!;
      NotEqual(Guid.Empty, customer.Id);
      Equal(_firstname, customer.Firstname);
      Equal(_lastname, customer.Lastname);
      Equal(_companyName, customer.CompanyName);
      Equal(_emailVo, customer.EmailVo);
   }

   [Theory]
   [InlineData("")]
   [InlineData("   ")]
   public void CreateCompany_invalid_firstname_fails(string firstname) {
      var result = Customer.Create(
         firstname: firstname,
         lastname: _lastname,
         companyName: _companyName,
         subject: _subject,
         emailVo: _emailVo,
         id: null,
         createdAt: _clock.UtcNow
      );

      True(result.IsFailure);
      Equivalent(CustomerErrors.FirstnameIsRequired, result.Error);
   }

   [Theory]
   [InlineData("")]
   [InlineData("   ")]
   public void CreateCompany_invalid_lastname_fails(string lastname) {
      var result = Customer.Create(
         firstname: _firstname,
         lastname: lastname,
         companyName: _companyName,
         subject: _subject,
         emailVo: _emailVo,
         id: null,
         createdAt: _clock.UtcNow
      );

      True(result.IsFailure);
      Equal(CustomerErrors.LastnameIsRequired, result.Error);
   }

   [Theory]
   [MemberData(nameof(InvalidLengths))]
   public void CreateComnay_invalid_companyName_length_fails(string companyName) {
       var result = Customer.Create(
         firstname: _firstname,
         lastname: _lastname,
         companyName: companyName,
         subject: _subject,
         emailVo: _emailVo,
         id: null,
         createdAt: _clock.UtcNow
      );
       
      True(result.IsFailure);
      Equal(CustomerErrors.InvalidCompanyName, result.Error);
   }

   
   [Theory]
   [InlineData("")]
   [InlineData("   ")]
   [InlineData("nonsense")]
   [InlineData("a.b.de")]
   public void CreateCompany_invalid_email_fails(string email) {
      // Act
      var result = EmailVo.Create(email);
      // Assert
      True(result.IsFailure);
   }

   [Fact]
   public void CreateCompany_with_valid_id_string_sets_id() {
      var id = "22222222-2222-2222-2222-222222222222";

      var result = Customer.Create(
         firstname: _firstname,
         lastname: _lastname,
         companyName: _companyName,
         subject: _subject,
         emailVo: _emailVo,
         id: id,
         createdAt: _clock.UtcNow
      );

      True(result.IsSuccess);
      Equal(Guid.Parse(id), result.Value!.Id);
   }

   [Fact]
   public void CreateCompany_invalid_id_should_fail() {
      var id = "not-a-guid";

      var result = Customer.Create(
         firstname: _firstname,
         lastname: _lastname,
         companyName: _companyName,
         subject: _subject,
         emailVo: _emailVo,
         id: id,
         createdAt: _clock.UtcNow
      );

      True(result.IsFailure);
      Equivalent(CustomerErrors.InvalidId, result.Error);
   }

   #endregion
   

   #region --- ChangeEmail tests ---------------------------
   /*
   [Fact]
   public void ChangeEmail_valid_updates_email_and_updatedAt() {
      // Arrange
      var customer = Customer.Create(
         firstname: _firstname,
         lastname: _lastname,
         companyName: null,
         email: _email,
         id: _id
      ).Value!;

      var now = _seed.UtcNow.AddDays(1);
      var newEmail = "new.mail@domain.de";

      // Act
      var result = customer.ChangeEmail(newEmail, now);

      // Assert
      True(result.IsSuccess);
      Equal(newEmail, customer.Email);
      Equal(now, customer.UpdatedAt);
   }

   [Fact]
   public void ChangeEmail_now_default_fails() {
      var customer = Customer.Create(
         firstname: _firstname,
         lastname: _lastname, 
         companyName: null, 
         email: _email, 
         id: _id
      ).Value!;

      var result = customer.ChangeEmail("new.mail@domain.de", utcNow: default);

      True(result.IsFailure);
      Equal(CommonErrors.TimestampIsRequired, result.Error);
   }
   */
   #endregion
}
