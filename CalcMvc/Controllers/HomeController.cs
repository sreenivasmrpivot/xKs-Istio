using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CalcMvc.Models;

using System.Net.Http;
using Autofac.Extras.DynamicProxy;

namespace CalcMvc.Controllers
{
    // Autofac Interceptor - mark class for interception
    [Intercept(typeof(CallLogger))]
    public class HomeController : Controller
    {
        // Autofac Interceptor - mark methods to be intercepted as virtual
        public virtual IActionResult Index()
        {
            return View();
        }

        // Autofac Interceptor - mark methods to be intercepted as virtual
        public virtual IActionResult Privacy()
        {
            return View();
        }

        // Autofac Interceptor - mark methods to be intercepted as virtual
        public virtual IActionResult Calculator(Operation model)
        {

            // string baseuri = "http://calculatorsvc.smanyamr.com/api/Calculator/";
            string baseuri = "http://calculatorsvc.microservices.svc.cluster.local/api/Calculator/";
            // string baseuri = "https://localhost:5001/api/Calculator/";
            string action = string.Empty;

            switch (model.OperationType)
            {
                case OperationType.Addition:
                    action = "sum";
                    break;
                case OperationType.Subtraction:
                    action = "difference";
                    break;
                case OperationType.Multiplication:
                    action = "product";
                    break;
                case OperationType.Division:
                    action = "quotient";
                    break;
                case OperationType.NthRoot:
                    action = "nthroot";
                    break;
            }

            string uri = baseuri + action + "/" + model.NumberA + "/" + model.NumberB;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("country", "india");

            var result = client.GetStringAsync(uri).Result;

            model.Result = double.Parse(result);
            model.Version = client.GetStringAsync(baseuri + "Version").Result;;

            return View( model );
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // Autofac Interceptor - mark methods to be intercepted as virtual
        public virtual IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
