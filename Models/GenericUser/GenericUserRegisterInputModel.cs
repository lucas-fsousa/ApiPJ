﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ApiPJ.Models.GenericUser {
  /* ------------ GLOSSARY ---------------
    * [StringLenght()] sets the maximum number of characters allowed by the field
    * 
    * [Required(ErrorMessage ="" )] informs that the field is necessary and displays an error message if not filled out properly
    * 
    * [DataType(Dataype.**)] Inform the type of HTML input the field allows
    * 
    * [MaxLength()] Entityframework structure capable of setting the field size in the database
    * 
    * [EmailAddress] Informs that the e-mail field has a suitable training structure nome@provider.net/.com/
    * 
    */
  public class GenericUserRegisterInputModel {
    [MaxLength(20)]
    [StringLength(20)]
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public string MaritalStatus { get; set; }

    [MaxLength(250)]
    [StringLength(250)]
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public string Name { get; set; }

    [MaxLength(250)]
    [StringLength(250)]
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public string Adress { get; set; }

    [MaxLength(7)]
    [StringLength(7)]
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public string Rg { get; set; }
    [MaxLength(11)]
    [StringLength(11)]
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public string Cpf { get; set; }

    [MaxLength(15)]
    [StringLength(15)]
    [DataType(DataType.PhoneNumber)]
    [Required(ErrorMessage = "This field is strictly necessary. Fill in correctly.")]
    public string PhoneNumber { get; set; }

    [EmailAddress]
    [MaxLength(250)]
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
  }
}
