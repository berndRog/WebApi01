using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi._2_Modules.BuildingBlocks._3_Domain.ValueObjects;
namespace WebApi._3_Infrastructure._2_Persistence.Converters;

public static class EmailPropertyBuilderExtensions {
   
   public static PropertyBuilder<EmailVo> HasEmailConversion(this PropertyBuilder<EmailVo> builder) {
      builder.HasConversion(EmailEf.Converter);
      builder.Metadata.SetValueComparer(EmailEf.Comparer);
      builder.HasMaxLength(254);
      return builder;
   }
}