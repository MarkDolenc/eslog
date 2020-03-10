using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eslog2_0.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eslog2_0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class eslogController : ControllerBase
    {

        [HttpPost]
        [Route("create")]
        public IActionResult createEslog([FromBody] EslogData eslogData)
        {

            Console.WriteLine(JsonConvert.SerializeObject(eslogData));

            Console.WriteLine("");
            try
            {
                return Ok(eslog.constructEslog(eslogData));
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
    }
}