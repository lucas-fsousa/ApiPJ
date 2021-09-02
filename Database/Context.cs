using ApiPJ.Database.Mapping;
using ApiPJ.Entities;
using ApiPJ.Models.GenericUser;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Database {
  public class Context: DbContext {
    public Context(DbContextOptions<Context> options) : base(options) { }

    public DbSet<GenericUser> GenericUserContext { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
      builder.ApplyConfiguration(new GenericUserMapping());
      base.OnModelCreating(builder);
    }
  }
}
