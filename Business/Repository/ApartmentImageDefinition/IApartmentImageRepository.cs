using ApiPJ.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Business.Repository.ApartmentImageDefinition {
  public interface IApartmentImageRepository {
    public Task UploadImages(ImagePath imagePath);
    public void DeleteImage(ImagePath images);
    public Task<List<ImagePath>> GetAllImagesByApartmentId(int apartmentId);
    public Task Commit();
  }
}
