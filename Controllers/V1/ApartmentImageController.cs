using ApiPJ.Business.Methods;
using ApiPJ.Business.Repository.ApartmentDefinition;
using ApiPJ.Business.Repository.ApartmentImageDefinition;
using ApiPJ.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiPJ.Models.ImagePath;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Hosting.Internal;
using Business.Methods;

namespace ApiPJ.Controllers.V1 {
  [Route("api/[controller]")]
  [ApiController]
  public class ApartmentImageController : ControllerBase {
    private readonly ILogger<ApartmentImageController> _logger;
    //private readonly IApartmentImageRepository _apartmentImageRepository;
    private readonly IApartmentRepository _apartmentRepository;

    public ApartmentImageController(ILogger<ApartmentImageController> logger, IApartmentImageRepository imageRepository, IApartmentRepository apartmentRepository) {
      _logger = logger;
      //_apartmentImageRepository = imageRepository;
      _apartmentRepository = apartmentRepository;
    }


    /// <summary>
    /// includes images of the apartment
    /// </summary>
    /// <param name="apartmentId"></param>
    /// <param name="files"></param>
    /// <returns></returns>
    [SwaggerResponse(statusCode: 401, description: "The request did not include an authentication token or the authentication token was expired.")]
    [SwaggerResponse(statusCode: 400, description: "The request was invalid. Check the parameters and try again.")]
    [SwaggerResponse(statusCode: 200, description: "The request was successfully completed.")]
    [SwaggerResponse(statusCode: 500, description: "The request was not completed due to an internal error on the server side.")]
    [FilterValidState]
    [HttpPost, Route("uploadFiles/{apartmentId}")]
    public async Task<IActionResult> UploadFiles(int apartmentId, List<IFormFile> files) {
      try {
        var apartmentConfirm = await _apartmentRepository.GetApartment(apartmentId);
        if(files == null || files.Count < 1 || apartmentConfirm == null) {
          return BadRequest("The request was invalid.");
        }

        foreach(var file in files) {

          var pathForDatabase = Path.Combine($"{DateTime.Now.Ticks.ToString() + apartmentId}-{file.FileName}");
          var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "files", "images", pathForDatabase);
          var newImage = new ImagePath {
            ApartmentId = apartmentId,
            Path = pathForDatabase
          };
          await _apartmentRepository.UploadImages(newImage);
          await _apartmentRepository.Commit();
          var stream = new FileStream(path, FileMode.Create);
          await file.CopyToAsync(stream);
        }

        return Ok("The request was successfully completed.");
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return StatusCode(500, "The request was not completed due to an internal error on the server side.");
      }
    }

    /// <summary>
    /// makes the query in the image bank and returns all the images corresponding to the entered ID
    /// </summary>
    /// <param name="apartmentId"></param>
    /// <returns></returns>
    [SwaggerResponse(statusCode: 404, description: "The requested resource was not found")]
    [SwaggerResponse(statusCode: 401, description: "The request did not include an authentication token or the authentication token was expired.")]
    [SwaggerResponse(statusCode: 200, description: "The request was successfully completed.", Type = typeof(List<ImagePath>))]
    [SwaggerResponse(statusCode: 500, description: "The request was not completed due to an internal error on the server side.")]
    [HttpGet, Route("getImagesByApartmentId/{apartmentId}")]
    public async Task<IActionResult> GetImagesByAparmentId(int apartmentId) {
      try {
        //get image names
        var imagesNameReturned = await _apartmentRepository.GetAllImagesByApartmentId(apartmentId);
        if(imagesNameReturned == null || imagesNameReturned.Count < 1) {
          return NotFound("The requested resource was not found.");
        }

        var listImageUrl = Functions.GenerateImageUrl(imagesNameReturned, $"{HttpContext.Request.Host.Value}/files/images");
        return Ok(listImageUrl);
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return StatusCode(500, "The request was not completed due to an internal error on the server side.");
      }
    }



    /// <summary>
    /// Deletes an image or an array of images based on the record entered. Expected a list of ImagePath
    /// </summary>
    /// <param name="idImage"></param>
    /// <returns></returns>
    [HttpDelete, Route("deleteImages/{idImage}")]
    [SwaggerResponse(statusCode: 401, description: "The request did not include an authentication token or the authentication token was expired.")]
    [SwaggerResponse(statusCode: 400, description: "The request was invalid. Check the parameters and try again.")]
    [SwaggerResponse(statusCode: 200, description: "The request was successfully completed.")]
    [SwaggerResponse(statusCode: 500, description: "The request was not completed due to an internal error on the server side.")]
    public async Task<IActionResult> DeleteImages(int idImage) {
      try {
        var image = await _apartmentRepository.GetImageById(idImage);
        if(idImage < 1 || image == null) {
          return BadRequest("The request was invalid.");
        }

        _apartmentRepository.DeleteImage(image);
        System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "images", image.Path));
        await _apartmentRepository.Commit();
        return Ok("The request was successfully completed.");
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return StatusCode(500, "The request was not completed due to an internal error on the server side.");
      }
    }
  }
}
