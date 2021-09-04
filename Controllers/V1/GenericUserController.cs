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
    [HttpPost, Route("register")]
    [SwaggerResponse(statusCode: 200, description: "The request was successfully completed.")]
    [SwaggerResponse(statusCode: 400, description: "The request was invalid. Check the parameters and try again.")]
    [SwaggerResponse(statusCode: 500, description: "The request was not completed due to an internal error on the server side.")]
    [FilterValidState]
    public async Task<IActionResult> Register([FromBody]GenericUserInputModel registerInputModel) {
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
    /// <returns>May return Ok(code 200), notFound(code 404), Unauthorized(code 401) or internal error(code 500)</returns>
    [HttpDelete, Route("delete/{cpf}")]
    //[Authorize]   
    [SwaggerResponse(statusCode: 200, description: "The request was successfully completed.")]
    [SwaggerResponse(statusCode: 401, description: "The request did not include an authentication token or the authentication token was expired.")]
    [SwaggerResponse(statusCode: 404, description: "The requested resource was not found")]
    [SwaggerResponse(statusCode: 500, description: "The request was not completed due to an internal error on the server side.")]
    public async Task<IActionResult> Delete([FromRoute]string cpf) {
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

    /// <summary>
    /// Update a user according to the CPF entered
    /// </summary>
    /// <param name="cpf"></param>
    /// <param name="userUpdate"></param>
    /// <returns>May return Ok(code 200), badRequest(code 400), Unauthorized(code 401) or internal error(code 500)</returns>
    [HttpPut, Route("update/{cpf}")]
    [SwaggerResponse(statusCode: 200, description: "The request was successfully completed.")]
    [SwaggerResponse(statusCode: 401, description: "The request did not include an authentication token or the authentication token was expired.")]
    [SwaggerResponse(statusCode: 400, description: "The request was invalid. Check the parameters and try again.")]
    [SwaggerResponse(statusCode: 500, description: "The request was not completed due to an internal error on the server side.")]
    [FilterValidState]
    public async Task<IActionResult> Update([FromRoute]string cpf, [FromBody]GenericUserUpdateInputModel userUpdate) {
      try {
        var oldUser = await _genericUser.GetUser(cpf);
        if(oldUser == null) {
          return NotFound("The request was not completed. Apparent reason: Does not exist");
        }

        //Definitely updates the user
        var update = new GenericUser {
          // Information is checked to identify if there has been a change. The change will only be applied if the update information is different from the old information

          // Ternary expressions for simple checking
          Email = userUpdate.Email == oldUser.Email ? oldUser.Email: userUpdate.Email,
          MaritalStatus = userUpdate.MaritalStatus == oldUser.MaritalStatus ? oldUser.MaritalStatus : userUpdate.MaritalStatus,
          Name = userUpdate.Name == oldUser.Name ? oldUser.Name : userUpdate.Name,
          PhoneNumber = userUpdate.PhoneNumber == oldUser.PhoneNumber ? oldUser.PhoneNumber : userUpdate.PhoneNumber,
          Sex = userUpdate.Sex == oldUser.Sex ? oldUser.Sex : userUpdate.Sex,
          BirthDate = oldUser.BirthDate,
          Cpf = cpf,
          Rg = oldUser.Rg,

          //To ensure that the password has a unique HASH, two strings are concatenated, the CPF identifier and the password itself defined by the user
          Password = $"{userUpdate.Password + cpf}".EncodePassword() == oldUser.Password ? oldUser.Password: $"{userUpdate.Password + cpf}".EncodePassword(),

          Adress = new FullAdress {
            PublicPlace = userUpdate.Adress.PublicPlace == oldUser.Adress.PublicPlace ? oldUser.Adress.PublicPlace : userUpdate.Adress.PublicPlace,
            Reference = userUpdate.Adress.Reference == oldUser.Adress.Reference ? oldUser.Adress.Reference : userUpdate.Adress.Reference,
            Street = userUpdate.Adress.Street == oldUser.Adress.Street ? oldUser.Adress.Street : userUpdate.Adress.Street
          }
        };

        await _genericUser.Update(update);
        await _genericUser.Commit();

        return Ok("User update completed successfully.");
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return new StatusCodeResult(500);
      }
      
      
    }

  }
}
