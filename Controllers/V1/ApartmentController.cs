using ApiPJ.Business.Repository.ApartmentDefinition;
using ApiPJ.Entities;
using ApiPJ.Models.Apartments;
using ApiPJ.Models.BlackoutDates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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

    //[Authorize]
    [HttpPost, Route("register")]
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
          Price = inputModel.Price
        };

        await _apartment.Register(apartment);
        await _apartment.Commit();

        return Ok();
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return new StatusCodeResult(500);
      }
    }

    


    [HttpGet, Route("getApartments")]
    public async Task<IActionResult> GetApartments() {
      try {

        var result = await _apartment.GetApartments();
        return Ok(result);
      } catch(Exception ex) {
        string a = " " + ex;
      }
      return BadRequest();
    }





  }
}
