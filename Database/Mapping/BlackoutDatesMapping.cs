using ApiPJ.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiPJ.Database.Mapping {
  public class BlackoutDatesMapping : IEntityTypeConfiguration<BlackoutDate> {
    public void Configure(EntityTypeBuilder<BlackoutDate> builder) {
      builder.ToTable("TB_BLACKOUTDATE");

      builder.Property(x => x.Id).UseIdentityColumn();
      builder.Property(x => x.InitialDate).IsRequired();
      builder.Property(x => x.FinalDate).IsRequired();
    }
  }

}
