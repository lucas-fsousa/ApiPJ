using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPJ.Controllers.V1 {
  [Route("api/v1/[controller]")]
  [ApiController]
  //[Authorize]
  public class ManagementController : ControllerBase {

    [HttpGet, Route("GETT")]
    public async Task<IActionResult> GETTESTE() {
      return Ok("SUCESSO PARÇA!");
    }
  }
}
