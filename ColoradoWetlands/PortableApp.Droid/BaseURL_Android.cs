using Xamarin.Forms;
using PortableApp.Droid;

[assembly: Dependency(typeof(BaseUrl_Android))]
namespace PortableApp.Droid
{
    public class BaseUrl_Android : IBaseUrl
    {
        public string Get()
        {
            return "file:///android_asset/";
        }
    }
}