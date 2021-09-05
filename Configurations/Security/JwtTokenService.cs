using ApiPJ.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiPJ.Configurations.Security {
  public class JwtTokenService : IAuthenticationService {
    private readonly IConfiguration _configuration;
    public JwtTokenService(IConfiguration config) {
      _configuration = config;
    }

    // This method is responsible for generating an authentication token for the user to remain active on the platform
    public string GenerateToken(Customer inputModel) {
      var secret = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfigurations:Secret").Value);
      var symmetricSecurityKey = new SymmetricSecurityKey(secret);
      var securityTokenDescriptor = new SecurityTokenDescriptor {
        Subject = new ClaimsIdentity(new Claim[]
          {
            new Claim(ClaimTypes.NameIdentifier, inputModel.Id.ToString()),
            new Claim(ClaimTypes.Name, inputModel.Name.Trim().Split()[0].ToString()),
            new Claim(ClaimTypes.Email, inputModel.Email.ToString())
          }),
        Expires = DateTime.UtcNow.AddDays(1),
        SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
      };
      var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
      var tokenGenerated = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
      var token = jwtSecurityTokenHandler.WriteToken(tokenGenerated);
      return token;
    }
  }
}
