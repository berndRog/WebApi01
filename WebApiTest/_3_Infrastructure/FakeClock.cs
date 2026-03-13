using WebApi._2_Core.BuildingBlocks._1_Ports.Outbound;
namespace WebApiTest._3_Infrastructure;

public sealed class FakeClock : IClock {
   public DateTimeOffset UtcNow { get; } = DateTimeOffset.Parse("2025-01-01T00:00:00Z");

   public FakeClock(DateTimeOffset? utcNow = null) {
      if (utcNow.HasValue) {
         UtcNow = DateTimeOffset.UtcNow;
      }
   }
}