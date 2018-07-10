using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using EFC_UWP.DAL;
using Windows.UI.ViewManagement;

namespace EFC_UWP
{
 /// <summary>
 /// Main page of the app
 /// </summary>
 public sealed partial class MainPage : Page, INotifyPropertyChanged
 {
  public MainPage()
  {
   this.DataContext = this;
   this.InitializeComponent();
   Windows.UI.ViewManagement.ApplicationView.PreferredLaunchViewSize = new Size(800, 500);
   Windows.UI.ViewManagement.ApplicationView.PreferredLaunchWindowingMode = Windows.UI.ViewManagement.ApplicationViewWindowingMode.PreferredLaunchViewSize;
   
   System.Diagnostics.Debug.WriteLine(ApplicationData.Current.LocalFolder.Path);

   ApplicationView appView = ApplicationView.GetForCurrentView();
   appView.Title = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " (" + DAL.EFContext.Provider + ")";

 

  }
  public event PropertyChangedEventHandler PropertyChanged;

  private ObservableCollection<Task> _Tasks { get; set; }
  public ObservableCollection<Task> Tasks
  {
   get { return _Tasks; }
   set { _Tasks = value; this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tasks))); }
  }

  private string _Statustext { get; set; }
  public string Statustext
  {
   get { return _Statustext; }
   set { _Statustext = value; this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Statustext))); }
  }

  private async void Page_Loaded(object sender, RoutedEventArgs e)
  {
   var count = await this.LoadTaskSet();
   SetStatus(count + " records loaded!");
  }

  private void SetStatus(string text)
  {
   string dbstatus;
   using (var db = new EFContext())
   {
    dbstatus = db.TaskSet.Count() + " tasks with " + db.TaskDetailSet.Count() + " task details in "+ db.DBInfo;
   }
   Statustext = text + " / DB: " + dbstatus + " / App: " + ApplicationData.Current.LocalFolder.Path;
  }

  /// <summary>
  /// Get all tasks from database
  /// </summary>
  /// <returns></returns>
  private async System.Threading.Tasks.Task<int> LoadTaskSet()
  {
   using (var db = new EFContext())
   {
    var list = await db.TaskSet.OrderBy(x => x.Date).ToListAsync();
    Tasks = new ObservableCollection<Task>(list);
    return Tasks.Count;
   }
  }

  private async void Add(object sender, RoutedEventArgs e)
  {
   if (String.IsNullOrEmpty(C_Task.Text)) return;
   if (!C_Datum.Date.HasValue) { C_Datum.Date = DateTime.Now; }

   // Create new Task
   var t = new Task { Title = C_Task.Text, Date = C_Datum.Date.Value.Date };
   var d1 = new TaskDetail() { Text = "Plan" };
   var d2 = new TaskDetail() { Text = "Execute" };
   var d3 = new TaskDetail() { Text = "Run Retrospective" };
   // Alternative 1
   //t.Details.Add(d1);
   //t.Details.Add(d2);
   //t.Details.Add(d3);

   // Alternative 2
   t.Details.AddRange(new List<TaskDetail>() { d1, d2, d3 });

   using (var db = new EFContext())
   {
    db.TaskSet.Add(t);
    // Save now!
    var count = await db.SaveChangesAsync();
    SetStatus(count + " records saved!");
    await this.LoadTaskSet();
   }
   this.C_Task.Text = "";
   this.C_Task.Focus(FocusState.Pointer);
  }

  private async void SetDone(object sender, RoutedEventArgs e)
  {
   // Get TaskID 
   var id = (int)((sender as Button).CommandParameter);
   // Remove record
   using (var db = new EFContext())
   {
    Task t = db.TaskSet.SingleOrDefault(x => x.TaskID == id);
    if (t == null) return; // not found :-(
    db.Remove(t);
    var count = await db.SaveChangesAsync();
    SetStatus(count + " records deleted!");
    await this.LoadTaskSet();
   }
  }

  private async void ShowDetails(object sender, RoutedEventArgs e)
  {
   // Get TaskID 
   var id = (int)((sender as Button).CommandParameter);
   // Get Details
   using (var db = new EFContext())
   {
    string s = "";
    Task t = db.TaskSet.Include(x => x.Details).SingleOrDefault(x => x.TaskID == id);
    s += "Task: " + t.Title + "\n\n";
    s += "Due: " + String.Format("{0:dd.MM.yyyy}",t.Date) + "\n\n";
    foreach (var d in t.Details)
    {
     s += "- " + d.Text + "\n";
    }
    SetStatus("Details for task #" + id);
    await new MessageDialog(s, "Details for task #" + id).ShowAsync();
   }
  }

  private void C_Task_KeyDown(object sender, KeyRoutedEventArgs e)
  {
   if (e.Key == Windows.System.VirtualKey.Enter) Add(null, null);
  }

  private async void RemoveAll(object sender, RoutedEventArgs e)
  {
   // Remove all tasks
   using (var db = new EFContext())
   {
    // Alternative 1: unefficient :-(
    //foreach (var t in db.TaskSet.ToList())
    //{
    // db.Remove(t);
    //}
    //db.SaveChanges();

    // Alternative 2: efficient!
    //db.Database.ExecuteSqlCommand("Delete from TaskDetailSet");
    var count = await db.Database.ExecuteSqlCommandAsync("Delete from TaskSet");
    SetStatus(count + " records deleted!");
    Tasks = null;
   }
  }
 }
}