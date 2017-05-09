using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PortableApp.Models;
using System.Collections.ObjectModel;

namespace PortableApp
{
    public class ExternalDBConnection
    {
        const string Url = "http://sdt1.cas.colostate.edu/mobileapi/api/wetland";
        const string localUrl = "http://129.82.38.57:61045/api/wetland";
        private string authorizationKey;

        private async Task<HttpClient> GetClient()
        {
            HttpClient client = new HttpClient();
            if (string.IsNullOrEmpty(authorizationKey))
            {
                authorizationKey = await client.GetStringAsync(Url + "login");
                authorizationKey = JsonConvert.DeserializeObject<string>(authorizationKey);
            }

            client.DefaultRequestHeaders.Add("Authorization", authorizationKey);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        public async Task<IEnumerable<WetlandPlant>> GetAll()
        {
            // TODO: use GET to retrieve PumATypes
            //HttpClient client = await GetClient();
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<IList<WetlandPlant>>(result);
        }

        public async Task<IEnumerable<WetlandPlant>> Search()
        {
            // TODO: use GET to retrieve PumATypes
            //HttpClient client = await GetClient();
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<IEnumerable<WetlandPlant>>(result);
        }

        public async Task<ObservableCollection<WetlandPlantImage>> GetImages(int resourceId)
        {
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync("http://129.82.38.57:61045/api/Puma/" + resourceId + "/images");
            return JsonConvert.DeserializeObject<ObservableCollection<WetlandPlantImage>>(result);
        }

        public async Task<WetlandSetting> GetDateUpdatedDataOnServer()
        {
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync(localUrl + "_settings/DatePlantDataUpdatedOnServer");
            return JsonConvert.DeserializeObject<WetlandSetting>(result);
        }

        //public List<Puma> GetAllPumas()
        //{
        //    // return a list of Puma Types saved to the table in the database
        //    return (from p in conn.Table<Puma>() select p).ToList();
        //}
    }
}
