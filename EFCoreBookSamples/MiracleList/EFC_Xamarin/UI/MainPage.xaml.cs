using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace EFC_Xamarin
{
 public partial class MainPage : ContentPage
 {
  private ObservableCollection<Task> _Tasks { get; set; }
  public ObservableCollection<Task> Tasks
  {
   get { return _Tasks; }
   set { _Tasks = value; this.OnPropertyChanged(nameof(Tasks)); }
  }

  private string _Statustext { get; set; }
  public string Statustext
  {
   get { return _Statustext; }
   set { _Statustext = value; this.OnPropertyChanged(nameof(Statustext)); }
  }

  public MainPage()
  {
   this.BindingContext = this;
   //C_Tasks.SetBinding(ListView.ItemsSourceProperty, new Binding { Source = Tasks });
   InitializeComponent();
   var count = this.LoadTaskSet();
   SetStatus(count + " Datensätze geladen!");
  }

  private async System.Threading.Tasks.Task<int> LoadTaskSet()
  {
   using (var db = new EFContext())
   {
    var list = await db.TaskSet.OrderBy(x => x.Date).ToListAsync();
    Tasks = new ObservableCollection<Task>(list);
    return Tasks.Count;
   }
  }

  private void SetStatus(string text)
  {
   string dbstatus;
   using (var db = new EFContext())
   {
    dbstatus = db.TaskSet.Count() + " Tasks with " + db.TaskDetailSet.Count() + " Task Details";
   }
   Statustext = text + " / Database Status: " + dbstatus + ")";
  }

  private async void Add(object sender, EventArgs e)
  {

   if (String.IsNullOrEmpty(C_Task.Text)) return;
   // Create new Task
   var t = new Task { Title = C_Task.Text, Date = C_Datum.Date };
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
    var count = db.SaveChangesAsync();

    SetStatus(count + " records saved!");
    await this.LoadTaskSet();
   }
   this.C_Task.Text = "";
   this.C_Task.Focus();
  }

  private async void SetDone(object sender, EventArgs e)
  {
   // Get TaskID 
   var id = (int)((sender as Button).CommandParameter);
   // Remove record
   using (var db = new EFContext())
   {
    Task t = db.TaskSet.Include(x => x.Details).SingleOrDefault(x => x.TaskID == id);
    if (t == null) return; // not found!
    db.Remove(t);
    int count = await db.SaveChangesAsync();
    SetStatus(count + " records deleted!");
    await this.LoadTaskSet();
   }
  }

  private async void ShowDetails(object sender, EventArgs e)
  {
   // Get TaskID 
   var id = (int)((sender as Button).CommandParameter);
   // Get Details
   using (var db = new EFContext())
   {
    string s = "";
    Task t = db.TaskSet.Include(x => x.Details).SingleOrDefault(x => x.TaskID == id);
    s += "Task: " + t.Title + "\n\n";
    s += "Due: " + String.Format("{0:dd.MM.yyyy}", t.Date) + "\n\n";
    foreach (var d in t.Details)
    {
     s += "- " + d.Text + "\n";
    }
    SetStatus("Details for Task #" + id);
    await this.DisplayAlert("Details for Task #" + id, s, "OK");
   }
  }

  private async void RemoveAll(object sender, EventArgs e)
  {
   // Remove all tasks
   using (var db = new EFContext())
   {
    // Alternative 1: unefficient :-(
    //foreach (var b in db.TaskSet.ToList())
    //{
    // db.Remove(b);
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