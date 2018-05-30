using EFC_Xamarin.UWP;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(Env))]
namespace EFC_Xamarin.UWP
{
 public class Env : IEnv
 {
        public string GetDbFolder()
        {
            return ApplicationData.Current.LocalFolder.Path;
        }
    }
}