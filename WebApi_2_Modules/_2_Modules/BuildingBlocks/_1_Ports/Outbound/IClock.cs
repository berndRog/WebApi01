namespace WebApi._2_Modules.BuildingBlocks._1_Ports.Outbound;

public interface IClock {
   DateTimeOffset UtcNow { get; }
}