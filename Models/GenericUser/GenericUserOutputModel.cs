using ApiPJ.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Models.GenericUser {
  public class GenericUserOutputModel {
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
