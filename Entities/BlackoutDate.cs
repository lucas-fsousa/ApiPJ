using System;

namespace ApiPJ.Entities {
  public class BlackoutDate {
    public int Id { get; set; }
    public DateTime InitialDate { get; set; }
    public DateTime FinalDate { get; set; }
  }
}
