using ApiPJ.Entities;
using System;

namespace ApiPJ.Models.UserBase {
  public abstract class GenericOutputModel {
    public string MaritalStatus { get; set; }
    public string Name { get; set; }
    public string Rg { get; set; }
    public string Cpf { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Sex { get; set; }
    public DateTime BirthDate { get; set; }
    public FullAdress Adress { get; set; }
  }
}
