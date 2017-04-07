﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using PortableApp.Models;

using Xamarin.Forms;

namespace PortableApp.Views
{
    public partial class WetlandPlantImagesPage : CarouselPage
    {
        public WetlandPlantImagesPage(int plantId)
        {
            InitializeComponent();
            ObservableCollection<WetlandPlantImage> plantImages = new ObservableCollection<WetlandPlantImage>(App.WetlandPlantImageRepo.PlantImages(plantId));
            ItemsSource = plantImages;
        }
    }
}