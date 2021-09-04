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
      builder.HasIndex(u => u.Cpf).IsUnique();
      builder.Property(u => u.Id).ValueGeneratedOnAdd();
      builder.Property(u => u.Rg).HasMaxLength(7).IsRequired();
      builder.Property(u => u.Sex).HasMaxLength(60).IsRequired();
      builder.Property(u => u.Cpf).HasMaxLength(11).IsRequired();
      builder.Property(u => u.Name).HasMaxLength(200).IsRequired();
      builder.Property(u => u.Email).HasMaxLength(200).IsRequired();
      builder.Property(u => u.Password).HasMaxLength(200).IsRequired();
      builder.Property(u => u.BirthDate).IsRequired();
      builder.Property(u => u.PhoneNumber).IsRequired();
      builder.Property(u => u.MaritalStatus).IsRequired();
      builder.HasOne(u => u.Adress).WithMany().HasForeignKey(fk => fk.Id);
    }
  }
}
