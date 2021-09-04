using ApiPJ.Business.Repository.GenericUserDefinition;
using ApiPJ.Entities;
using ApiPJ.Error;
using ApiPJ.Models.GenericUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Methods;
using ApiPJ.Business.Methods;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace ApiPJ.Controllers.V1 {
  [Route("api/v1/[controller]")]
  [ApiController]
  public class GenericUserController : ControllerBase {
    private readonly IGenericUserRepository _genericUser;
    private readonly ILogger<GenericUserController> _logger;
    public GenericUserController(IGenericUserRepository genericUserRepository, ILogger<GenericUserController> logger) {
      _genericUser = genericUserRepository;
    }

    /// <summary>
    /// Allows you to create a new system user who is not registered
    /// </summary>
    /// <param name="registerInputModel"></param>
    /// <returns>May return Ok(code 200), BadRequest(code 400) or internal error(code 500)</returns>
    [HttpPost]
    [Route("register")]
    [SwaggerResponse(statusCode: 200, description: "Registration was completed successfully.")]
    [SwaggerResponse(statusCode: 400, description: "Necessary to fill in the fields correctly.")]
    [SwaggerResponse(statusCode: 500, description: "Internal Error.")]
    [FilterValidState]
    public async Task<IActionResult> Register([FromBody]GenericUserRegisterInputModel registerInputModel) {
      try {
        //Checks if the user exists by validating the information.
        var user = await _genericUser.GetUser(registerInputModel.Cpf);
        if(user != null) {
          return BadRequest("This user cannot be registered. Reasons: Exists, is locked or is invalid.");
        }

        //Definitely creates the user
        user = new GenericUser {
          BirthDate = registerInputModel.BirthDate,
          Cpf = registerInputModel.Cpf,
          Email = registerInputModel.Email,
          MaritalStatus = registerInputModel.MaritalStatus,
          Name = registerInputModel.Name,
          PhoneNumber = registerInputModel.PhoneNumber,
          Rg = registerInputModel.Rg,
          Sex = registerInputModel.Sex,

          //To ensure that the password has a unique HASH, two strings are concatenated, the CPF identifier and the password itself defined by the user
          Password = $"{registerInputModel.Password + registerInputModel.Cpf}".EncodePassword(),

          Adress = new FullAdress {
            PublicPlace = registerInputModel.Adress.PublicPlace,
            Reference = registerInputModel.Adress.Reference,
            Street = registerInputModel.Adress.Street
          }
        };

        // tries to insert the data into the database and if everything goes well, the information is committed
        await _genericUser.Register(user);
        await _genericUser.Commit();

        return Ok("This user has been successfully registered."); 
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return new StatusCodeResult(500);
        throw;
      }
    }
    /// <summary>
    /// You can delete a user based on the CPF(unique) informed.
    /// </summary>
    /// <param name="cpf"></param>
    /// <returns>May return Ok(code 200), notFound(code 404) or internal error(code 500)</returns>
    [Route("delete")]
    //[Authorize]
    [HttpDelete()]
    [SwaggerResponse(statusCode: 200, description: "Deletion completed successfully.")]
    [SwaggerResponse(statusCode: 404, description: "User not found / Does not exist.")]
    [SwaggerResponse(statusCode: 500, description: "Internal Error.")]
    public async Task<IActionResult> Delete(string cpf) {
      try {

        // query the database to see if the user is valid and then check if the result is null
        var user = await _genericUser.GetUser(cpf);
        if(user == null) {
          return NotFound("The request was not completed. Apparent reason: Does not exist");
        }

        //if the user is valid, it is deleted and then the information is committed.
        _genericUser.Delete(user);
        await _genericUser.Commit();
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return new StatusCodeResult(500);
      }
      return Ok("This user has been successfully deleted.");
    }

  }
}
