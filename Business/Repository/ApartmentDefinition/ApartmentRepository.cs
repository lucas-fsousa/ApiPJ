using ApiPJ.Database;
using ApiPJ.Entities;
using ApiPJ.Models.BlackoutDates;
using Microsoft.EntityFrameworkCore;
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
        add.DatesNotAvailable = ls;
        list.Add(add);

      }
      return list;
    }

    public async Task<Apartment> GetApartment(int id) {
      var result = await _context.ApartmentContext.FindAsync(id);
      if(result == null) {
        return null;
      }
      result.DatesNotAvailable = _context.BlackoutDatesContext.Select(x => x).Where(x => x.ApartmentId == id).ToList();
      return result;
    }

    public void Delete(Apartment apartment) {
      var listBlackoutDates = _context.BlackoutDatesContext.Select(x => x).Where(x => x.ApartmentId == apartment.Id).ToList();
      foreach(var itemForDelet in listBlackoutDates) {
        _context.BlackoutDatesContext.Remove(itemForDelet);
      }
      _context.ApartmentContext.Remove(apartment);
    }

    public async void Update(Apartment apartment) {
      await _context.ApartmentContext.Where(x => x.Id == apartment.Id).ForEachAsync(x => {
        x.MaximumPeoples = apartment.MaximumPeoples;
        x.Localization = apartment.Localization;
        x.Description = apartment.Description;
        x.ParkingLots = apartment.ParkingLots;
        x.Available = apartment.Available;
        x.Bedrooms = apartment.Bedrooms;
        x.Price = apartment.Price;
        x.City = apartment.City;
      });
    }
  }
}
