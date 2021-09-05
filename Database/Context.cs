using ApiPJ.Database.Mapping;
using ApiPJ.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiPJ.Database {
  public class Context: DbContext {
    public Context(DbContextOptions<Context> options) : base(options) { }

    public DbSet<Customer> CustomerContext { get; set; }
    public DbSet<FullAdress> FullAdressesContext { get; set; }
    public DbSet<Employee> EmployeeContext { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
      builder.ApplyConfiguration(new CustomerMapping());
      builder.ApplyConfiguration(new FullAdressMapping());
      builder.ApplyConfiguration(new EmployeeMapping());
      base.OnModelCreating(builder);
    }
  }
}
