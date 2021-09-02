using ApiPJ.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Database.Mapping {
  public class FullAdressMapping: IEntityTypeConfiguration<FullAdress> {
    public void Configure(EntityTypeBuilder<FullAdress> builder) {
      builder.ToTable("TB_ADRESS");
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.PublicPlace).IsRequired().HasMaxLength(100);
      builder.Property(x => x.Reference).IsRequired().HasMaxLength(100);
      builder.Property(x => x.Street).IsRequired().HasMaxLength(100);
    }
  }
}
