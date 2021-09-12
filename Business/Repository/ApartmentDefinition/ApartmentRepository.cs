using ApiPJ.Database;
using ApiPJ.Entities;
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
      var listReserve = _context.ReserveContext.Select(x => x).ToList();
      var list = new List<Apartment>();

      foreach(var apartment in listApartment) {
        var allReserves = new List<Reserve>();
        foreach(var reserve in listReserve) {
          if(apartment.Id == reserve.IdApartment) {
            allReserves.Add(reserve);
          }
        }
        Apartment apartmentFound = apartment;
        apartmentFound.Reserves = allReserves;
        list.Add(apartmentFound);

      }
      return list;
    }

    public async Task<Apartment> GetApartment(int id) {
      var result = await _context.ApartmentContext.FindAsync(id);
      if(result == null) {
        return null;
      }
      result.Reserves = _context.ReserveContext.Select(x => x).Where(x => x.IdApartment == id).ToList();
      return result;
    }

    public void Delete(Apartment apartment) {
      var listBlackoutDates = _context.ReserveContext.Select(x => x).Where(x => x.IdApartment == apartment.Id).ToList();
      foreach(var itemForDelet in listBlackoutDates) {
        _context.ReserveContext.Remove(itemForDelet);
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
        x.DailyPrice = apartment.DailyPrice;
        x.City = apartment.City;
      });
    }
  }
}
