using WebApi._2_Core.Customers._3_Domain.Entities;
namespace WebApi._2_Core.Customers._1_Ports.Outbound;

// Database context abstraction for the Customers module.
// Provides minimal persistence access required by repositories.
// The concrete implementation typically wraps an EF Core DbContext.
public interface ICustomerDbContext {

   // Query access to Customer aggregates
   IQueryable<Customer> Customers { get; }

   // Add a new customer to the persistence context
   void Add(Customer entity);

   // Mark an existing customer as modified
   void Update(Customer entity);
}

/*
Didaktik
--------

Dieses Interface abstrahiert den Datenbankzugriff für den Customers-Bounded-Context.

Es handelt sich nicht um ein Repository, sondern um eine technische Abstraktion über 
den konkreten ORM-Context (z.B. EF Core DbContext).

Repositories verwenden dieses Interface, um Customer-Aggregate zu laden und zu speichern.

Die konkrete Implementierung liegt in der Infrastructure Schicht.
Dadurch bleibt das Domainmodell unabhängig von derPersistenztechnologie.

Lernziele
---------
- Unterschied zwischen Repository und DbContext verstehen
- Entkopplung der Domain von EF Core
- Einsatz von Persistence Ports
- Verbesserung der Testbarkeit durch Abstraktion
*/
