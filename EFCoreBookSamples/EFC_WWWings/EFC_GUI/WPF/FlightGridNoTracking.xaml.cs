using BO;
using DA;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI.WPF
{
 public partial class FlightGridNoTracking : Window
 {

  public FlightGridNoTracking()
  {
   InitializeComponent();
   this.Title = this.Title + "- Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
  }

  private void SetStatus(string s)
  {
   this.C_Status.Content = s;
  }

  WWWingsContext ctx;

  /// <summary>
  /// Load flights
  /// </summary>
  private void C_Load_Click(object sender, RoutedEventArgs e)
  {
   ctx = new WWWingsContext();
   // Clear grid 
   this.C_flightDataGrid.ItemsSource = null;
   // Get departure
   string Ort = this.C_City.Text.ToString();
   // Show status
   SetStatus("Loading with " + this.C_Mode.Text + "...");

   // Prepare query
   var q = ctx.FlightSet.AsQueryable();
   if (this.C_Mode.Text == "NoTracking") q = q.AsNoTracking();
   if (Ort != "All") q = (from f in q where f.Departure == Ort select f);

   if (Int32.TryParse(this.C_Count.Text, out int count))
   {
    if (count>0) q = q.Take(count);
   }


   var sw = new Stopwatch();
   sw.Start();
   // Execute query
   var fluege = q.ToList();
   sw.Stop();

   // Databinding to grid
   this.C_flightDataGrid.ItemsSource = fluege; // Local is empty at NoTracking;

   // Status setzen
   SetStatus(fluege.Count() + " loaded records using " + this.C_Mode.Text + ": " + sw.ElapsedMilliseconds + "ms!");
  }

  /// <summary>
  /// Save the changed flights
  /// </summary>
  private void C_Save_Click(object sender, RoutedEventArgs e)
  {
   // Get changes and ask
   var added = from x in ctx.ChangeTracker.Entries() where x.State == EntityState.Added select x;
   var del = from x in ctx.ChangeTracker.Entries() where x.State == EntityState.Deleted select x;
   var mod = from x in ctx.ChangeTracker.Entries() where x.State == EntityState.Modified select x;

   if (MessageBox.Show("Do you want to save the following changes?\n" + String.Format("Client: Changed: {0} New: {1} Deleted: {2}", mod.Count(), added.Count(), del.Count()), "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.No) return;

   string Ergebnis = "";

   // Save
   Ergebnis = ctx.SaveChanges().ToString();

   // Show status
   SetStatus("Number of saved changes: " + Ergebnis);
  }

  /// <summary>
  /// Called when starting to editing a flight in the grid
  /// </summary>
  private void C_flightDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
  {
   // Zugriff auf aktuellen editierten Flight
   var flight = (Flight)e.Row.Item;

   if (flight.FlightNo > 0) // important so that new flights are not added before filling
   {
    // Attach may only be done if the object is not already attached!
    if (!ctx.FlightSet.Local.Any(x => x.FlightNo == flight.FlightNo))
    {
     ctx.FlightSet.Attach(flight);
     SetStatus($"Flight {flight.FlightNo} can now be edited!");
    }
   }
  }

  /// <summary>
  /// Called when deleting a flight in the grid
  /// </summary>
  private void C_flightDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
  {
   var flight = (Flight)((DataGrid)sender).CurrentItem;

   if (e.Key == Key.Delete)
   {
    // Attach may only be done if the object is not already attached!
    if (!ctx.FlightSet.Local.Any(x => x.FlightNo == flight.FlightNo))
    {
     ctx.FlightSet.Attach(flight);
    }

    ctx.FlightSet.Remove(flight);
    SetStatus($"Flight {flight.FlightNo} can be deleted!");
   }
  }

  /// <summary>
  /// Called when adding a flight in the grid
  /// </summary>
  private void C_flightDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
  {
   var flight = (Flight)e.Row.Item;
   if (!ctx.FlightSet.Local.Any(x => x.FlightNo == flight.FlightNo))
   {
    ctx.FlightSet.Add(flight);
    SetStatus($"Flight {flight.FlightNo} has bee added!");
   }
  }

  private void C_Test_Click(object sender, RoutedEventArgs e)
  {
   try
   {
    ctx = new WWWingsContext();
    var flight = ctx.FlightSet.FirstOrDefault();
    if (flight == null) MessageBox.Show("No flights :-(", "Test Connection", MessageBoxButton.OK, MessageBoxImage.Warning);
    else MessageBox.Show("OK!", "Test Connection", MessageBoxButton.OK, MessageBoxImage.Information);
   }
   catch (Exception ex)
   {
    MessageBox.Show("Error: " + ex.ToString(), "Test Connection", MessageBoxButton.OK, MessageBoxImage.Error);
   }

  }
 }
}