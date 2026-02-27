using WebApi._4_BuildingBlocks._3_Domain.Errors;
namespace WebApi._4_BuildingBlocks._3_Domain.Entities;

/// <summary>
/// Base class for all domain entities.
///
/// Identity semantics:
/// - Equality is based solely on Id.
/// - Two entities are equal if and only if their Id values are equal.
/// - Transient entities (Id == Guid.Empty) are never considered equal.
///
/// Design goals:
/// - Reflect database identity (Primary Key semantics).
/// - Provide consistent equality behavior across the domain model.
/// - Avoid reference-based (address) equality pitfalls.
/// </summary>
public abstract class Entity : IEquatable<Entity> {
   /// <summary>
   /// Primary key of the entity.
   /// Must be unique and immutable once persisted.
   /// </summary>
   public Guid Id { get; protected set; }

   
   // standard constructor in an abstract class is protected to prevent direct instantiation
   // protected Entity() {}
   
   /// <summary>
   /// Resolves an entity id from an optional raw string.
   ///
   /// Behavior:
   /// - null               → generates a new Guid
   /// - empty/whitespace   → failure
   /// - invalid Guid       → failure
   /// - valid Guid         → success
   ///
   /// Purpose:
   /// - Centralizes ID parsing logic.
   /// - Ensures consistent validation and error handling.
   /// - Prevents Guid parsing logic from leaking into controllers or use cases.
   /// </summary>
   public static Result<Guid> Resolve(
      string? rawId,
      DomainErrors invalidIdError
   ) {
      // If no id is provided, generate a new identity
      if (rawId is null)
         return Result<Guid>.Success(Guid.NewGuid());

      // Reject empty or whitespace input
      if (string.IsNullOrWhiteSpace(rawId))
         return Result<Guid>.Failure(invalidIdError);

      // Attempt to parse Guid
      if (!Guid.TryParse(rawId, out var guid))
         return Result<Guid>.Failure(invalidIdError);

      return Result<Guid>.Success(guid);
   }

   /// <summary>
   /// Overrides object equality.
   ///
   /// Entities are equal if:
   /// - They are of the same type hierarchy
   /// - Both have a non-empty Id
   /// - Their Id values are equal
   /// </summary>
   public override bool Equals(object? obj) {
      if (obj is not Entity other)
         return false;

      if (ReferenceEquals(this, other))
         return true;

      // Transient entities are never equal
      if (Id == Guid.Empty || other.Id == Guid.Empty)
         return false;

      return Id == other.Id;
   }

   /// <summary>
   /// Strongly typed equality implementation.
   /// </summary>
   public bool Equals(Entity? other) =>
      Equals((object?)other);

   /// <summary>
   /// Hash code is derived from Id.
   /// Required for correct behavior in hash-based collections.
   /// </summary>
   public override int GetHashCode() =>
      Id.GetHashCode();

   /// <summary>
   /// Equality operator overload.
   /// </summary>
   public static bool operator ==(Entity? a, Entity? b) {
      if (a is null && b is null) return true;
      if (a is null || b is null) return false;
      return a.Equals(b);
   }

   /// <summary>
   /// Inequality operator overload.
   /// </summary>
   public static bool operator !=(Entity? a, Entity? b) =>
      !(a == b);
}

/*
================================================================================
DIDAKTIK UND LERNZIELE (für Vorlesung / 4. Semester Softwarearchitektur)
================================================================================

1. Unterschied zwischen OOP-Gleichheit und DDD-Identität verstehen
   - Standard-OOP: Referenzgleichheit
   - Entity (DDD): Identitätsgleichheit (Primary Key)
   - Value Object: Wertgleichheit

2. Datenbank-Identität vs. Objektzustand unterscheiden
   - Eine Entity bleibt fachlich gleich, auch wenn sich ihr Zustand ändert.
   - Gleichheit basiert ausschließlich auf der Identität (Guid).

3. Transiente Entities korrekt behandeln
   - Entities ohne gesetzte Id (Guid.Empty) dürfen niemals als gleich gelten.
   - Verhindert subtile Fehler in Collections und EF Core Tracking.

4. Technische vs. fachliche Verantwortung reflektieren
   - Die Domäne arbeitet mit Guid (Identität).
   - String-Parsing stammt aus dem Application/API-Layer.
   - ResolveId ist eine pragmatische, aber bewusst technische Lösung.

5. Saubere Modellierung von Identität
   - Guid als technische Identität.
   - Optionale Weiterentwicklung: Strongly Typed Id (OwnerId, AccountId).

6. Architekturprinzipien anwenden
   - Kapselung von Gleichheitslogik.
   - Konsistente Identitätssemantik im gesamten System.
   - Vermeidung von Copy-Paste-Guid-Parsing.

Didaktisches Ziel:
Studierende sollen erkennen, dass eine Entity kein Objekt,
sondern ein Identitätskonzept ist. Die Implementierung spiegelt
gleichzeitig OOP-Prinzipien, DDD-Konzepte und relationale
Datenbanksemantik wider.

================================================================================
*/