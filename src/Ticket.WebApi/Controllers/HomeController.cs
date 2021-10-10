using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ticket.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public List<string> Get()
        {
            var result = new List<string>
            {
                "Payam",
                "Ali",
                "Sina"
            };
            return result;
        }
    }
}
