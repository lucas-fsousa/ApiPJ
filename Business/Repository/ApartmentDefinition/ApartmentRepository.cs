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
      var listApartment = await _context.ApartmentContext.Select(x => x).ToListAsync();
      var listReserve = await _context.ReserveContext.Select(x => x).ToListAsync();
      var list = new List<Apartment>();

      foreach(var apartment in listApartment) {
        var allReserves = new List<Reserve>();
        foreach(var reserve in listReserve) {
          if(apartment.IdAp == reserve.IdApartment) {
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
      result.Reserves = await _context.ReserveContext.Select(x => x).Where(x => x.IdApartment == id).ToListAsync();
      return result;
    }

    public async void Delete(Apartment apartment) {
      await _context.ReserveContext.Select(x => x).Where(x => x.IdApartment == apartment.IdAp).ForEachAsync(x => {
        _context.Remove(x);
      });
      await _context.ImagePathContext.Select(x => x).Where(x => x.ApartmentId == apartment.IdAp).ForEachAsync(x => {
        DeleteImage(x);
      });
      _context.ApartmentContext.Remove(apartment);
    }

    public async void Update(Apartment apartment) {
      await _context.ApartmentContext.Where(x => x.IdAp == apartment.IdAp).ForEachAsync(x => {
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

    public async Task UploadImages(ImagePath imagePath) {
      await _context.ImagePathContext.AddAsync(imagePath);
    }

    public void DeleteImage(ImagePath image) {
      _context.ImagePathContext.Remove(image);
    }
    public async Task<List<ImagePath>> GetAllImagesByApartmentId(int apartmentId) {
      return await _context.ImagePathContext.Select(x => x).Where(x => x.ApartmentId == apartmentId).ToListAsync();
    }

    public async Task<ImagePath> GetImageById(int idImage) {
      return await _context.ImagePathContext.FirstOrDefaultAsync(x => x.IdImgPath == idImage);
    }
  }
}
