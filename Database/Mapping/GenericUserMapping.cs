using ApiPJ.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Database.Mapping {
  public class GenericUserMapping: IEntityTypeConfiguration<GenericUser> {
    public void Configure(EntityTypeBuilder<GenericUser> builder) {
      builder.ToTable("TB_USER");
      builder.HasKey(u => u.Id);
      builder.Property(u => u.Id).ValueGeneratedOnAdd();
      builder.Property(u => u.Rg);
      builder.Property(u => u.Sex);
      builder.Property(u => u.Cpf);
      builder.Property(u => u.Name);
      builder.Property(u => u.Email);
      builder.Property(u => u.Adress);
      builder.Property(u => u.Password);
      builder.Property(u => u.BirthDate);
      builder.Property(u => u.PhoneNumber);
      builder.Property(u => u.MaritalStatus);

    }
  }
}
