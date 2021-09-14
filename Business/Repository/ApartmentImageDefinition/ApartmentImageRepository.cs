using ApiPJ.Database;
using ApiPJ.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Business.Repository.ApartmentImageDefinition {
  public class ApartmentImageRepository: IApartmentImageRepository {
    private readonly Context _context;
    public ApartmentImageRepository(Context context) {
      _context = context;
    }
    public async Task Commit() {
      await _context.SaveChangesAsync();
    }
    public async Task UploadImages(ImagePath imagePath) {
      await _context.ImagePathContext.AddAsync(imagePath);
    }

    public void DeleteImage(ImagePath image) {
      _context.Remove(image);
    }
    public async Task<List<ImagePath>> GetAllImagesByApartmentId(int apartmentId) {
      return await _context.ImagePathContext.Select(x => x).Where(x => x.ApartmentId == apartmentId).ToListAsync();
    }
  }
}
