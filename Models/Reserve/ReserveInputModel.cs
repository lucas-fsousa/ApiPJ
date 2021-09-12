using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Models.Reserve {
  /* ------------ GLOSSARY ---------------
  * 
  * [Required(ErrorMessage ="" )] informs that the field is necessary and displays an error message if not filled out properly
  * 
  */
  public class ReserveInputModel {

    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public decimal TotalPrice { get; set; }

    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public int IdApartment { get; set; }

    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public int IdCustomer { get; set; }

    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public DateTime InitialDate { get; set; }

    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public DateTime FinalDate { get; set; }
  }
}
