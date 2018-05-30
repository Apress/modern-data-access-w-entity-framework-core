using EFC_Xamarin.iOS;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(Env))]
namespace EFC_Xamarin.iOS
{
 public class Env : IEnv
 {
  public string GetDbFolder()
  {
   return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
       "..", "Library");
  }
 }
}