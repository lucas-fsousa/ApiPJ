﻿using ApiPJ.Business.Methods;
using ApiPJ.Business.Repository.ApartmentDefinition;
using ApiPJ.Business.Repository.ApartmentImageDefinition;
using ApiPJ.Entities;
using ApiPJ.Models.Apartments;
using Business.Methods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ApiPJ.Controllers.V1 {
  [Route("api/v1/[controller]")]
  [ApiController]
  //[Authorize]
  public class ApartmentController : ControllerBase {
    private readonly ILogger<ApartmentController> _logger;
    private readonly IApartmentRepository _apartment;
    private readonly IApartmentImageRepository _apartmentImage;

    public ApartmentController(ILogger<ApartmentController> logger, IApartmentRepository apartment, IApartmentImageRepository imageRepository) {
      _logger = logger;
      _apartmentImage = imageRepository;
      _apartment = apartment;
    }

    /// <summary>
    /// can create a new "apartment" item
    /// </summary>
    /// <param name="inputModel"></param>
    /// <returns></returns>
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
    [HttpGet, Route("getApartments")]
    [SwaggerResponse(statusCode: 401, description: "The request did not include an authentication token or the authentication token was expired.")]
    [SwaggerResponse(statusCode: 200, description: "The request was successfully completed.", Type = typeof(List<Apartment>))]
    [SwaggerResponse(statusCode: 500, description: "The request was not completed due to an internal error on the server side.")]
    public async Task<IActionResult> GetApartments() {
      try {
        var allApartments = await _apartment.GetApartments();
        foreach(var apartment in allApartments) {
          var imagesNameReturned = await _apartmentImage.GetAllImagesByApartmentId(apartment.IdAp);
          var listImageUrl = Functions.GenerateImageUrl(imagesNameReturned, $"{HttpContext.Request.Host.Value}");
          apartment.Images = listImageUrl;
        }
        return Ok(allApartments);
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
        var images = await _apartmentImage.GetAllImagesByApartmentId(id);
        await _apartment.Delete(apartment);
        
        foreach(var image in images) {
          System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "images", image.Path));
        }

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
        } else {
          var imagesNameReturned = await _apartmentImage.GetAllImagesByApartmentId(id);
          var listImageUrl = Functions.GenerateImageUrl(imagesNameReturned, $"{HttpContext.Request.Host.Value}");
          result.Images = listImageUrl;
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
          IdAp = id
        };
        await _apartment.Update(newApartment);
        await _apartment.Commit();
        return Ok();
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return new StatusCodeResult(500);
      }
    }
  }
}
