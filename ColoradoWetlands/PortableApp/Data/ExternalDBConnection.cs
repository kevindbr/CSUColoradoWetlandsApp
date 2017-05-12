using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PortableApp.Models;
using System.Collections.ObjectModel;
using System.IO;

namespace PortableApp
{
    public class ExternalDBConnection
    {
        const string Url = "http://sdt1.cas.colostate.edu/mobileapi/api/wetland";
        public const string localUrl = "http://129.82.38.57:61045/api/wetland";
        private string authorizationKey;
        HttpClient client = new HttpClient();

        private async Task<HttpClient> GetClient()
        {
            if (string.IsNullOrEmpty(authorizationKey))
            {
                authorizationKey = await client.GetStringAsync(Url + "login");
                authorizationKey = JsonConvert.DeserializeObject<string>(authorizationKey);
            }

            client.DefaultRequestHeaders.Add("Authorization", authorizationKey);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        public async Task<IEnumerable<WetlandPlant>> GetAllPlants()
        {
            string result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<IList<WetlandPlant>>(result);
        }
        
        public async Task<WetlandSetting> GetDateUpdatedDataOnServer()
        {
            string result = await client.GetStringAsync(Url + "_settings/DatePlantDataUpdatedOnServer");
            return JsonConvert.DeserializeObject<WetlandSetting>(result);
        }

        public async Task<IEnumerable<WetlandSetting>> GetImageZipFileSettings()
        {
            string result = await client.GetStringAsync(localUrl + "_settings/images");
            return JsonConvert.DeserializeObject<IList<WetlandSetting>>(result);
        }

        //public List<Puma> GetAllPumas()
        //{
        //    // return a list of Puma Types saved to the table in the database
        //    return (from p in conn.Table<Puma>() select p).ToList();
        //}
    }
}
