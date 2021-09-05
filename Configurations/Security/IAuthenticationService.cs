using ApiPJ.Entities;
using ApiPJ.Entities.Base;
using ApiPJ.Models.Login;

namespace ApiPJ.Configurations.Security {
  public interface IAuthenticationService {
    public string GenerateToken(GenericUserBase inputModel);
  }
}
