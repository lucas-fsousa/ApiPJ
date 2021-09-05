using ApiPJ.Business.Methods;
using ApiPJ.Business.Repository.EmployeeDefinition;
using ApiPJ.Configurations.Security;
using ApiPJ.Entities;
using ApiPJ.Models.Login;
using Business.Methods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Controllers.V1 {
  [Route("api/v1/[controller]")]
  [ApiController]
  public class EmployeeController : ControllerBase {

    private readonly IEmployeeRepository _employee;
    private readonly IAuthenticationService _authentication;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IEmployeeRepository employee, IAuthenticationService authentication, ILogger<EmployeeController> logger) {
      _employee = employee;
      _authentication = authentication;
      _logger = logger;
    }

    [FilterValidState]
    [HttpPost, Route("login")]
    public async Task<IActionResult> LogIn(LoginInputViewModel credentials) {
      try {
        var recebeteste = await _employee.GetUser(credentials.Cpf);

        // The password is encrypted with a concatenation of the password entered with the unique cpf
        credentials.Password = (credentials.Password + credentials.Cpf).EncodePassword().Trim();

        var result = await _employee.LogIn(credentials);
        if(result == null) {
          return BadRequest();
        }

        var token = _authentication.GenerateToken(result);
        var returnAuthentication = new LoginOutputViewModel {
          AuthenticationToken = token,
          Cpf = result.Cpf,
          FirstName = result.Name.Trim().Split()[0]
        };
        return Ok(returnAuthentication);
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return new StatusCodeResult(500);
      }

    }
  }
}
