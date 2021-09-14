using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ApiPJ.Entities {
  public class Apartment {
    public int Id { get; set; }
    public int Bedrooms { get; set; } // quantity
    public int ParkingLots { get; set; } // quantity
    public int MaximumPeoples { get; set; } // quantity
    public string Description { get; set; }
    public string Localization { get; set; }
    public string City { get; set; }
    public bool Available { get; set; }
    public decimal DailyPrice { get; set; }

    public List<Reserve> Reserves { get; set; }
    public List<IFormFile> image { get; set; }
  }
}
