using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthServer.Models;
using AuthServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthServer.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IDataService _dataService;

        public AuthController(IDataService dataService)
        {
            _dataService = dataService;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return Ok(await _dataService.GetUsers());
        }
    }
}