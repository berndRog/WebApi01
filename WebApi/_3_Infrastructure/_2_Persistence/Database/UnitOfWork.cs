using Microsoft.EntityFrameworkCore;
using WebApi._2_Modules.Customers._1_Ports.Outbound;
namespace WebApi._3_Infrastructure._2_Persistence.Database;

public sealed class UnitOfWork(
   DbContext dbContext,
   ILogger<UnitOfWork> logger
) : IUnitOfWork {
   public async Task<int> SaveAllChangesAsync(
      string? text = null,
      CancellationToken ctToken = default
   ) {
      dbContext.ChangeTracker.DetectChanges();
      LogBefore(text);
      
      var rows = await dbContext.SaveChangesAsync(ctToken);

      LogAfter(rows);
      return rows;
   }

   public void ClearChangeTracker() =>
      dbContext.ChangeTracker.Clear();

   public void LogChangeTracker(string text) {
      if (!logger.IsEnabled(LogLevel.Debug)) return;
      dbContext.ChangeTracker.DetectChanges();
      var output = dbContext.ChangeTracker.DebugView.LongView;
      LogOutput(text, output);
   }
   
   // -----------------------------
   // Logging helpers
   // -----------------------------
   private void LogBefore(string? text) {
      if (!logger.IsEnabled(LogLevel.Debug)) return;
      if (!string.IsNullOrWhiteSpace(text)) logger.LogDebug("{Text}", text);
      LogOutput("Before save Changes", dbContext.ChangeTracker.DebugView.LongView);
   }

   private void LogAfter(int rows) {
      if (!logger.IsEnabled(LogLevel.Debug)) return;
      logger.LogDebug("SaveChanges affected {Result} rows", rows);
      LogOutput("After save Changes", dbContext.ChangeTracker.DebugView.LongView);
   }

   private static List<string> SplitIntoChunks(string text, int chunkSize) {
      var chunks = new List<string>();
      for (int i = 0; i < text.Length; i += chunkSize) {
         chunks.Add(text.Substring(i, Math.Min(chunkSize, text.Length - i)));
      }
      return chunks;
   }
   private void LogOutput(string text, string output) {
      // Split into chunks of 4000 characters
      const int chunkSize = 4000;
      var chunks = SplitIntoChunks(output, chunkSize);
      
      logger.LogDebug("{Text} - ChangeTracker Output (Part {Part}/{Total})", 
         text, 1, chunks.Count);

      for (int i = 0; i < chunks.Count; i++) {
         logger.LogDebug("Part {Part}/{Total}:\n{Output}",
            i + 1, chunks.Count, chunks[i]);
      }
   }
}