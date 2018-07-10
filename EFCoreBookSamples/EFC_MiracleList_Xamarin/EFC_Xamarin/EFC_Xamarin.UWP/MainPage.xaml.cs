using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace EFC_Xamarin.UWP
{
 public sealed partial class MainPage
 {
  public MainPage()
  {
   this.InitializeComponent();
   Windows.UI.ViewManagement.ApplicationView.PreferredLaunchViewSize = new Size(800, 500);
   Windows.UI.ViewManagement.ApplicationView.PreferredLaunchWindowingMode = Windows.UI.ViewManagement.ApplicationViewWindowingMode.PreferredLaunchViewSize;
   LoadApplication(new EFC_Xamarin.App());
  }
 }
}
