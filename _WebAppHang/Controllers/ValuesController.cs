using _WebAppHang.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _WebAppHang.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IGuidService _guidService;

        public ValuesController(IGuidService guidService)
        {
            _guidService = guidService;
        }



        [HttpGet]
        public ActionResult Get()
        {
            var result = _guidService.GetRandomIdentifier();

            return Ok(result);
        }

       
    }
}
