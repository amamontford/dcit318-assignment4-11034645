using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using PharmacyInventorySystem.Data;

namespace PharmacyInventorySystem.UI
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			WireUpEvents();
		}

		private void WireUpEvents()
		{
			this.btnAddMedicine.Click += btnAddMedicine_Click;
			this.btnSearch.Click += btnSearch_Click;
			this.btnUpdateStock.Click += btnUpdateStock_Click;
			this.btnRecordSale.Click += btnRecordSale_Click;
			this.btnViewAll.Click += btnViewAll_Click;
			this.btnViewSales.Click += btnViewSales_Click;
			this.dgvMedicines.SelectionChanged += DgvMedicines_SelectionChanged;
			this.Load += MainForm_Load;
		}

		private void MainForm_Load(object? sender, EventArgs e)
		{
			RefreshGrid();
		}

		private void RefreshGrid()
		{
			try
			{
				using DatabaseHelper db = new DatabaseHelper();
				db.Open();
				DataTable medicines = db.GetAllMedicines();
				dgvMedicines.DataSource = medicines;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to load medicines. {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnAddMedicine_Click(object? sender, EventArgs e)
		{
			try
			{
				if (!decimal.TryParse(txtPrice.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price))
				{
					MessageBox.Show("Enter a valid price.");
					return;
				}
				if (!int.TryParse(txtQuantity.Text, out int quantity))
				{
					MessageBox.Show("Enter a valid quantity.");
					return;
				}
				using DatabaseHelper db = new DatabaseHelper();
				db.Open();
				int rows = db.AddMedicine(txtName.Text.Trim(), txtCategory.Text.Trim(), price, quantity);
				if (rows > 0)
				{
					MessageBox.Show("Medicine added successfully.");
					RefreshGrid();
				}
				else
				{
					MessageBox.Show("No rows affected.");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to add medicine. {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnSearch_Click(object? sender, EventArgs e)
		{
			try
			{
				using DatabaseHelper db = new DatabaseHelper();
				db.Open();
				DataTable results = db.SearchMedicine(txtSearch.Text.Trim());
				dgvMedicines.DataSource = results;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Search failed. {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void DgvMedicines_SelectionChanged(object? sender, EventArgs e)
		{
			if (dgvMedicines.CurrentRow != null)
			{
				// Check if we're viewing medicines or sales data
				if (dgvMedicines.DataSource is DataTable dt && dt.Columns.Contains("MedicineName"))
				{
					// We're viewing sales data, don't populate the text boxes
					return;
				}
				
				// We're viewing medicines data
				txtName.Text = dgvMedicines.CurrentRow.Cells["Name"].Value?.ToString() ?? "";
				txtCategory.Text = dgvMedicines.CurrentRow.Cells["Category"].Value?.ToString() ?? "";
				txtPrice.Text = dgvMedicines.CurrentRow.Cells["Price"].Value?.ToString() ?? "";
				txtQuantity.Text = dgvMedicines.CurrentRow.Cells["Quantity"].Value?.ToString() ?? "";
				txtSaleQuantity.Clear(); // Clear sale quantity when selecting a new medicine
			}
		}

		private void btnUpdateStock_Click(object? sender, EventArgs e)
		{
			try
			{
				// Check if we're viewing medicines data (not sales data)
				if (dgvMedicines.DataSource is DataTable dt && dt.Columns.Contains("SaleID"))
				{
					MessageBox.Show("Please switch to 'View All' to see medicines before updating.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				if (dgvMedicines.CurrentRow == null)
				{
					MessageBox.Show("Select a medicine first.");
					return;
				}
				if (!decimal.TryParse(txtPrice.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price))
				{
					MessageBox.Show("Enter a valid price.");
					return;
				}
				if (!int.TryParse(txtQuantity.Text, out int quantity))
				{
					MessageBox.Show("Enter a valid quantity.");
					return;
				}
				int medicineId = Convert.ToInt32(dgvMedicines.CurrentRow.Cells["MedicineID"].Value);
				using DatabaseHelper db = new DatabaseHelper();
				db.Open();
				int rows = db.UpdateMedicine(medicineId, txtName.Text.Trim(), txtCategory.Text.Trim(), price, quantity);
				MessageBox.Show(rows > 0 ? "Medicine updated successfully." : "No rows affected.");
				RefreshGrid();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to update medicine. {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnRecordSale_Click(object? sender, EventArgs e)
		{
			try
			{
				// Check if we're viewing medicines data (not sales data)
				if (dgvMedicines.DataSource is DataTable dt && dt.Columns.Contains("SaleID"))
				{
					MessageBox.Show("Please switch to 'View All' to see medicines before recording a sale.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				if (dgvMedicines.CurrentRow == null)
				{
					MessageBox.Show("Select a medicine first.");
					return;
				}
				if (!int.TryParse(txtSaleQuantity.Text, out int quantitySold))
				{
					MessageBox.Show("Enter a valid quantity to sell in the 'Sale Qty' field.");
					return;
				}
				if (quantitySold <= 0)
				{
					MessageBox.Show("Sale quantity must be greater than 0.");
					return;
				}
				int medicineId = Convert.ToInt32(dgvMedicines.CurrentRow.Cells["MedicineID"].Value);
				int currentStock = Convert.ToInt32(dgvMedicines.CurrentRow.Cells["Quantity"].Value);
				if (quantitySold > currentStock)
				{
					MessageBox.Show($"Cannot sell {quantitySold} units. Only {currentStock} units available in stock.");
					return;
				}
				using DatabaseHelper db = new DatabaseHelper();
				db.Open();
				int rows = db.RecordSale(medicineId, quantitySold);
				MessageBox.Show(rows > 0 ? $"Sale recorded successfully. {quantitySold} units sold." : "No rows affected.");
				txtSaleQuantity.Clear();
				RefreshGrid();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to record sale. {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnViewAll_Click(object? sender, EventArgs e)
		{
			RefreshGrid();
		}

		private void btnViewSales_Click(object? sender, EventArgs e)
		{
			try
			{
				using DatabaseHelper db = new DatabaseHelper();
				db.Open();
				DataTable sales = db.GetSales();
				dgvMedicines.DataSource = sales;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to load sales. {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}


