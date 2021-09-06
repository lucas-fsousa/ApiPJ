using ApiPJ.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiPJ.Database.Mapping {
  public class BlackoutDatesMapping : IEntityTypeConfiguration<BlackoutDate> {
    public void Configure(EntityTypeBuilder<BlackoutDate> builder) {
      builder.ToTable("TB_BLACKOUTDATES");
      builder.Property(x => x.InitialDate);
      builder.Property(x => x.FinalDate);
    }
  }
}
