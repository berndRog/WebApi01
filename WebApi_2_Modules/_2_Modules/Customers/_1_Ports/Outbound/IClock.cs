namespace WebApi._2_Modules.Customers._1_Ports.Outbound;

public interface IClock {
   DateTimeOffset UtcNow { get; }
}