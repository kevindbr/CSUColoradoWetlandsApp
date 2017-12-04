using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;

namespace PortableApp
{

    public class WetlandSettingRepository : DBConnection
	{

        public string StatusMessage { get; set; }

        public WetlandSettingRepository()
		{
            // Create the Wetland Setting table (only if it's not yet created)
            //conn.DropTable<WetlandSetting>();
            conn.CreateTable<WetlandSetting>();
            SeedDB();
        }

        // return a list of Wetlandplants saved to the WetlandSetting table in the database
        public List<WetlandSetting> GetAllWetlandSettings()
        {
            return (from s in conn.Table<WetlandSetting>() select s).ToList();
        }

        // get a list of image settings stored in the local database
        public List<WetlandSetting> GetAllImageSettings()
        {
            return conn.Table<WetlandSetting>().Where(s => s.name.Equals("ImagesZipFile")).ToList();
        }

        // get an individual setting based on its name
        public WetlandSetting GetSetting(string settingName)
        {
            return conn.Table<WetlandSetting>().FirstOrDefault(s => s.name.Equals(settingName));
        }

        // (async) get an individual setting based on its name
        public async Task<WetlandSetting> GetSettingAsync(string settingName)
        {

             var setting = await connAsync.Table<WetlandSetting>().Where(s => s.name.Equals(settingName)).FirstOrDefaultAsync();

            if (setting != null)
                return setting;

            else
                return null;
        }

        public WetlandSetting GetImageZipFileSetting(string fileName)
        {
            return conn.Table<WetlandSetting>().Where(s => s.valuetext.Equals(fileName)).FirstOrDefault();
        }

        // add a setting
        public void AddSetting(WetlandSetting setting)
        {
            try
            {
                if (string.IsNullOrEmpty(setting.name))
                    throw new Exception("Valid setting name required");

                var result = conn.Insert(setting);
                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, setting);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add/update {0}. Error: {1}", setting, ex.Message);
            }

        }

        // add a setting async
        public async Task AddSettingAsync(WetlandSetting setting)
        {
            try
            {
                if (string.IsNullOrEmpty(setting.name))
                    throw new Exception("Valid setting name required");

                var result = await connAsync.InsertAsync(setting);
                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, setting);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add/update {0}. Error: {1}", setting, ex.Message);
            }

        }

        // add or update a setting
        public void AddOrUpdateSetting(WetlandSetting setting)
        {
            try
            {
                if (string.IsNullOrEmpty(setting.name))
                    throw new Exception("Valid setting name required");

                var result = conn.InsertOrReplace(setting);
                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, setting);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add/update {0}. Error: {1}", setting, ex.Message);
            }

        }

        // add or update a setting
        public async Task AddOrUpdateSettingAsync(WetlandSetting setting)
        {
            try
            {
                if (string.IsNullOrEmpty(setting.name))
                    throw new Exception("Valid setting name required");

                var result = await connAsync.InsertOrReplaceAsync(setting);
                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, setting);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add/update {0}. Error: {1}", setting, ex.Message);
            }
            
        }

        // Seed database with essential settings
        public void SeedDB()
        {
            if (GetSetting("Sort Field") == null)
                conn.Insert(new WetlandSetting { name = "Sort Field", valuetext = "Scientific Name", valueint = 0 });
            if (GetSetting("Download Images") == null)
                conn.Insert(new WetlandSetting { name = "Download Images", valuebool = false });
            if (GetSetting("Date Plants Downloaded") == null)
                conn.Insert(new WetlandSetting { name = "Date Plants Downloaded", valuetimestamp = new DateTime(2000, 1, 1) });
        }

    }
}