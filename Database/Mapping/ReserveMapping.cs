﻿using ApiPJ.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Database.Mapping {
  public class ReserveMapping : IEntityTypeConfiguration<Reserve> {
    public void Configure(EntityTypeBuilder<Reserve> builder) {
      builder.ToTable("TB_RESERVE").HasKey(x => x.IdReserve);
      builder.Property(x => x.IdReserve).UseIdentityColumn();
      builder.Property(x => x.FinalDate).IsRequired();
      builder.Property(x => x.InitialDate).IsRequired();
      builder.Property(x => x.IdCustomer).ValueGeneratedNever().IsRequired();
      builder.Property(x => x.IdApartment).ValueGeneratedNever().IsRequired();
      builder.Property(x => x.TotalPrice).ValueGeneratedNever().IsRequired().HasPrecision(18, 2);
      builder.Property(x => x.Checkin);
      builder.Property(x => x.Checkout);
    }
  }
}
