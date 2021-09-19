using ApiPJ.Entities;
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
    public Task UploadImages(ImagePath imagePath);
    public void DeleteImage(ImagePath images);
    public Task<ImagePath> GetImageById(int idImage);
    public Task<List<ImagePath>> GetAllImagesByApartmentId(int apartmentId);
  }
}
