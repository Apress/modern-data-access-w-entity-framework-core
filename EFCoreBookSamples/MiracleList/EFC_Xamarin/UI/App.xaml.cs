using Xamarin.Forms;

//[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace EFC_Xamarin
{
 public partial class App : Application
 {
  public App()
  {
   InitializeComponent();
   // Create Database if it does not exist
   using (var db = new EFContext())
   {
    db.Database.EnsureCreated();
   }
   MainPage = new EFC_Xamarin.MainPage();
  }

  protected override void OnStart()
  {

  }

  protected override void OnSleep()
  {
   // Handle when your app sleeps
  }

  protected override void OnResume()
  {
   // Handle when your app resumes
  }
 }
}
