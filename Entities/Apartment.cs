namespace ApiPJ.Entities {
  public class Apartment {
    public int Id { get; set; }
    public int Bedrooms { get; set; } // quantity
    public int ParkingLots { get; set; } // quantity
    public int MaximumPeoples { get; set; } // quantity
    public string Description { get; set; }
    public string City { get; set; }
    public bool Available { get; set; }
    public decimal Price { get; set; }
    public FullAdress Adress { get; set; }
    public BlackoutDate BlackoutDates { get; set; }
  }
}
