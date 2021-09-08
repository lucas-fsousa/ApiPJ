using ApiPJ.Database;
using ApiPJ.Entities;
using ApiPJ.Models.BlackoutDates;
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


    public async Task Teste(BlackoutDate inputModel) {
      await _context.BlackoutDatesContext.AddAsync(inputModel);
    }

    public async Task<List<Apartment>> GetApartments() {
      var listApartment = _context.ApartmentContext.Select(x => x).ToList();
      var listBlackoutDate = _context.BlackoutDatesContext.Select(x => x).ToList();
      var list = new List<Apartment>();

      foreach(var apartment in listApartment) {
        var ls = new List<BlackoutDate>();
        foreach(var blackoutDate in listBlackoutDate) {
          if(apartment.Id == blackoutDate.ApartmentId) {
            ls.Add(blackoutDate);
          }
        }
        Apartment add = apartment;
        add.ListaEnumerable = ls;
        list.Add(add);

      }
      return list;
    }





  }
}
