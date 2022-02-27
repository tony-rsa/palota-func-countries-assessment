using Palota.Assessment.Countries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp1.Functions
{
    internal class ProcessResponse
    {
        public object GetList(dynamic data)
        {
            List<object> lstCountries = new List<object>();
            foreach (dynamic country in data)
            {
                var location = new object();
                try { location = new Location {
                        Lattitude = country.latlng[0],
                        Longitude = country.latlng[1] };
                } catch (Exception ex) { }
                
                lstCountries.Add((object)new Country
                {
                    Name = country.name,
                    Iso3Code = country.alpha3Code,
                    Capital = country.capital,
                    Subregion = country.subregion,
                    Region = country.region,
                    Population = country.population,
                    Location = location,
                    Demonym = country.demonym,
                    NativeName = country.nativeName,
                    NumericCode = country.numericCode,
                    Flag = country.flags.svg
                });
            }
            return lstCountries;
        }
    }
}
