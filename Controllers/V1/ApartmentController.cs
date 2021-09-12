using ApiPJ.Business.Methods;
using ApiPJ.Business.Repository.ApartmentDefinition;
using ApiPJ.Entities;
using ApiPJ.Models.Apartments;
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
  public class ApartmentController : ControllerBase {
    private readonly ILogger<ApartmentController> _logger;
    private readonly IApartmentRepository _apartment;

    public ApartmentController(ILogger<ApartmentController> logger, IApartmentRepository apartment) {
      _logger = logger;
      _apartment = apartment;
    }

    /// <summary>
    /// can create a new "apartment" item
    /// </summary>
    /// <param name="inputModel"></param>
    /// <returns></returns>
    //[Authorize]
    [HttpPost, Route("register")]
    [SwaggerResponse(statusCode: 401, description: "The request did not include an authentication token or the authentication token was expired.")]
    [SwaggerResponse(statusCode: 200, description: "The request was successfully completed.", Type = typeof(Apartment))]
    [SwaggerResponse(statusCode: 400, description: "The request was invalid. Check the parameters and try again.")]
    [SwaggerResponse(statusCode: 500, description: "The request was not completed due to an internal error on the server side.")]
    [FilterValidState]
    public async Task<IActionResult> Register(ApartmentInputModel inputModel) {
      try {
        var apartment = new Apartment {
          Localization = inputModel.Localization,
          Available = true,
          Bedrooms = inputModel.Bedrooms,
          City = inputModel.City,
          Description = inputModel.Description,
          MaximumPeoples = inputModel.MaximumPeoples,
          ParkingLots = inputModel.ParkingLots,
          DailyPrice = inputModel.DailyPrice
        };

        await _apartment.Register(apartment);
        await _apartment.Commit();

        return Ok();
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return new StatusCodeResult(500);
      }
    }



    /// <summary>
    /// Returns a list with all apartments and their availability.
    /// </summary>
    /// <returns></returns>
    //[Authorize]
    [HttpGet, Route("getApartments")]
    [SwaggerResponse(statusCode: 401, description: "The request did not include an authentication token or the authentication token was expired.")]
    [SwaggerResponse(statusCode: 200, description: "The request was successfully completed.", Type = typeof(List<Apartment>))]
    [SwaggerResponse(statusCode: 500, description: "The request was not completed due to an internal error on the server side.")]
    public async Task<IActionResult> GetApartments() {
      try {
        var result = await _apartment.GetApartments();
        return Ok(result);
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return new StatusCodeResult(500);
      }
    }

    /// <summary>
    /// Deletes an apartment based on the name entered.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    //[Authorize]
    [HttpDelete, Route("delete/{id}")]
    [SwaggerResponse(statusCode: 401, description: "The request did not include an authentication token or the authentication token was expired.")]
    [SwaggerResponse(statusCode: 200, description: "The request was successfully completed.")]
    [SwaggerResponse(statusCode: 404, description: "The requested resource was not found.")]
    [SwaggerResponse(statusCode: 500, description: "The request was not completed due to an internal error on the server side.")]
    public async Task<IActionResult> Delete(int id) {
      try {
        var apartment = await _apartment.GetApartment(id);
        if(apartment == null) {
          return NotFound();
        }
        _apartment.Delete(apartment);
        await _apartment.Commit();
        return Ok();
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return new StatusCodeResult(500);
      }
    }

    /// <summary>
    /// Returns a single apartment and their availability.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    //[Authorize]
    [HttpGet, Route("getApartment/{id}")]
    [SwaggerResponse(statusCode: 401, description: "The request did not include an authentication token or the authentication token was expired.")]
    [SwaggerResponse(statusCode: 200, description: "The request was successfully completed.", Type = typeof(Apartment))]
    [SwaggerResponse(statusCode: 404, description: "The requested resource was not found.")]
    [SwaggerResponse(statusCode: 500, description: "The request was not completed due to an internal error on the server side.")]
    public async Task<IActionResult> GetApartment(int id) {
      try {
        var result = await _apartment.GetApartment(id);
        if(result == null) {
          return NotFound();
        }
        return Ok(result);
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return new StatusCodeResult(500);
      }
    }

    /// <summary>
    /// Update an apartment based on the entered identifier and entity to be updated
    /// </summary>
    /// <param name="id"></param>
    /// <param name="apartment"></param>
    /// <returns></returns>
    //[Authorize]
    [HttpPut, Route("update")]
    [SwaggerResponse(statusCode: 401, description: "The request did not include an authentication token or the authentication token was expired.")]
    [SwaggerResponse(statusCode: 200, description: "The request was successfully completed.")]
    [SwaggerResponse(statusCode: 500, description: "The request was not completed due to an internal error on the server side.")]
    [FilterValidState]
    public async Task<IActionResult> Update(int id, ApartmentUpdateInputmodel apartment) {
      try {
        var oldApartment = await _apartment.GetApartment(id);
        var newApartment = new Apartment {
          MaximumPeoples = apartment.MaximumPeoples == oldApartment.MaximumPeoples ? oldApartment.MaximumPeoples : apartment.MaximumPeoples,
          Localization = apartment.Localization == oldApartment.Localization ? oldApartment.Localization : apartment.Localization,
          Description = apartment.Description == oldApartment.Description ? oldApartment.Description : apartment.Description,
          ParkingLots = apartment.ParkingLots == oldApartment.ParkingLots ? oldApartment.ParkingLots : apartment.ParkingLots,
          Available = apartment.Available == oldApartment.Available ? oldApartment.Available : apartment.Available,
          Bedrooms = apartment.Bedrooms == oldApartment.Bedrooms ? oldApartment.Bedrooms : apartment.Bedrooms,
          DailyPrice = apartment.DailyPrice == oldApartment.DailyPrice ? oldApartment.DailyPrice : apartment.DailyPrice,
          City = apartment.City == oldApartment.City ? oldApartment.City : apartment.City,
          Id = id
        };
        _apartment.Update(newApartment);
        await _apartment.Commit();
        return Ok();
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return new StatusCodeResult(500);
      }
    }
  }
}
