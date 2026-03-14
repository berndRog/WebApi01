using WebApi._2_Core.BuildingBlocks._3_Domain;
using WebApi._2_Core.BuildingBlocks._3_Domain.Entities;
using WebApi._2_Core.BuildingBlocks._3_Domain.Errors;
using WebApi._2_Core.BuildingBlocks._3_Domain.ValueObjects;
using WebApi._2_Core.Customers._3_Domain.Enums;
using WebApi._2_Core.Customers._3_Domain.Errors;
namespace WebApi._2_Core.Customers._3_Domain.Entities;

public class Customer : AggregateRoot {
   
   //--- properties ------------------------------------------------------------
   // is inherited from Entity base class
   // public Guid Id { get; private set; } 
   // is inherited from Aggregate root base class
   // public DateTimeOffset CreatedAt { get; private set; }
   // public DateTimeOffset UpdatedAt { get; private set; }
   public string Firstname { get; private set; } = string.Empty;
   public string Lastname { get; private set; } = string.Empty;
   public string? CompanyName { get; private set; } = null;
   
   // Display name used in UIs and documents (derived, not persisted)
   public string DisplayName => CompanyName ?? $"{Firstname} {Lastname}";
   
   // Subject identifier from the identity provider (OIDC / OAuth)
   public string Subject { get; private set; } = default!;
   
   // Status (business lifecycle)
   public CustomerStatus Status { get; private set; } = CustomerStatus.Pending;
   
   // Employee decisions (audit facts)
   public DateTimeOffset? ActivatedAt { get; private set; }
   public DateTimeOffset? RejectedAt  { get; private set; }
   public string? RejectionReason { get; private set; }
   public Guid? AuditedByEmployeeId   { get; private set; }

   public DateTimeOffset? DeactivatedAt { get; private set; }
   public Guid? DeactivatedByEmployeeId { get; private set; }
   
   // Derived state (read convenience, not persisted)
   public bool IsProfileComplete =>
      !string.IsNullOrWhiteSpace(Firstname) &&
      !string.IsNullOrWhiteSpace(Lastname) &&
      !string.IsNullOrWhiteSpace(Subject) &&
      EmailVo is not null && !string.IsNullOrWhiteSpace(EmailVo.Value);

   public bool IsActive =>
      Status == CustomerStatus.Active &&
      DeactivatedAt is null;
   
   // public string Email { get; private set; } = string.Empty;
   public EmailVo EmailVo { get; private set; } = null!;
   public AddressVo? AddressVo { get; private set; } = null;
   
   //--- ctors -----------------------------------------------------------------
   private Customer() {
   }

   private Customer(
      Guid id,
      string firstname,
      string lastname,
      string? companyName,
      EmailVo emailVo,
      string subject,
      AddressVo? addressVo = null
   ) {
      Id = id;
      Firstname = firstname;
      Lastname = lastname;
      CompanyName = companyName;
      EmailVo = emailVo;
      Subject = subject;
      AddressVo= addressVo;
   }
   
