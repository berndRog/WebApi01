using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi._2_Core.Customers._3_Domain.Entities;
namespace WebApi._3_Infrastructure._2_Persistence.Configuration.Configurations;

public sealed class ConfigCustomer 
   : IEntityTypeConfiguration<Customer>
{
   public void Configure(EntityTypeBuilder<Customer> builder) {
      
      builder.ToTable("Customer");

      // Primary Key (aus Entity Basisklasse)
      builder.HasKey(c => c.Id);

      // Primitive Properties
      builder.Property(c => c.Firstname)
         .IsRequired()
         .HasMaxLength(80);

      builder.Property(c => c.Lastname)
         .IsRequired()
         .HasMaxLength(80);

      builder.Property(c => c.CompanyName)
         .HasMaxLength(80);

      // Email Value Object
      builder.OwnsOne(c => c.Email, email => {
         email.Property(e => e.Value)
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(254);
         email.WithOwner();
      });

      // Address Value Object (optional)
      builder.OwnsOne(c => c.Address, address => {
         address.Property(a => a.Street)
            .HasColumnName("Street")
            .HasMaxLength(80);

         address.Property(a => a.PostalCode)
            .HasColumnName("PostalCode")
            .HasMaxLength(10);

         address.Property(a => a.City)
            .HasColumnName("City")
            .HasMaxLength(80);

         address.Property(a => a.Country)
            .HasColumnName("Country")
            .HasMaxLength(80);

         address.WithOwner();
      });
   }
}