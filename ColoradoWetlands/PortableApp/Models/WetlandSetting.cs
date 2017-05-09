﻿using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PortableApp.Models
{
    [Table("wetland_settings")]
    public class WetlandSetting
    {
        [PrimaryKey, AutoIncrement]
        public int settingid { get; set; }
        [Unique]
        public string name { get; set; }
        public DateTime? valuetimestamp { get; set; }
        public string valuetext { get; set; }
        public decimal? valueamount { get; set; }
        public bool? valuebool { get; set; }

    }
}