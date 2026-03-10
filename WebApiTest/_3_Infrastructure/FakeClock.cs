using WebApi._2_Modules.BuildingBlocks._1_Ports.Outbound;
namespace WebApiTest._3_Infrastructure;

public sealed class FakeClock : IClock {
   public DateTimeOffset UtcNow { get; } = DateTimeOffset.UtcNow;
   
   public FakeClock(DateTimeOffset? utcNow = null) {
      if (utcNow.HasValue) {
         UtcNow = utcNow.Value;
      }
   }

   public FakeClock(DateTime utcNow) {
      UtcNow = new DateTimeOffset(utcNow, TimeSpan.Zero);
   }
}