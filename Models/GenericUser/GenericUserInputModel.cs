using ApiPJ.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace ApiPJ.Models.GenericUser {
  /* ------------ GLOSSARY ---------------
    * [StringLenght()] sets the maximum number of characters allowed by the field
    * 
    * [Required(ErrorMessage ="" )] informs that the field is necessary and displays an error message if not filled out properly
    * 
    * [DataType(Dataype.**)] Inform the type of HTML input the field allows
    * 
    * [EmailAddress] Informs that the e-mail field has a suitable training structure nome@provider.net/.com/
    * 
    * [MaxLength()] Set maximum string size
    * 
    * [MinLenght()] Set minimum string size
    * 
    */
  public class GenericUserInputModel {
    [StringLength(40)]
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public string MaritalStatus { get; set; }

    [StringLength(250)]
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public string Name { get; set; }

    [StringLength(7)]
    [MinLength(7)]
    [MaxLength(7)]
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public string Rg { get; set; }

    [MinLength(11)]
    [MaxLength(11)]
    [StringLength(11)]
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public string Cpf { get; set; }

    [StringLength(15)]
    [DataType(DataType.PhoneNumber)]
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public string PhoneNumber { get; set; }

    [EmailAddress]
    [StringLength(250)]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public string Sex { get; set; }

    [DataType(DataType.DateTime)]
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public DateTime BirthDate { get; set; }

    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public FullAdress Adress { get; set; }
  }
}
