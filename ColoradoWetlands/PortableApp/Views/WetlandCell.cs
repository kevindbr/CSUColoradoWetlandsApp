using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PortableApp.Models;

namespace PortableApp.Views
{
    public class WetlandCell : ViewCell
    {
        readonly CachedImage cachedImage = null;

        public WetlandCell()
        {
            cachedImage = new CachedImage();
            View = cachedImage;
        }

        protected override void OnBindingContextChanged()
        {
            var item = BindingContext as WetlandImage;

            if (item == null)
                return;

            cachedImage.Source = item.ImageUrl;

            base.OnBindingContextChanged();
        }
    }

}