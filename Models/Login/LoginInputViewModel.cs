using System.ComponentModel.DataAnnotations;

namespace ApiPJ.Models.Login {
  /* ------------ GLOSSARY ---------------
    * [StringLenght()] sets the maximum number of characters allowed by the field
    * 
    * [Required(ErrorMessage ="" )] informs that the field is necessary and displays an error message if not filled out properly
    * 
    * [DataType(Dataype.**)] Inform the type of HTML input the field allows
    * 
    * 
    * [MaxLength()] Set maximum string size
    * 
    * [MinLenght()] Set minimum string size
    * 
    */
  public class LoginInputViewModel {

    [StringLength(11)]
    [MinLength(11)]
    [MaxLength(11)]
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public string Cpf { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public string Password { get; set; }
  }
}
