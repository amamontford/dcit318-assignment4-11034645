using System.Windows.Forms;

namespace PharmacyInventorySystem.UI
{
	partial class MainForm
	{
		private System.ComponentModel.IContainer components = null;

		private TextBox txtName;
		private TextBox txtCategory;
		private TextBox txtPrice;
		private TextBox txtQuantity;
		private TextBox txtSaleQuantity;
		private TextBox txtSearch;
		private Button btnAddMedicine;
		private Button btnSearch;
		private Button btnUpdateStock;
		private Button btnRecordSale;
		private Button btnViewAll;
		private Button btnViewSales;
		private DataGridView dgvMedicines;
		private Label lblName;
		private Label lblCategory;
		private Label lblPrice;
		private Label lblQuantity;
		private Label lblSaleQuantity;
		private Label lblSearch;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			this.txtName = new TextBox();
			this.txtCategory = new TextBox();
			this.txtPrice = new TextBox();
			this.txtQuantity = new TextBox();
			this.txtSaleQuantity = new TextBox();
			this.txtSearch = new TextBox();
			this.btnAddMedicine = new Button();
			this.btnSearch = new Button();
			this.btnUpdateStock = new Button();
			this.btnRecordSale = new Button();
			this.btnViewAll = new Button();
			this.btnViewSales = new Button();
			this.dgvMedicines = new DataGridView();
			this.lblName = new Label();
			this.lblCategory = new Label();
			this.lblPrice = new Label();
			this.lblQuantity = new Label();
			this.lblSaleQuantity = new Label();
			this.lblSearch = new Label();
			((System.ComponentModel.ISupportInitialize)(this.dgvMedicines)).BeginInit();
			this.SuspendLayout();
			// 
			// Labels
			// 
			this.lblName.AutoSize = true;
			this.lblName.Text = "Name:";
			this.lblName.Left = 20;
			this.lblName.Top = 23;

			this.lblCategory.AutoSize = true;
			this.lblCategory.Text = "Category:";
			this.lblCategory.Left = 20;
			this.lblCategory.Top = 63;

			this.lblPrice.AutoSize = true;
			this.lblPrice.Text = "Price:";
			this.lblPrice.Left = 20;
			this.lblPrice.Top = 103;

			this.lblQuantity.AutoSize = true;
			this.lblQuantity.Text = "Quantity:";
			this.lblQuantity.Left = 20;
			this.lblQuantity.Top = 143;

			this.lblSaleQuantity.AutoSize = true;
			this.lblSaleQuantity.Text = "Sale Qty:";
			this.lblSaleQuantity.Left = 20;
			this.lblSaleQuantity.Top = 183;

			this.lblSearch.AutoSize = true;
			this.lblSearch.Text = "Search:";
			this.lblSearch.Left = 20;
			this.lblSearch.Top = 223;

			// 
			// TextBoxes
			// 
			this.txtName.Left = 120;
			this.txtName.Top = 20;
			this.txtName.Width = 220;
			this.txtName.Height = 23;

			this.txtCategory.Left = 120;
			this.txtCategory.Top = 60;
			this.txtCategory.Width = 220;
			this.txtCategory.Height = 23;

			this.txtPrice.Left = 120;
			this.txtPrice.Top = 100;
			this.txtPrice.Width = 220;
			this.txtPrice.Height = 23;

			this.txtQuantity.Left = 120;
			this.txtQuantity.Top = 140;
			this.txtQuantity.Width = 220;
			this.txtQuantity.Height = 23;

			this.txtSaleQuantity.Left = 120;
			this.txtSaleQuantity.Top = 180;
			this.txtSaleQuantity.Width = 220;
			this.txtSaleQuantity.Height = 23;

			this.txtSearch.Left = 120;
			this.txtSearch.Top = 220;
			this.txtSearch.Width = 220;
			this.txtSearch.Height = 23;

			// 
			// Buttons
			// 
			this.btnAddMedicine.Text = "Add Medicine";
			this.btnAddMedicine.Left = 360;
			this.btnAddMedicine.Top = 16;
			this.btnAddMedicine.Width = 120;
			this.btnAddMedicine.Height = 30;

			this.btnUpdateStock.Text = "Update Medicine";
			this.btnUpdateStock.Left = 360;
			this.btnUpdateStock.Top = 56;
			this.btnUpdateStock.Width = 120;
			this.btnUpdateStock.Height = 30;

			this.btnRecordSale.Text = "Record Sale";
			this.btnRecordSale.Left = 360;
			this.btnRecordSale.Top = 96;
			this.btnRecordSale.Width = 120;
			this.btnRecordSale.Height = 30;

			this.btnViewAll.Text = "View All";
			this.btnViewAll.Left = 360;
			this.btnViewAll.Top = 136;
			this.btnViewAll.Width = 120;
			this.btnViewAll.Height = 30;

			this.btnSearch.Text = "Search";
			this.btnSearch.Left = 360;
			this.btnSearch.Top = 216;
			this.btnSearch.Width = 120;
			this.btnSearch.Height = 30;

			this.btnViewSales.Text = "View Sales";
			this.btnViewSales.Left = 360;
			this.btnViewSales.Top = 176;
			this.btnViewSales.Width = 120;
			this.btnViewSales.Height = 30;

			// 
			// DataGridView
			// 
			this.dgvMedicines.Left = 20;
			this.dgvMedicines.Top = 260;
			this.dgvMedicines.Width = 840;
			this.dgvMedicines.Height = 320;
			this.dgvMedicines.ReadOnly = true;
			this.dgvMedicines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvMedicines.MultiSelect = false;
			this.dgvMedicines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

			// 
			// MainForm
			// 
			this.Text = "Pharmacy Inventory System";
			this.StartPosition = FormStartPosition.CenterScreen;
			this.ClientSize = new System.Drawing.Size(884, 601);
			this.Controls.Add(this.lblName);
			this.Controls.Add(this.lblCategory);
			this.Controls.Add(this.lblPrice);
			this.Controls.Add(this.lblQuantity);
			this.Controls.Add(this.lblSaleQuantity);
			this.Controls.Add(this.lblSearch);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.txtCategory);
			this.Controls.Add(this.txtPrice);
			this.Controls.Add(this.txtQuantity);
			this.Controls.Add(this.txtSaleQuantity);
			this.Controls.Add(this.txtSearch);
			this.Controls.Add(this.btnAddMedicine);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.btnUpdateStock);
			this.Controls.Add(this.btnRecordSale);
			this.Controls.Add(this.btnViewAll);
			this.Controls.Add(this.btnViewSales);
			this.Controls.Add(this.dgvMedicines);
			((System.ComponentModel.ISupportInitialize)(this.dgvMedicines)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
	}
}


