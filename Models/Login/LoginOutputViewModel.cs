using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Models.Login {
  public class LoginOutputViewModel {
    public string AuthenticationToken { get; set; }
    public string FirstName { get; set; }
    public string Cpf { get; set; }
  }
}
