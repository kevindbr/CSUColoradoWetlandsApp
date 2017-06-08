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
using System.Net.Http.Headers;
using System.Diagnostics;

namespace PortableApp
{
    public class ExternalDBConnection
    {
        // Declare variables
        public string Url = (Debugger.IsAttached == true) ? "http://129.82.38.57:61045/api/wetland" : "http://sdt1.cas.colostate.edu/mobileapi/api/wetland";
        HttpClient client = new HttpClient();
        private string result;
        private Stream resultStream;

        // Set headers for client
        public ExternalDBConnection()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", "p4OqMiplghVdWPbVv5rx84jdlskdJk*jdlsKDIE84");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }
        
        public async Task<IEnumerable<WetlandPlant>> GetAllPlants()
        {
            result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<IList<WetlandPlant>>(result);
        }
        
        public async Task<WetlandSetting> GetDateUpdatedDataOnServer()
        {
            try
            {
                result = await client.GetStringAsync(Url + "_settings/DatePlantDataUpdatedOnServer");
            }
            catch (Exception e)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<WetlandSetting>(result);
        }

        public async Task<IEnumerable<WetlandSetting>> GetImageZipFileSettings()
        {
            result = await client.GetStringAsync(Url + "_settings/images");
            return JsonConvert.DeserializeObject<IList<WetlandSetting>>(result);
        }

        public async Task<Stream> GetImageZipFiles(string imageFileToDownload)
        {
            resultStream = await client.GetStreamAsync(Url + "/image_zip_files/" + imageFileToDownload);
            return resultStream;
        }
        
    }
}