   // --- static factory to create a Customer object ---------------------------
   public static Result<Customer> Create(
      string firstname,
      string lastname,
      string? companyName,
      EmailVo emailVo,
      string subject,
      string? id = null,
      DateTimeOffset createdAt = default!,
      AddressVo? addressVo = null
   ) {
      // Normalize input early
      firstname = firstname.Trim();
      lastname = lastname.Trim();
      companyName = companyName?.Trim();
      
      // Validate input fields
      if (string.IsNullOrWhiteSpace(firstname))
         return Result<Customer>.Failure(CustomerErrors.FirstnameIsRequired);
      if (firstname.Length is < 2 or > 80)
         return Result<Customer>.Failure(CustomerErrors.InvalidFirstname);

      if (string.IsNullOrWhiteSpace(lastname))
         return Result<Customer>.Failure(CustomerErrors.LastnameIsRequired);
      if (lastname.Length is < 2 or > 80)
         return Result<Customer>.Failure(CustomerErrors.InvalidLastname);

      if (!string.IsNullOrWhiteSpace(companyName) && companyName.Length is < 2 or > 80)
         return Result<Customer>.Failure(CustomerErrors.InvalidCompanyName);
      
      // Check subject
      var resultSubject = IdentitySubject.Check(subject);
      if (resultSubject.IsFailure)
         return Result<Customer>.Failure(resultSubject.Error);

      // createdAt is required for proper event timestamping
      if (createdAt == default)
         return Result<Customer>.Failure(CustomerErrors.CreatedAtIsRequired);
      
      // Resolve an entity id from an optional raw string.
      var resultId = Resolve(id, CustomerErrors.InvalidId);
      if (resultId.IsFailure)
         return Result<Customer>.Failure(resultId.Error);
      var localId = resultId.Value;
      
      // Create the entity with validated and normalized input
      var customer = new Customer(
         id: localId,
         firstname: firstname,
         lastname: lastname,
         companyName: companyName,
         emailVo: emailVo,
         subject: subject,
         addressVo: addressVo
      );
      
      // set the timestamps
      customer.Initialize(createdAt);

      // auto-activate on creation (no employee involved)
      var employeeId = Guid.Parse("00000000-0000-0000-0000-123456789000"); // system employee for self-service actions
      customer.Activate(employeeId, customer.CreatedAt); 
      
      return Result<Customer>.Success(customer);
   }
   
   // --- domain methods ---------------------------- --------------------------
   // Employee activates the owner after external identity verification.
   // Activation is only possible if the owner is Pending and profile is complete.
   public Result Activate(
      Guid activatedByEmployeeId, 
      DateTimeOffset activatedAt
   ) {
      if (activatedAt == default)
         return Result.Failure(CommonErrors.TimestampIsRequired);
      
      // fail early if preconditions for activation are not met
      // (employee, timestamp, status, profile)
      if (activatedByEmployeeId == Guid.Empty)
         return Result.Failure(CustomerErrors.AuditRequiresEmployee);
      if (Status != CustomerStatus.Pending)
         return Result.Failure(CustomerErrors.NotPending);
      if (!IsProfileComplete)
         return Result.Failure(CustomerErrors.ProfileIncomplete);
   
      Status = CustomerStatus.Active;
      ActivatedAt = activatedAt;
      AuditedByEmployeeId = activatedByEmployeeId;
      
      RejectedAt = null;
      RejectionReason = null;
      
      // create initial account for the owner (domain event, handled in application layer)
      Touch(activatedAt);
      return Result.Success();
   }

   
   // Customer updates their profile
   public Result Update(
      string? lastname = null,
      string? companyName = null,
      EmailVo? emailVo = null,
      AddressVo? addressVo = null,
      DateTimeOffset updatedAt = default!
   ) {
      if (updatedAt == default)
         return Result.Failure(CommonErrors.TimestampIsRequired);
      
      lastname  = lastname?.Trim();
      companyName = companyName?.Trim();

      if (!string.IsNullOrWhiteSpace(lastname) && lastname.Length is < 2 or > 80)
         return Result.Failure(CustomerErrors.InvalidLastname);

      if (!string.IsNullOrWhiteSpace(companyName) && companyName.Length is < 2 or > 80)
         return Result.Failure(CustomerErrors.InvalidCompanyName);
      
      // Apply changes
      if(lastname is not null) Lastname  = lastname;
      if(companyName is not null) CompanyName = companyName;
      if(emailVo is not null) EmailVo = emailVo;
      if(addressVo is not null) AddressVo = addressVo;
      
      Touch(updatedAt);
      return Result.Success();
   }
   
   public string AsString() {
      var text = $"Vorname: {Firstname}, Nachname: {Lastname}\n" +
         $"Company: {CompanyName}\n" +
         $"E-Mail: {EmailVo.Value}";
      if (AddressVo != null)
         text += "\nAddress" +
                  $"\n{AddressVo.Street}" +
                  $"\n{AddressVo.PostalCode} {AddressVo.City}";
      if(AddressVo?.Country != null) 
         text +=  $"\n{AddressVo.Country}";
      return text;
   }
}