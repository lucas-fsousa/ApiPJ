using ApiPJ.Entities;
using ApiPJ.Models.Login;

namespace ApiPJ.Configurations.Security {
  public interface IAuthenticationService {
    public string GenerateToken(Customer inputModel);
  }
}
