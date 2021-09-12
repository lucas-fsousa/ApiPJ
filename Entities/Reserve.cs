using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Entities {
  public class Reserve {
    public int Id { get; set; }
    public int IdCustomer { get; set; }
    public int IdApartment { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime InitialDate { get; set; }
    public DateTime FinalDate { get; set; }
  }
}
