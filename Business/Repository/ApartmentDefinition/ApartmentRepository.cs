using ApiPJ.Database;
using ApiPJ.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Business.Repository.ApartmentDefinition {
  public class ApartmentRepository : IApartmentRepository {
    private readonly Context _context;

    public ApartmentRepository(Context context) {
      _context = context;
    }
    public async Task Commit() {
      await _context.SaveChangesAsync();
    }

    public async Task Register(Apartment apartment) {
      await _context.ApartmentContext.AddAsync(apartment);
    }
  }
}
