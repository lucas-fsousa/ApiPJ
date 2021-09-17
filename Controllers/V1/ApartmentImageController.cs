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

namespace ApiPJ.Controllers.V1 {
  [Route("api/[controller]")]
  [ApiController]
  public class ApartmentImageController : ControllerBase {
    private readonly ILogger<ApartmentImageController> _logger;
    private readonly IApartmentImageRepository _apartmentImageRepository;
    private readonly IApartmentRepository _apartmentRepository;

    public ApartmentImageController(ILogger<ApartmentImageController> logger, IApartmentImageRepository imageRepository, IApartmentRepository apartmentRepository) {
      _logger = logger;
      _apartmentImageRepository = imageRepository;
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
          var path = Path.Combine(Directory.GetCurrentDirectory(), "Images", pathForDatabase);
          var stream = new FileStream(path, FileMode.Create);
          await file.CopyToAsync(stream);

          var newImage = new ImagePath {
            ApartmentId = apartmentId,
            Path = pathForDatabase
          };
          await _apartmentImageRepository.UploadImages(newImage);
          await _apartmentImageRepository.Commit();
        }

        return Ok("The request was successfully completed.");
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return StatusCode(500, "The request was not completed due to an internal error on the server side.");
      }
    }


    [HttpGet, Route("getImagesByApartmentId/{apartmentId}")]
    public async Task<IActionResult> GetImagesByAparmentId(int apartmentId) {
      try {
        var images = await _apartmentImageRepository.GetAllImagesByApartmentId(apartmentId);

        if(images == null) {
          return NotFound();
        }

        var physicalFiles = new List<PhysicalFileResult>();
        foreach(var image in images) {
          var imageInList = new ImagePathOutputModel();
          var diretorioInformacoes = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Images"));
          foreach(var item in diretorioInformacoes.GetFiles().Select(x => x).ToList()) {
            if(item.Name.Equals(image.Path)) {
              var gg = new PhysicalFileResult(Path.Combine(Directory.GetCurrentDirectory(), "Images", item.Name), imageInList.Exptention);
              gg.EnableRangeProcessing = true;
              physicalFiles.Add(gg);
              }
            }
          }
        return Ok(physicalFiles);
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return StatusCode(500, "The request was not completed due to an internal error on the server side.");
      }
    }



    /// <summary>
    /// Deletes an image or an array of images based on the record entered. Expected a list of ImagePath
    /// </summary>
    /// <param name="images"></param>
    /// <returns></returns>
    [HttpDelete, Route("deleteImages")]
    [SwaggerResponse(statusCode: 401, description: "The request did not include an authentication token or the authentication token was expired.")]
    [SwaggerResponse(statusCode: 400, description: "The request was invalid. Check the parameters and try again.")]
    [SwaggerResponse(statusCode: 200, description: "The request was successfully completed.")]
    [SwaggerResponse(statusCode: 500, description: "The request was not completed due to an internal error on the server side.")]
    public async Task<IActionResult> DeleteImages(List<ImagePath> images) {
      try {
        if(images == null || images.Count < 1) {
          return BadRequest("The request was invalid.");
        }

        foreach(var image in images) {
          var pathForDatabase = image.Path;
          var path = Path.Combine(Directory.GetCurrentDirectory(), pathForDatabase);
          System.IO.File.Delete(path);
          var imageForDelete = new ImagePath {
            ApartmentId = image.ApartmentId,
            Path = pathForDatabase,
            Id = image.Id
          };
          _apartmentImageRepository.DeleteImage(imageForDelete);
          await _apartmentImageRepository.Commit();
        }
        return Ok("The request was successfully completed.");
      } catch(Exception ex) {
        _logger.LogError(ex.Message);
        return StatusCode(500, "The request was not completed due to an internal error on the server side.");
      }
    }
  }
}
