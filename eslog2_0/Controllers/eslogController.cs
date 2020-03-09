using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eslog2_0.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eslog2_0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class eslogController : ControllerBase
    {

        [HttpPost]
        [Route("create")]
        public IActionResult createEslog(/*[FromBody] EslogData eslogData*/)
        {
            EslogData eslogData = new EslogData();

            return Ok(eslog.constructEslog(eslogData));
        }
    }
}