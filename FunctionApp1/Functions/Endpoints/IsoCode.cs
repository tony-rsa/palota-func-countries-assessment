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
    public static class IsoCode
    {
        private static HttpClient httpClient = new HttpClient();

        [FunctionName("IsoCode")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, nameof(HttpMethods.Get), Route = "countries/{iso3Code}")] HttpRequest req,
            ILogger log, string iso3code)
        {
            List<object> lstCountries = new List<object>();
            try
            {
                string api_url = Environment.GetEnvironmentVariable("COUNTRIES_API_URL")
                                    + "/" + Environment.GetEnvironmentVariable("GetByCode")+iso3code;
                var response = await httpClient.GetAsync(api_url);

                log.LogInformation("API_URL: " + api_url);
                log.LogInformation("IsSuccessStatusCode: " + response.IsSuccessStatusCode.ToString());

                if (!response.IsSuccessStatusCode)
                {
                    return new BadRequestObjectResult(new Payload
                    {
                        Message = "The country with ISO 3166 Alpha 3 code '"+iso3code+"' could not be found."
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
            return new OkObjectResult(lstCountries[0]);
        }
    }
}
