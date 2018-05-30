namespace GUI.WindowsForms
{
 partial class FlugPassagierMasterDetail
 {
  /// <summary>
  /// Required designer variable.
  /// </summary>
  private System.ComponentModel.IContainer components = null;

  /// <summary>
  /// Clean up any resources being used.
  /// </summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
   if (disposing && (components != null))
   {
    components.Dispose();
   }
   base.Dispose(disposing);
  }

  #region Windows Form Designer generated code

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
   this.components = new System.ComponentModel.Container();
   System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlugPassagierMasterDetail));
   this.flugBindingSource = new System.Windows.Forms.BindingSource(this.components);
   this.flugBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
   this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
   this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
   this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
   this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
   this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
   this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
   this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
   this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
   this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
   this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
   this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
   this.flugBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
   this.C_Orte = new System.Windows.Forms.ToolStripComboBox();
   this.flugDataGridView = new System.Windows.Forms.DataGridView();
   this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
   this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   ((System.ComponentModel.ISupportInitialize)(this.flugBindingSource)).BeginInit();
   ((System.ComponentModel.ISupportInitialize)(this.flugBindingNavigator)).BeginInit();
   this.flugBindingNavigator.SuspendLayout();
   ((System.ComponentModel.ISupportInitialize)(this.flugDataGridView)).BeginInit();
   this.SuspendLayout();
   // 
   // flugBindingSource
   // 
   this.flugBindingSource.DataSource = typeof(BO.Flight);
   // 
   // flugBindingNavigator
   // 
   this.flugBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
   this.flugBindingNavigator.BindingSource = this.flugBindingSource;
   this.flugBindingNavigator.CountItem = this.bindingNavigatorCountItem;
   this.flugBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
   this.flugBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.flugBindingNavigatorSaveItem,
            this.C_Orte});
   this.flugBindingNavigator.Location = new System.Drawing.Point(0, 0);
   this.flugBindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
   this.flugBindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
   this.flugBindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
   this.flugBindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
   this.flugBindingNavigator.Name = "flugBindingNavigator";
   this.flugBindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
   this.flugBindingNavigator.Size = new System.Drawing.Size(1003, 25);
   this.flugBindingNavigator.TabIndex = 0;
   this.flugBindingNavigator.Text = "bindingNavigator1";
   // 
   // bindingNavigatorAddNewItem
   // 
   this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
   this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
   this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
   this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
   this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
   this.bindingNavigatorAddNewItem.Text = "Add new";
   // 
   // bindingNavigatorCountItem
   // 
   this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
   this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
   this.bindingNavigatorCountItem.Text = "of {0}";
   this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
   // 
   // bindingNavigatorDeleteItem
   // 
   this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
   this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
   this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
   this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
   this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
   this.bindingNavigatorDeleteItem.Text = "Delete";
   this.bindingNavigatorDeleteItem.Click += new System.EventHandler(this.bindingNavigatorDeleteItem_Click);
   // 
   // bindingNavigatorMoveFirstItem
   // 
   this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
   this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
   this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
   this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
   this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
   this.bindingNavigatorMoveFirstItem.Text = "Move first";
   // 
   // bindingNavigatorMovePreviousItem
   // 
   this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
   this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
   this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
   this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
   this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
   this.bindingNavigatorMovePreviousItem.Text = "Move previous";
   // 
   // bindingNavigatorSeparator
   // 
   this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
   this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
   // 
   // bindingNavigatorPositionItem
   // 
   this.bindingNavigatorPositionItem.AccessibleName = "Position";
   this.bindingNavigatorPositionItem.AutoSize = false;
   this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
   this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
   this.bindingNavigatorPositionItem.Text = "0";
   this.bindingNavigatorPositionItem.ToolTipText = "Current position";
   // 
   // bindingNavigatorSeparator1
   // 
   this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
   this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
   // 
   // bindingNavigatorMoveNextItem
   // 
   this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
   this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
   this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
   this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
   this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
   this.bindingNavigatorMoveNextItem.Text = "Move next";
   // 
   // bindingNavigatorMoveLastItem
   // 
   this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
   this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
   this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
   this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
   this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
   this.bindingNavigatorMoveLastItem.Text = "Move last";
   // 
   // bindingNavigatorSeparator2
   // 
   this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
   this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
   // 
   // flugBindingNavigatorSaveItem
   // 
   this.flugBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
   this.flugBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("flugBindingNavigatorSaveItem.Image")));
   this.flugBindingNavigatorSaveItem.Name = "flugBindingNavigatorSaveItem";
   this.flugBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
   this.flugBindingNavigatorSaveItem.Text = "Save Data";
   this.flugBindingNavigatorSaveItem.Click += new System.EventHandler(this.flugBindingNavigatorSaveItem_Click);
   // 
   // C_Orte
   // 
   this.C_Orte.Name = "C_Orte";
   this.C_Orte.Size = new System.Drawing.Size(121, 25);
   this.C_Orte.TextUpdate += new System.EventHandler(this.C_Orte_TextUpdate);
   this.C_Orte.Click += new System.EventHandler(this.C_Orte_Click);
   // 
   // flugDataGridView
   // 
   this.flugDataGridView.AutoGenerateColumns = false;
   this.flugDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
   this.flugDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10,
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12,
            this.dataGridViewTextBoxColumn13,
            this.dataGridViewTextBoxColumn14,
            this.dataGridViewTextBoxColumn15,
            this.dataGridViewTextBoxColumn16,
            this.dataGridViewTextBoxColumn17,
            this.dataGridViewImageColumn1,
            this.dataGridViewTextBoxColumn18});
   this.flugDataGridView.DataSource = this.flugBindingSource;
   this.flugDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
   this.flugDataGridView.Location = new System.Drawing.Point(0, 25);
   this.flugDataGridView.Name = "flugDataGridView";
   this.flugDataGridView.Size = new System.Drawing.Size(1003, 631);
   this.flugDataGridView.TabIndex = 1;
   // 
   // dataGridViewTextBoxColumn1
   // 
   this.dataGridViewTextBoxColumn1.DataPropertyName = "FlightNo";
   this.dataGridViewTextBoxColumn1.HeaderText = "FlightNo";
   this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
   // 
   // dataGridViewTextBoxColumn3
   // 
   this.dataGridViewTextBoxColumn3.DataPropertyName = "Departure";
   this.dataGridViewTextBoxColumn3.HeaderText = "Departure";
   this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
   // 
   // dataGridViewTextBoxColumn4
   // 
   this.dataGridViewTextBoxColumn4.DataPropertyName = "Destination";
   this.dataGridViewTextBoxColumn4.HeaderText = "Destination";
   this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
   // 
   // dataGridViewTextBoxColumn5
   // 
   this.dataGridViewTextBoxColumn5.DataPropertyName = "Date";
   this.dataGridViewTextBoxColumn5.HeaderText = "Date";
   this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
   // 
   // dataGridViewTextBoxColumn6
   // 
   this.dataGridViewTextBoxColumn6.DataPropertyName = "NonSmokingFlight";
   this.dataGridViewTextBoxColumn6.HeaderText = "NonSmokingFlight";
   this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
   // 
   // dataGridViewTextBoxColumn7
   // 
   this.dataGridViewTextBoxColumn7.DataPropertyName = "Seats";
   this.dataGridViewTextBoxColumn7.HeaderText = "Seats";
   this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
   // 
   // dataGridViewTextBoxColumn8
   // 
   this.dataGridViewTextBoxColumn8.DataPropertyName = "FreeSeats";
   this.dataGridViewTextBoxColumn8.HeaderText = "FreeSeats";
   this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
   // 
   // dataGridViewTextBoxColumn9
   // 
   this.dataGridViewTextBoxColumn9.DataPropertyName = "Price";
   this.dataGridViewTextBoxColumn9.HeaderText = "Price";
   this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
   // 
   // dataGridViewTextBoxColumn10
   // 
   this.dataGridViewTextBoxColumn10.DataPropertyName = "Memo";
   this.dataGridViewTextBoxColumn10.HeaderText = "Memo";
   this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
   // 
   // dataGridViewTextBoxColumn11
   // 
   this.dataGridViewTextBoxColumn11.DataPropertyName = "BookingSet";
   this.dataGridViewTextBoxColumn11.HeaderText = "BookingSet";
   this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
   // 
   // dataGridViewTextBoxColumn12
   // 
   this.dataGridViewTextBoxColumn12.DataPropertyName = "Pilot";
   this.dataGridViewTextBoxColumn12.HeaderText = "Pilot";
   this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
   // 
   // dataGridViewTextBoxColumn13
   // 
   this.dataGridViewTextBoxColumn13.DataPropertyName = "Copilot";
   this.dataGridViewTextBoxColumn13.HeaderText = "Copilot";
   this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
   // 
   // dataGridViewTextBoxColumn14
   // 
   this.dataGridViewTextBoxColumn14.DataPropertyName = "PilotId";
   this.dataGridViewTextBoxColumn14.HeaderText = "PilotId";
   this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
   // 
   // dataGridViewTextBoxColumn15
   // 
   this.dataGridViewTextBoxColumn15.DataPropertyName = "CopilotId";
   this.dataGridViewTextBoxColumn15.HeaderText = "CopilotId";
   this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
   // 
   // dataGridViewTextBoxColumn16
   // 
   this.dataGridViewTextBoxColumn16.DataPropertyName = "AircraftType";
   this.dataGridViewTextBoxColumn16.HeaderText = "AircraftType";
   this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
   // 
   // dataGridViewTextBoxColumn17
   // 
   this.dataGridViewTextBoxColumn17.DataPropertyName = "AircraftTypeID";
   this.dataGridViewTextBoxColumn17.HeaderText = "AircraftTypeID";
   this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
   // 
   // dataGridViewImageColumn1
   // 
   this.dataGridViewImageColumn1.DataPropertyName = "Timestamp";
   this.dataGridViewImageColumn1.HeaderText = "Timestamp";
   this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
   // 
   // dataGridViewTextBoxColumn18
   // 
   this.dataGridViewTextBoxColumn18.DataPropertyName = "Strikebound";
   this.dataGridViewTextBoxColumn18.HeaderText = "Strikebound";
   this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
   this.dataGridViewTextBoxColumn18.ReadOnly = true;
   // 
   // FlugPassagierMasterDetail
   // 
   this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
   this.ClientSize = new System.Drawing.Size(1003, 656);
   this.Controls.Add(this.flugDataGridView);
   this.Controls.Add(this.flugBindingNavigator);
   this.Name = "FlugPassagierMasterDetail";
   this.Text = "World Wide Wings - Flugdatenverwaltung (Windows Forms)";
   this.Load += new System.EventHandler(this.FlugPassagierMasterDetail_Load);
   ((System.ComponentModel.ISupportInitialize)(this.flugBindingSource)).EndInit();
   ((System.ComponentModel.ISupportInitialize)(this.flugBindingNavigator)).EndInit();
   this.flugBindingNavigator.ResumeLayout(false);
   this.flugBindingNavigator.PerformLayout();
   ((System.ComponentModel.ISupportInitialize)(this.flugDataGridView)).EndInit();
   this.ResumeLayout(false);
   this.PerformLayout();

  }

  #endregion
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
  private System.Windows.Forms.BindingSource flugBindingSource;
  private System.Windows.Forms.BindingNavigator flugBindingNavigator;
  private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
  private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
  private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
  private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
  private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
  private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
  private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
  private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
  private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
  private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
  private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
  private System.Windows.Forms.ToolStripButton flugBindingNavigatorSaveItem;
  private System.Windows.Forms.DataGridView flugDataGridView;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
  private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
  private System.Windows.Forms.ToolStripComboBox C_Orte;
 }
}