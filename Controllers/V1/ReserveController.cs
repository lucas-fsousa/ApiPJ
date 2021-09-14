using ApiPJ.Business.Methods;
using ApiPJ.Business.Repository.ApartmentDefinition;
using ApiPJ.Business.Repository.CustomerDefinition;
using ApiPJ.Business.Repository.ReserveDefintion;
using ApiPJ.Entities;
using ApiPJ.Models.Reserve;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiPJ.Controllers.V1 {
  [Route("api/v1/[controller]")]
  [ApiController]
  public class ReserveController : ControllerBase {

    private readonly IApartmentRepository _apartmentRepository;
    private readonly IReserveRepository _reserveRepository;
    private readonly ILogger<ReserveController> _logger;
    public ReserveController(IReserveRepository repository, ILogger<ReserveController> logger, IApartmentRepository apartmentRepository) {
      _reserveRepository = repository;
      _logger = logger;
      _apartmentRepository = apartmentRepository;
    }

    /// <summary>
    /// Returns a list of paged reservations with 10 results per page
    /// </summary>
    /// <returns></returns>
    [HttpGet, Route("getReserves")]
    [SwaggerResponse(statusCode: 500, "The request was not completed due to an internal error on the server side.")]
    [SwaggerResponse(statusCode: 404, "The requested resource was not found")]
    [SwaggerResponse(statusCode: 200, "The request was successfully completed.", typeof(List<Reserve>))]
    public async Task<IActionResult> GetReserves(int CurrentPage) {
      try {
        var reserves = await _reserveRepository.GetReserves(CurrentPage, "");
        if(reserves == null) {
          return BadRequest("The request was invalid.");
        }
        return Ok(reserves);
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return StatusCode(500, "The request was not completed due to an internal error on the server side.");
      }
    }

    /// <summary>
    ///  returns a paged reserve list with up to 10 records per page based on the CPF of the entered user
    /// </summary>
    /// <param name="CurrentPage"></param>
    /// <param name="customerCpf"></param>
    /// <returns></returns>
    [HttpGet, Route("getReservesByCpf/{customerCpf}")]
    [SwaggerResponse(statusCode: 500, "The request was not completed due to an internal error on the server side.")]
    [SwaggerResponse(statusCode: 404, "The requested resource was not found")]
    [SwaggerResponse(statusCode: 200, "The request was successfully completed.", typeof(List<Reserve>))]
    public async Task<IActionResult> GetReservesByCpf(int CurrentPage, string customerCpf) {
      try {
        var reserves = await _reserveRepository.GetReserves(CurrentPage, customerCpf);
        if(reserves == null) {
          return BadRequest("The request was invalid.");
        }
        return Ok(reserves);
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return StatusCode(500, "The request was not completed due to an internal error on the server side.");
      }
    }


    /// <summary>
    /// Returns a reservation based on the given identifier
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet, Route("getReserve/{id}")]
    [SwaggerResponse(statusCode: 500, "The request was not completed due to an internal error on the server side.")]
    [SwaggerResponse(statusCode: 404, "The requested resource was not found")]
    [SwaggerResponse(statusCode: 200, "The request was successfully completed.", typeof(Reserve))]
    public async Task<IActionResult> GetReserve(int id) {
      try {
        var reserve = await _reserveRepository.GetReserve(id);
        if(reserve == null) {
          return NotFound("The requested resource was not found.");
        }
        return Ok(reserve);
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return new StatusCodeResult(500);
      }
    }


    /// <summary>
    /// add a new apartment reservation
    /// </summary>
    /// <param name="inputModel"></param>
    /// <returns></returns>
    [HttpPost, Route("register")]
    [SwaggerResponse(statusCode: 500, "The request was not completed due to an internal error on the server side.")]
    [SwaggerResponse(statusCode: 400, "The requested resource was not found")]
    [SwaggerResponse(statusCode: 200, "The request was successfully completed.")]
    [FilterValidState]
    public async Task<IActionResult> Register([FromBody] ReserveInputModel inputModel) {
      try {
        var ValidateInputIds = await _apartmentRepository.GetApartment(inputModel.IdApartment);
        if(ValidateInputIds == null) {
          return BadRequest("The request was invalid.");
        }

        var newReserve = new Reserve {
          IdApartment = inputModel.IdApartment,
          IdCustomer = inputModel.IdCustomer,
          TotalPrice = inputModel.TotalPrice,
          FinalDate = inputModel.FinalDate,
          InitialDate = inputModel.InitialDate
        };

        await _reserveRepository.Insert(newReserve);
        await _reserveRepository.Commit();
        return Ok("The request was successfully completed.");
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return StatusCode(500, "The request was not completed due to an internal error on the server side.");
      }
    }

    /// <summary>
    /// Updates the information of an existing reservation based on new data entries
    /// </summary>
    /// <param name="id"></param>
    /// <param name="inputModel"></param>
    /// <returns></returns>
    [HttpPut, Route("updateReserve/{id}")]
    [SwaggerResponse(statusCode: 500, "The request was not completed due to an internal error on the server side.")]
    [SwaggerResponse(statusCode: 400, "The requested resource was not found")]
    [SwaggerResponse(statusCode: 200, "The request was successfully completed.")]
    [FilterValidState]
    public async Task<IActionResult> UpdateReserve(int id, [FromBody] ReserveUpdateInputModel inputModel) {
      try {
        var oldReserve = await _reserveRepository.GetReserve(id);
        if(oldReserve == null) {
          return BadRequest("The request was invalid.");
        }

        var newReserve = new Reserve { 
          InitialDate = inputModel.InitialDate == oldReserve.InitialDate ? oldReserve.InitialDate : inputModel.InitialDate,
          IdApartment = inputModel.IdApartment == oldReserve.IdApartment ? oldReserve.IdApartment : inputModel.IdApartment,
          TotalPrice = inputModel.TotalPrice == oldReserve.TotalPrice ? oldReserve.TotalPrice : inputModel.TotalPrice,
          IdCustomer = inputModel.IdCustomer == oldReserve.IdCustomer ? oldReserve.IdCustomer : inputModel.IdCustomer,
          FinalDate = inputModel.FinalDate == oldReserve.FinalDate ? oldReserve.FinalDate : inputModel.FinalDate,
          Id = oldReserve.Id
        };

        await _reserveRepository.Update(newReserve);
        await _reserveRepository.Commit();

        return Ok("The request was successfully completed.");
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return StatusCode(500, "The request was not completed due to an internal error on the server side.");
      }
    }

    /// <summary>
    /// Delete an existing reservation based on the entered identifier
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete, Route("deleteReserve/{id}")]
    [SwaggerResponse(statusCode: 500, "The request was not completed due to an internal error on the server side.")]
    [SwaggerResponse(statusCode: 400, "The request was invalid.")]
    [SwaggerResponse(statusCode: 200, "The request was successfully completed.")]
    public async Task<IActionResult> DeleteReserve(int id) {
      try {
        var reserve = await _reserveRepository.GetReserve(id);
        if(reserve == null) {
          return BadRequest("The request was invalid.");
        }
        _reserveRepository.Delete(reserve);
        await _reserveRepository.Commit();
        return Ok("The request was successfully completed.");
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return StatusCode(500, "The request was not completed due to an internal error on the server side.");
      }
    }
  }
}
