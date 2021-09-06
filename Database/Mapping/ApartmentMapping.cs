using ApiPJ.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiPJ.Database.Mapping {
  public class ApartmentMapping : IEntityTypeConfiguration<Apartment> {
    public void Configure(EntityTypeBuilder<Apartment> builder) {
      builder.ToTable("TB_APARTMENT");
      builder.Property(x => x.MaximumPeoples).IsRequired();
      builder.Property(x => x.Description).IsRequired();
      builder.Property(x => x.ParkingLots).IsRequired();
      builder.Property(x => x.Available).IsRequired();
      builder.Property(x => x.Bedrooms).IsRequired();
      builder.Property(x => x.Price).IsRequired();
      builder.Property(x => x.City).IsRequired();
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.HasKey(x => x.Id);
      builder.HasOne(x => x.Adress).WithMany().HasForeignKey(fk => fk.Id);
      //builder.HasOne(x => x.BlackoutDates).WithMany().HasForeignKey(fk => fk.Id);
    }
  }
}
