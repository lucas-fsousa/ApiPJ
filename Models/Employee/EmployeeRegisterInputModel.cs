using ApiPJ.Models.GenericUser;
using System;
using System.ComponentModel.DataAnnotations;


namespace ApiPJ.Models.Employee {
  /* ------------ GLOSSARY ---------------
  * [StringLenght()] sets the maximum number of characters allowed by the field
  * 
  * [Required(ErrorMessage ="" )] informs that the field is necessary and displays an error message if not filled out properly
  * 
  * [DataType(Dataype.**)] Inform the type of HTML input the field allows
  * 
  * [MaxLength()] Entityframework structure capable of setting the field size in the database
  */
  public class EmployeeRegisterInputModel: GenericUserRegisterInputModel {

    [DataType(DataType.DateTime)]
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public DateTime AdmissionDate { get; set; }

    [MaxLength(15)]
    [StringLength(15)]
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public string WalletWorkId { get; set; }

    [MaxLength(70)]
    [StringLength(70)]
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public string FunctionName { get; set; }

    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public float ContractualSalary { get; set; }
  }
}
