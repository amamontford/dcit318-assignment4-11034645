using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace MedicalAppointmentSystem
{
    public partial class DoctorListForm : Form
    {
        private DataGridView dgvDoctors;
        private Button btnRefresh;
        private Button btnClose;

        public DoctorListForm()
        {
            InitializeComponent();
            LoadDoctors();
        }

        private void InitializeComponent()
        {
            this.Text = "Available Doctors";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Create title label
            Label titleLabel = new Label
            {
                Text = "Available Doctors",
                Font = new Font("Arial", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(600, 30),
                Location = new Point(50, 20)
            };

            // Create DataGridView
            dgvDoctors = new DataGridView
            {
                Location = new Point(50, 70),
                Size = new Size(600, 300),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };
            dgvDoctors.DataBindingComplete += DgvDoctors_DataBindingComplete;

            // Create buttons
            btnRefresh = new Button
            {
                Text = "Refresh",
                Size = new Size(100, 35),
                Location = new Point(50, 390),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnRefresh.Click += BtnRefresh_Click;

            btnClose = new Button
            {
                Text = "Close",
                Size = new Size(100, 35),
                Location = new Point(550, 390),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnClose.Click += BtnClose_Click;

            // Add controls to form
            this.Controls.AddRange(new Control[]
            {
                titleLabel,
                dgvDoctors,
                btnRefresh,
                btnClose
            });
        }

        private void LoadDoctors()
        {
            try
            {
                string query = @"
                    SELECT 
                        DoctorID,
                        FullName,
                        Specialty,
                        CASE 
                            WHEN Availability = 1 THEN 'Available'
                            ELSE 'Not Available'
                        END AS Availability
                    FROM Doctors 
                    ORDER BY FullName";

                DataTable doctorsTable = DatabaseHelper.ExecuteQuery(query);
                dgvDoctors.DataSource = doctorsTable ?? new DataTable();
                // Styling and header setup moved to DataBindingComplete for reliability
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading doctors: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvDoctors_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                if (dgvDoctors?.Columns == null) return;

                if (dgvDoctors.Columns.Contains("DoctorID")) dgvDoctors.Columns["DoctorID"].HeaderText = "Doctor ID";
                if (dgvDoctors.Columns.Contains("FullName")) dgvDoctors.Columns["FullName"].HeaderText = "Doctor Name";
                if (dgvDoctors.Columns.Contains("Specialty")) dgvDoctors.Columns["Specialty"].HeaderText = "Specialty";
                if (dgvDoctors.Columns.Contains("Availability")) dgvDoctors.Columns["Availability"].HeaderText = "Status";
                if (dgvDoctors.Columns.Contains("DoctorID")) dgvDoctors.Columns["DoctorID"].Width = 80;

                dgvDoctors.EnableHeadersVisualStyles = false;
                dgvDoctors.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                dgvDoctors.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                dgvDoctors.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            }
            catch { /* swallow styling errors */ }
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadDoctors();
        }

        private void BtnClose_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}
