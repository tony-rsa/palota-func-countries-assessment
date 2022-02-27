using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using Palota.Assessment.Countries.Models;

namespace FunctionApp1.Functions
{
    public static class borders
    {
        private static HttpClient httpClient = new HttpClient();

        [FunctionName("borders")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "countries/{iso3Code}/borders")] HttpRequest req,
            ILogger log, string iso3code)
        {
            List<object> lstCountries = new List<object>();
            try
            {
                string api_url = Environment.GetEnvironmentVariable("COUNTRIES_API_URL")
                                    + "/" + Environment.GetEnvironmentVariable("GetByCode");
                var response = await httpClient.GetAsync(api_url + iso3code);

                log.LogInformation("API_URL: " + api_url);
                log.LogInformation("IsSuccessStatusCode: " + response.IsSuccessStatusCode.ToString());

                if (!response.IsSuccessStatusCode)
                {
                    return new BadRequestObjectResult(new Payload
                    {
                        Message = "The country with ISO 3166 Alpha 3 code '" + iso3code + "' could not be found."
                    });
                }

                dynamic data = JsonConvert.DeserializeObject(
                    await new StreamReader(response.Content.ReadAsStream()).ReadToEndAsync());
                
                foreach (var border in data[0].borders)
                {
                    var res = await httpClient.GetAsync(api_url + (string)border);
                    dynamic country = new ProcessResponse().GetList(JsonConvert.DeserializeObject(
                        await new StreamReader(res.Content.ReadAsStream()).ReadToEndAsync()));
                    lstCountries.Add(country[0]);
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Some Error: " + ex.ToString());
            }
            return new OkObjectResult(lstCountries);
        }
    }
}
