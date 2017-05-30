using System;
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
        public WetlandPlantImagesPage(WetlandPlant plant)
        {
            InitializeComponent();
            ItemsSource = plant.Images;
        }
    }
}
