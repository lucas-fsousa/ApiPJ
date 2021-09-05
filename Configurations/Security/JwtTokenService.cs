using ApiPJ.Entities;
using ApiPJ.Models.GenericUser;
using ApiPJ.Models.Login;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiPJ.Configurations.Security {
  public class JwtTokenService : IAuthenticationService {
    private readonly IConfiguration _configuration;
    public JwtTokenService(IConfiguration config) {
      _configuration = config;
    }

    // This method is responsible for generating an authentication token for the user to remain active on the platform
    public string GenerateToken(Customer inputModel) {
      var secretCode = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfigurations:Secret").Value);
      var symetricSecurityKey = new SymmetricSecurityKey(secretCode);
      var securityTokenDescriptor = new SecurityTokenDescriptor {
        Subject = new ClaimsIdentity(new Claim[] {
          new Claim(ClaimTypes.NameIdentifier, inputModel.Cpf),
          new Claim(ClaimTypes.Name, inputModel.Name.Trim().Split()[0]),
          new Claim(ClaimTypes.Email, inputModel.Email)
        }),
        Expires = DateTime.UtcNow.AddHours(8),
        SigningCredentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
      };
      var jwtSecutiryTokenHandler = new JwtSecurityTokenHandler();
      var tokenGenerated = jwtSecutiryTokenHandler.CreateToken(securityTokenDescriptor);
      var token = jwtSecutiryTokenHandler.WriteToken(tokenGenerated);
      return token;
    }
  }
}
