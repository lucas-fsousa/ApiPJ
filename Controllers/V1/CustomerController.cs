using ApiPJ.Business.Methods;
using ApiPJ.Business.Repository.CustomerDefinition;
using ApiPJ.Configurations.Security;
using ApiPJ.Entities;
using ApiPJ.Models.Customer;
using ApiPJ.Models.Login;
using Business.Methods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiPJ.Controllers.V1 {
  [Route("api/v1/[controller]")]
  [ApiController]
  public class CustomerController : ControllerBase {
    private readonly ICustomerRepository _customer;
    private readonly ILogger<CustomerController> _logger;
    private readonly IAuthenticationService _authentication;

    public CustomerController(ICustomerRepository genericUserRepository, ILogger<CustomerController> logger, IAuthenticationService authentication) {
      _customer = genericUserRepository;
      _logger = logger;
      _authentication = authentication;
    }

    /// <summary>
    /// Authenticates the user according to the credentials entered.
    /// </summary>
    /// <param name="credentials"></param>
    /// <returns>May return Ok(code 200), BadRequest(code 400) or internal error(code 500)</returns>
    [SwaggerResponse(statusCode: 200, description: "The request was successfully completed.", Type = typeof(LoginOutputViewModel))]
    [SwaggerResponse(statusCode: 400, description: "The request was invalid. Check the parameters and try again.")]
    [SwaggerResponse(statusCode: 500, description: "The request was not completed due to an internal error on the server side.")]
    [FilterValidState]
    [HttpPost, Route("login")]
    public async Task<IActionResult> LogIn(LoginInputViewModel credentials) {
      try {
        var result = await _customer.LogIn(credentials);
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
    public async Task<IActionResult> Register([FromBody]CustomerInputModel registerInputModel) {
      try {
        //Checks if the user exists by validating the information.
        var user = await _customer.GetUser(registerInputModel.Cpf);
        if(user != null) {
          return BadRequest("This user cannot be registered. Reasons: Exists, is locked or is invalid.");
        }

        if(!registerInputModel.Cpf.ValidateCPF()) {
          return BadRequest("Invalid CPF.");
        }

        //Definitely creates the user
        user = new Customer {
          BirthDate = registerInputModel.BirthDate,
          Cpf = registerInputModel.Cpf,
          Email = registerInputModel.Email,
          MaritalStatus = registerInputModel.MaritalStatus,
          Name = registerInputModel.Name,
          PhoneNumber = registerInputModel.PhoneNumber,
          Rg = registerInputModel.Rg,
          Sex = registerInputModel.Sex,
          Password = registerInputModel.Password,

          Adress = new FullAdress {
            PublicPlace = registerInputModel.Adress.PublicPlace,
            Reference = registerInputModel.Adress.Reference,
            Street = registerInputModel.Adress.Street
          }
        };

        // tries to insert the data into the database and if everything goes well, the information is committed
        await _customer.Register(user);
        await _customer.Commit();

        return Ok("This user has been successfully registered."); 
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return new StatusCodeResult(500);
      }
    }

    /// <summary>
    /// You can delete a user based on the CPF(unique) informed.
    /// </summary>
    /// <param name="cpf"></param>
    /// <returns>May return Ok(code 200), notFound(code 404), Unauthorized(code 401) or internal error(code 500)</returns>
    [HttpDelete, Route("delete")]
    //[Authorize]   
    [SwaggerResponse(statusCode: 200, description: "The request was successfully completed.")]
    [SwaggerResponse(statusCode: 401, description: "The request did not include an authentication token or the authentication token was expired.")]
    [SwaggerResponse(statusCode: 404, description: "The requested resource was not found")]
    [SwaggerResponse(statusCode: 500, description: "The request was not completed due to an internal error on the server side.")]
    public async Task<IActionResult> Delete(string cpf) {
      try {

        // query the database to see if the user is valid and then check if the result is null
        var user = await _customer.GetUser(cpf);
        if(user == null) {
          return NotFound("The request was not completed. Apparent reason: Does not exist");
        }

        //if the user is valid, it is deleted and then the information is committed.
        _customer.Delete(user);
        await _customer.Commit();
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
    [HttpPut, Route("update")]
    //[Authorize]
    [SwaggerResponse(statusCode: 200, description: "The request was successfully completed.")]
    [SwaggerResponse(statusCode: 401, description: "The request did not include an authentication token or the authentication token was expired.")]
    [SwaggerResponse(statusCode: 400, description: "The request was invalid. Check the parameters and try again.")]
    [SwaggerResponse(statusCode: 500, description: "The request was not completed due to an internal error on the server side.")]
    [FilterValidState]
    public async Task<IActionResult> Update(string cpf, CustomerUpdateInputModel userUpdate) {
      try {
        var oldUser = await _customer.GetUser(cpf);
        if(oldUser == null) {
          return NotFound("The request was not completed. Apparent reason: Does not exist");
        }

        //Definitely updates the user
        var update = new Customer {
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
          Password = $"{userUpdate.Password + cpf}".Trim().EncodePassword() == oldUser.Password ? oldUser.Password: $"{userUpdate.Password + cpf}".Trim().EncodePassword(),

          Adress = new FullAdress {
            PublicPlace = userUpdate.Adress.PublicPlace == oldUser.Adress.PublicPlace ? oldUser.Adress.PublicPlace : userUpdate.Adress.PublicPlace,
            Reference = userUpdate.Adress.Reference == oldUser.Adress.Reference ? oldUser.Adress.Reference : userUpdate.Adress.Reference,
            Street = userUpdate.Adress.Street == oldUser.Adress.Street ? oldUser.Adress.Street : userUpdate.Adress.Street
          }
        };

        await _customer.Update(update);
        await _customer.Commit();

        return Ok("User update completed successfully.");
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return new StatusCodeResult(500);
      }
    }

    /// <summary>
    /// Returns a user according to the CPF(unique) entered
    /// </summary>
    /// <param name="cpf"></param>
    /// <returns>May return Ok(code 200), notFound(code 404), Unauthorized(code 401) or internal error(code 500)</returns>
    [HttpGet, Route("getCustomer")]
    //[Authorize]
    [SwaggerResponse(statusCode: 500, description: "The request was not completed due to an internal error on the server side.")]
    [SwaggerResponse(statusCode: 401, description: "The request did not include an authentication token or the authentication token was expired.")]
    [SwaggerResponse(statusCode: 404, description: "The requested resource was not found")]
    [SwaggerResponse(statusCode: 200, description: "User located in database", Type = typeof(CustomerOutputModel))]
    public async Task<IActionResult> GetUser(string cpf) {
      try {

        // checks if the requested user is registered.
        var user = await _customer.GetUser(cpf);
        if(user == null) {
          return NotFound("The request was not completed. Apparent reason: Does not exist");
        }

        // Definition of the object that will be returned
        var userOutPut = new CustomerOutputModel {

          Adress = user.Adress,
          BirthDate = user.BirthDate,
          Cpf = user.Cpf,
          Email = user.Email,
          MaritalStatus = user.MaritalStatus,
          Name = user.Name,
          PhoneNumber = user.PhoneNumber,
          Rg = user.Rg,
          Sex = user.Sex
        };

        return Ok(userOutPut);
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return new StatusCodeResult(500);
      }
    }


    /// <summary>
    /// does a paged search and returns a list of 10 users
    /// </summary>
    /// <param name="currentPage"></param>
    /// <returns>May return Ok(code 200), Unauthorized(code 401), badRequest(code 400) or internal error(code 500)</returns>
    //[Authorize]
    [HttpGet, Route("getCustomers")]
    //[Authorize]
    [SwaggerResponse(statusCode: 500, description: "The request was not completed due to an internal error on the server side.")]
    [SwaggerResponse(statusCode: 401, description: "The request did not include an authentication token or the authentication token was expired.")]
    [SwaggerResponse(statusCode: 400, description: "The request was invalid. Check the parameters and try again.")]
    [SwaggerResponse(statusCode: 200, description: "User located in database", Type = typeof(List<CustomerOutputModel>))]
    public async Task<IActionResult> GetAllUser(int currentPage) {
      try {

        // performs a search in the base and then checks if the result of the list is null
        var list = await _customer.GetAllUser(currentPage);
        if(list == null) {
          return BadRequest("Oops. The request failed. Try again in a few minutes.");
        }

        var newReturn = new List<CustomerOutputModel>();
        foreach(var user in list) {
          var userFound = new CustomerOutputModel {
            Adress = user.Adress,
            BirthDate = user.BirthDate,
            Cpf = user.Cpf,
            Email = user.Email,
            MaritalStatus = user.MaritalStatus,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber,
            Rg = user.Rg,
            Sex = user.Sex
          };
          newReturn.Add(userFound);
        }
        return Ok(newReturn);
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return new StatusCodeResult(500);
      }
    }
  }
}
