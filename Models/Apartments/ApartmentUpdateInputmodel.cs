using ApiPJ.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Models.Apartments {
  /* ------------ GLOSSARY ---------------
    * [StringLenght()] sets the maximum number of characters allowed by the field
    * 
    * [Required(ErrorMessage ="" )] informs that the field is necessary and displays an error message if not filled out properly
    * 
    * [DataType(Dataype.**)] Inform the type of HTML input the field allows
    * 
    * [MaxLength()] Set maximum string size
    * 
    */
  public class ApartmentUpdateInputmodel {
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    [Range(1, 99)]
    public int Bedrooms { get; set; } // quantity

    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    [Range(1, 99)]
    public int ParkingLots { get; set; } // quantity

    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    [Range(1, 99)]
    public int MaximumPeoples { get; set; } // quantity

    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    [MaxLength(255)]
    [StringLength(255)]
    public string Description { get; set; }

    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    [MaxLength(255)]
    [StringLength(255)]
    public string City { get; set; }

    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public string Localization { get; set; }

    public bool Available { get; set; }
  }
}
