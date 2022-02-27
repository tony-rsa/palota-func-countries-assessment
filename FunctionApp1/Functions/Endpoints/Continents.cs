using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using Palota.Assessment.Countries.Models;

namespace FunctionApp1.Functions
{
    public static class Continents
    {
        private static HttpClient httpClient = new HttpClient();

        [FunctionName("Continents")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, nameof(HttpMethods.Get), Route = "continents/{continentName}/countries/")] HttpRequest req,
            ILogger log, string continentName)
        {
            List<object> lstCountries = new List<object>();
            try
            {
                string api_url = Environment.GetEnvironmentVariable("COUNTRIES_API_URL")
                                    + "/" + Environment.GetEnvironmentVariable("GetByContinent") + "/" + continentName;
                var response = await httpClient.GetAsync(api_url);

                log.LogInformation("API_URL: " + api_url);
                log.LogInformation("IsSuccessStatusCode: " + response.IsSuccessStatusCode.ToString());

                if (!response.IsSuccessStatusCode)
                {
                    return new BadRequestObjectResult(new Payload
                    {
                        Message = "The continent with name '"+continentName+"' could not be found."
                    });
                }

                dynamic data = JsonConvert.DeserializeObject(
                    await new StreamReader(response.Content.ReadAsStream()).ReadToEndAsync());

                lstCountries = new ProcessResponse().GetList(data);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Some Error: " + ex.ToString());
            }
            return new OkObjectResult(lstCountries);
        }
    }
}
