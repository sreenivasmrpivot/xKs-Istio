using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

using Autofac.Extras.DynamicProxy;

namespace CalculatorSvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Intercept(typeof(CallLogger))]
    public class CalculatorController : ControllerBase
    {
        // GET api/Calculator/Version
        [HttpGet("Version")]
        public virtual ActionResult<string> GetVersion()
        {
            return "v2";
        }

        // GET api/Calculator/Sum/5/3
        [HttpGet("Sum/{addendA}/{addendB}")]
        public virtual ActionResult<double> GetSum(double addendA, double addendB)
        {
            return addendA + addendB;
        }

        // GET api/Calculator/Difference/5/3
        [HttpGet("Difference/{minuend}/{subtrahend}")]
        public virtual ActionResult<double> GetDifference(double minuend, double subtrahend)
        {
            return minuend - subtrahend;
        }

        // GET api/Calculator/Product/5/3
        [HttpGet("Product/{multiplier}/{multiplicant}")]
        public virtual ActionResult<double> GetProduct(double multiplier, double multiplicant)
        {
            return multiplier * multiplicant;
        }


        // GET api/Calculator/Product/5/3
        [HttpGet("Quotient/{dividend}/{divisor}")]
        public virtual ActionResult<double> GetQuotient(double dividend, double divisor)
        {
            // return dividend * divisor; // wrong version - v1
            return dividend / divisor; // correct version - v2
        }

        // GET api/Calculator/NthRoot/27/3
        [HttpGet("NthRoot/{number}/{root}")]
        public virtual ActionResult<double> GetNthRoot(double number, double root)
        {
            string uri = string.Format("https://scientificcalculator.azurewebsites.net/api/Calculate/NthRoot/{0}/{1}",number,root);
            var client = new HttpClient();
            var result = client.GetStringAsync(uri).Result;

            return double.Parse(result);

        }
    }
}
