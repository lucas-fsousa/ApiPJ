using ApiPJ.Entities.Base;

namespace ApiPJ.Configurations.Security {
  public interface IAuthenticationService {
    public string GenerateToken(GenericUserBase inputModel);
  }
}
