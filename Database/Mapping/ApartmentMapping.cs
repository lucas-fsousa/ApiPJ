using ApiPJ.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiPJ.Database.Mapping {
  public class ApartmentMapping : IEntityTypeConfiguration<Apartment> {
    public void Configure(EntityTypeBuilder<Apartment> builder) {
      builder.ToTable("TB_APARTMENT");
      builder.Property(x => x.MaximumPeoples).IsRequired();
      builder.Property(x => x.Localization).IsRequired();
      builder.Property(x => x.Description).IsRequired();
      builder.Property(x => x.ParkingLots).IsRequired();
      builder.Property(x => x.Available).IsRequired();
      builder.Property(x => x.Bedrooms).IsRequired();
      builder.Property(x => x.DailyPrice).IsRequired();
      builder.Property(x => x.City).IsRequired();
      builder.Property(x => x.Id).UseIdentityColumn();

      builder.Ignore(x => x.Reserves);
      builder.Ignore(x => x.image);
    }
  }
}
