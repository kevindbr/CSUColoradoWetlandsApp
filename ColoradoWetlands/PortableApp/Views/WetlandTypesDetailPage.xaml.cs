using System;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class WetlandTypesDetailPage : ContentPage
    {
        public WetlandTypesDetailPage()
        {
            InitializeComponent();
        }

        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }
    }
}
