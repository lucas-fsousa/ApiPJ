using ApiPJ.Entities;
using ApiPJ.Models.BlackoutDates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Business.Repository.ApartmentDefinition {
  public interface IApartmentRepository {
    public Task Commit();
    public Task Register(Apartment apartment);
    public Task<List<Apartment>> GetApartments();
    public Task<Apartment> GetApartment(int id);
    public void Delete(Apartment apartment);
    public void Update(Apartment apartment);
  }
}
