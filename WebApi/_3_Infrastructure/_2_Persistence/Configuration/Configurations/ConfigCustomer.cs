using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi._2_Core.BuildingBlocks._3_Domain.ValueObjects;
using WebApi._2_Core.Customers._3_Domain.Entities;
using WebApi._3_Infrastructure._2_Persistence.Database.Converter;
namespace WebApi._3_Infrastructure._2_Persistence.Configuration.Configurations;

public sealed class ConfigCustomer(
   DateTimeOffsetToIsoStringConverter dtConv,
   DateTimeOffsetToIsoStringConverterNullable dtConvNul
) : IEntityTypeConfiguration<Customer> {

   public void Configure(EntityTypeBuilder<Customer> builder) {
      
      // Tablename
      builder.ToTable("Customers");

      // Primary Key will never be generated
      builder.HasKey(c => c.Id);
      builder.Property(c => c.Id).ValueGeneratedNever();
      
      // Auditing timestamps
      builder.Property(c => c.CreatedAt)
         .HasConversion(dtConv)
         .IsRequired();

      builder.Property(c => c.UpdatedAt)
         .HasConversion(dtConv)
         .IsRequired();
      
      // Profile data
      builder.Property(c => c.Firstname)
         .HasMaxLength(80)
         .IsRequired();
      builder.Property(c => c.Lastname)
         .HasMaxLength(80)
         .IsRequired();
      builder.Property(c => c.CompanyName)
         .HasMaxLength(80)
         .IsRequired(false);

      // Value Object EmailVo mit Conversion
      builder.Property(c => c.EmailVo)
         .HasConversion(
            vo => vo.Value, 
            s => EmailVo.FromPersisted(s))
         .IsRequired()
         .HasColumnName("Email") 
         .HasMaxLength(254);
      builder.HasIndex(c => c.EmailVo).IsUnique();

      builder.Property(c => c.Subject)
         .HasMaxLength(200)
         .IsRequired();
      builder.HasIndex(c => c.Subject)
         .IsUnique();

      // Status
      builder.Property(c => c.Status)
         .HasConversion<int>()   
         .IsRequired();

      // Employee decisions / audit facts
      builder.Property(c => c.ActivatedAt)
         .HasConversion(dtConvNul)
         .IsRequired(false);

      builder.Property(c => c.RejectedAt)
         .HasConversion(dtConvNul)
         .IsRequired(false);

      builder.Property(c => c.RejectCode)
         .HasConversion<int>()   
         .IsRequired();

      builder.Property(c => c.AuditedByEmployeeId)
         .IsRequired(false);

      builder.Property(c => c.DeactivatedAt)
         .HasConversion(dtConvNul)
         .IsRequired(false);

      builder.Property(c => c.DeactivatedByEmployeeId)
         .IsRequired(false);

      // Domain-only
      builder.Ignore(o => o.DisplayName);
      builder.Ignore(c => c.IsActive);
      builder.Ignore(c => c.IsProfileComplete);
      

      // Address (owned value object)
      builder.OwnsOne(c => c.AddressVo, a => {
         
         a.Property(a => a.Street)
            .HasMaxLength(80)
            .HasColumnName("Street")
            .IsRequired();

         a.Property(a => a.PostalCode)
            .HasMaxLength(20)
            .HasColumnName("PostalCode")
            .IsRequired();

         a.Property(a => a.City)
            .HasMaxLength(80)
            .HasColumnName("City")
            .IsRequired();

         a.Property(a => a.Country)
            .HasMaxLength(80)
            .HasColumnName("Country")
            .IsRequired(false);
      });
      builder.Navigation(c => c.AddressVo)
         .IsRequired();
      
   }
}
