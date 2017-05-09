﻿using System.Collections.Generic;
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
            conn.CreateTable<WetlandSetting>();
		}
        
        public List<WetlandSetting> GetAllWetlandSettings()
        {
            // return a list of Wetlandplants saved to the WetlandSetting table in the database
            return (from s in conn.Table<WetlandSetting>() select s).ToList();
        }

        public async Task<WetlandSetting> GetSettingAsync(string settingName)
        {
            return await connAsync.Table<WetlandSetting>().Where(s => s.name.Equals(settingName)).FirstOrDefaultAsync();
        }

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

    }
}