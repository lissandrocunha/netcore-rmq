using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMQ.API.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMQ.Microservice1.API.Controllers
{    
    [ApiController]
    public class ServicosController : BaseController
    {

        [HttpGet]
        public ActionResult Get()
        {

            return CustomResponse("Teste OK!");
        }
    }
}
