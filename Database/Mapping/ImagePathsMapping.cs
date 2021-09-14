using ApiPJ.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Database.Mapping {
  public class ImagePathsMapping : IEntityTypeConfiguration<ImagePath> {
    public void Configure(EntityTypeBuilder<ImagePath> builder) {
      builder.ToTable("TB_IMAGEPATH").HasKey(x => x.Id);
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.ApartmentId).IsRequired();
      builder.Property(x => x.Path).IsRequired();
      builder.HasIndex(x => x.Path).IsUnique();
    }
  }
}
