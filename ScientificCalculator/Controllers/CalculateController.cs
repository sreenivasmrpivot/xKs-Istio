using System;
using Microsoft.AspNetCore.Mvc;

namespace ScientificCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculateController : ControllerBase
    {
        // GET api/values/5
        [HttpGet("Round/{input}")]
        public ActionResult<double> GetRound(double input)
        {
            return Math.Round(input);
        }

        // GET api/values/5
        [HttpGet("NthRoot/{number}/{root}")]
        public ActionResult<double> GetNthRoot(double number, double root)
        {
            return Math.Pow(number, 1 / root);
        }
    }
}
