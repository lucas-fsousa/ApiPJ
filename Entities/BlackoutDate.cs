using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPJ.Entities {
  public class BlackoutDate {
    public int Id { get; set; }
    public int ApartmentId { get; set; }
    public DateTime InitialDate { get; set; }
    public DateTime FinalDate { get; set; }
  }
}
