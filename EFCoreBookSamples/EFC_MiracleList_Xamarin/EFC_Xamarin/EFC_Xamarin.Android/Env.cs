using EFC_Xamarin.Android;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(Env))]
namespace EFC_Xamarin.Android
{
 public class Env : IEnv
 {
  public string GetDbFolder()
  {
   return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
  }
 }
}