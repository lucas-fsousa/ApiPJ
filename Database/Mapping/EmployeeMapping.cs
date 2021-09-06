using ApiPJ.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiPJ.Database.Mapping {
  public class EmployeeMapping: IEntityTypeConfiguration<Employee> {
    public void Configure(EntityTypeBuilder<Employee> builder) {
      builder.ToTable("TB_EMPLOYEE");
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

      builder.Property(u => u.AdmissionDate).IsRequired();
      builder.Property(u => u.DemissionDate);
      builder.Property(u => u.FunctionName).IsRequired();
      builder.Property(u => u.WalletWorkId).IsRequired();
      builder.Property(u => u.ContractualSalary).IsRequired().HasPrecision(18,2);
      builder.Property(u => u.AcessLevel).IsRequired();
      builder.Property(u => u.Active).IsRequired();
    }
  }
}
