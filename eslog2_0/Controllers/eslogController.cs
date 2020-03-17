using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eslog2_0.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Eslog_1_6;

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
            try
            {
                return Ok(eslog.constructEslog(eslogData));
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpPost]
        [Route("convert")]
        public IActionResult convertFrom16eSlog([FromForm] string eslogFile)
        {


            Eslog_1_6.Parse parse = new Eslog_1_6.Parse();
            var doc = parse.DeserializeEslog16(eslogFile);

            Mapper mapper = new Mapper();
            var eslog = mapper.mapEslog(doc);

            return Ok(eslog);
        }
    }
}