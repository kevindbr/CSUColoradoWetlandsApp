using Xamarin.Forms;
using PortableApp.iOS;
using Foundation;

[assembly: Dependency (typeof (BaseUrl_iOS))]

namespace PortableApp.iOS
{
    public class BaseUrl_iOS : IBaseUrl
    {
        public string Get()
        {
            return NSBundle.MainBundle.BundlePath;
        }
    }
}