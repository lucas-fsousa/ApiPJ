using ApiPJ.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiPJ.Database.Mapping {
  public class CustomerMapping: IEntityTypeConfiguration<Customer> {
    public void Configure(EntityTypeBuilder<Customer> builder) {
      builder.ToTable("TB_CUSTOMER");
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
      builder.HasOne(u => u.Adress).WithMany().HasForeignKey(x => x.Id);
    }
  }
}
