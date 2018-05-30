using BO;
using ITVisions.EFCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;


namespace GUI.WindowsForms
{
 public partial class FlugPassagierMasterDetail : Form
 {
  // Direkter DB-Zugriff
  DA.WWWingsContext ctx = new DA.WWWingsContext();

  // Nutzung der GL
  //FlugManager fm = new FlugManager();

  public FlugPassagierMasterDetail()
  {
   InitializeComponent();
   // Variante 1: Hier geht aber Anfügen und Löschen nicht
   //this.flugBindingSource.DataSource = ctx.FlightSet.Take(10).ToList();

   // Variante 2: Hier geht auch Anfügen und Löschen!!!
   ctx.FlightSet.Take(10).ToList();
   this.flugBindingSource.DataSource = ctx.FlightSet.Local.ToBindingList();

   // Variante 3: Mehrschichtig!
   //this.flugBindingSource.DataSource = fm.GetFlightSet("Rom", "");

  }

  private void FlugPassagierMasterDetail_Load(object sender, EventArgs e)
  {

  }

  private void flugBindingNavigatorSaveItem_Click(object sender, EventArgs e)
  {
   try
   {
    //var flugSet = this.flugBindingSource.DataSource as List<Flight>;
    //var anz = fm.Save(flugSet);
    MessageBox.Show(ctx.ChangeTracker.GetStatistics(), "Speichern");
    var anz = ctx.SaveChanges();

    //MessageBox.Show(anz + " gespeicherte Änderungen!", "Speichern");
   }
   catch (Exception ex)
   {

    MessageBox.Show(ex.Message, "Fehler beim Speichern", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
   }


  }

  private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
  {
   //fm.RemoveFlug(this.flugDataGridView.CurrentRow.DataBoundItem as Flight);
  }

  private void C_Orte_TextUpdate(object sender, EventArgs e)
  {
   // Das ist NICHT richtig: ctx.FlightSet.Local.Clear();
   ctx = new DA.WWWingsContext();
   string ort = this.C_Orte.Text;

   var set = ctx.FlightSet.Where(f => f.Departure.ToLower().Contains(ort.ToLower())).ToList();

   this.Text = set.Count + " Flüge from " + ort;
   this.flugBindingSource.DataSource = ctx.FlightSet.Local.ToBindingList();
  }

  private void C_Orte_Click(object sender, EventArgs e)
  {

  }
 }
}
