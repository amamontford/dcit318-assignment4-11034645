using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic; // Added for List<SqlParameter>

namespace MedicalAppointmentSystem
{
    public partial class ManageAppointmentsForm : Form
    {
        private DataGridView dgvAppointments;
        private ComboBox cmbFilterPatient;
        private ComboBox cmbFilterDoctor;
        private DateTimePicker dtpFilterDate;
        private Button btnSearch;
        private Button btnRefresh;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClose;
        private DataSet appointmentsDataSet;

        public ManageAppointmentsForm()
        {
            InitializeComponent();
            LoadFilterData();
            LoadAppointments();
        }

        private void InitializeComponent()
        {
            this.Text = "Manage Appointments";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Create title label
            Label titleLabel = new Label
            {
                Text = "Manage Appointments",
                Font = new Font("Arial", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(800, 30),
                Location = new Point(50, 20)
            };

            // Create filter controls
            Label lblFilterPatient = new Label
            {
                Text = "Filter by Patient:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Size = new Size(100, 20),
                Location = new Point(50, 70)
            };

            cmbFilterPatient = new ComboBox
            {
                Location = new Point(160, 70),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Label lblFilterDoctor = new Label
            {
                Text = "Filter by Doctor:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Size = new Size(100, 20),
                Location = new Point(330, 70)
            };

            cmbFilterDoctor = new ComboBox
            {
                Location = new Point(440, 70),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Label lblFilterDate = new Label
            {
                Text = "Filter by Date:",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Size = new Size(100, 20),
                Location = new Point(610, 70)
            };

            dtpFilterDate = new DateTimePicker
            {
                Location = new Point(720, 70),
                Size = new Size(120, 25),
                Format = DateTimePickerFormat.Short
            };

            btnSearch = new Button
            {
                Text = "Search",
                Size = new Size(80, 30),
                Location = new Point(50, 110),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            btnSearch.Click += BtnSearch_Click;

            btnRefresh = new Button
            {
                Text = "Refresh",
                Size = new Size(80, 30),
                Location = new Point(140, 110),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            btnRefresh.Click += BtnRefresh_Click;

            // Create DataGridView
            dgvAppointments = new DataGridView
            {
                Location = new Point(50, 160),
                Size = new Size(800, 300),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };
            dgvAppointments.DataBindingComplete += DgvAppointments_DataBindingComplete;
            dgvAppointments.SelectionChanged += DgvAppointments_SelectionChanged;

            // Create action buttons
            btnUpdate = new Button
            {
                Text = "Update Appointment",
                Size = new Size(140, 35),
                Location = new Point(50, 480),
                Font = new Font("Arial", 10, FontStyle.Bold),
                Enabled = false
            };
            btnUpdate.Click += BtnUpdate_Click;

            btnDelete = new Button
            {
                Text = "Delete Appointment",
                Size = new Size(140, 35),
                Location = new Point(200, 480),
                Font = new Font("Arial", 10, FontStyle.Bold),
                Enabled = false
            };
            btnDelete.Click += BtnDelete_Click;

            btnClose = new Button
            {
                Text = "Close",
                Size = new Size(100, 35),
                Location = new Point(750, 480),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnClose.Click += BtnClose_Click;

            // Add controls to form
            this.Controls.AddRange(new Control[]
            {
                titleLabel,
                lblFilterPatient,
                lblFilterDoctor,
                lblFilterDate,
                cmbFilterPatient,
                cmbFilterDoctor,
                dtpFilterDate,
                btnSearch,
                btnRefresh,
                dgvAppointments,
                btnUpdate,
                btnDelete,
                btnClose
            });
        }

        private void LoadFilterData()
        {
            try
            {
                // Load patients for filter
                string patientQuery = @"
                    SELECT PatientID, FullName 
                    FROM Patients 
                    ORDER BY FullName";

                DataTable patientsTable = DatabaseHelper.ExecuteQuery(patientQuery);
                cmbFilterPatient.DataSource = patientsTable;
                cmbFilterPatient.DisplayMember = "FullName";
                cmbFilterPatient.ValueMember = "PatientID";

                // Load doctors for filter
                string doctorQuery = @"
                    SELECT DoctorID, FullName 
                    FROM Doctors 
                    ORDER BY FullName";

                DataTable doctorsTable = DatabaseHelper.ExecuteQuery(doctorQuery);
                cmbFilterDoctor.DataSource = doctorsTable;
                cmbFilterDoctor.DisplayMember = "FullName";
                cmbFilterDoctor.ValueMember = "DoctorID";

                // Set default date to today
                dtpFilterDate.Value = DateTime.Today;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading filter data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAppointments()
        {
            try
            {
                string query = @"
                    SELECT 
                        a.AppointmentID,
                        d.FullName AS DoctorName,
                        p.FullName AS PatientName,
                        a.AppointmentDate,
                        a.Notes
                    FROM Appointments a
                    INNER JOIN Doctors d ON a.DoctorID = d.DoctorID
                    INNER JOIN Patients p ON a.PatientID = p.PatientID
                    ORDER BY a.AppointmentDate DESC";

                appointmentsDataSet = new DataSet();
                DataTable appointmentsTable = DatabaseHelper.ExecuteQuery(query) ?? new DataTable();
                appointmentsDataSet.Tables.Add(appointmentsTable);
                dgvAppointments.DataSource = appointmentsDataSet.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading appointments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvAppointments_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                if (dgvAppointments?.Columns == null) return;
                if (dgvAppointments.Columns.Contains("AppointmentID")) dgvAppointments.Columns["AppointmentID"].HeaderText = "ID";
                if (dgvAppointments.Columns.Contains("DoctorName")) dgvAppointments.Columns["DoctorName"].HeaderText = "Doctor";
                if (dgvAppointments.Columns.Contains("PatientName")) dgvAppointments.Columns["PatientName"].HeaderText = "Patient";
                if (dgvAppointments.Columns.Contains("AppointmentDate")) dgvAppointments.Columns["AppointmentDate"].HeaderText = "Date & Time";
                if (dgvAppointments.Columns.Contains("Notes")) dgvAppointments.Columns["Notes"].HeaderText = "Notes";
                if (dgvAppointments.Columns.Contains("AppointmentID")) dgvAppointments.Columns["AppointmentID"].Width = 50;

                dgvAppointments.EnableHeadersVisualStyles = false;
                dgvAppointments.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                dgvAppointments.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                dgvAppointments.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            }
            catch { /* swallow styling errors */ }
        }

        private void BtnSearch_Click(object? sender, EventArgs e)
        {
            try
            {
                string query = @"
                    SELECT 
                        a.AppointmentID,
                        d.FullName AS DoctorName,
                        p.FullName AS PatientName,
                        a.AppointmentDate,
                        a.Notes
                    FROM Appointments a
                    INNER JOIN Doctors d ON a.DoctorID = d.DoctorID
                    INNER JOIN Patients p ON a.PatientID = p.PatientID
                    WHERE 1=1";

                var parameters = new List<SqlParameter>();

                if (cmbFilterPatient.SelectedValue != null)
                {
                    query += " AND a.PatientID = @PatientID";
                    parameters.Add(new SqlParameter("@PatientID", cmbFilterPatient.SelectedValue));
                }

                if (cmbFilterDoctor.SelectedValue != null)
                {
                    query += " AND a.DoctorID = @DoctorID";
                    parameters.Add(new SqlParameter("@DoctorID", cmbFilterDoctor.SelectedValue));
                }

                query += " AND CAST(a.AppointmentDate AS DATE) = @AppointmentDate";
                parameters.Add(new SqlParameter("@AppointmentDate", dtpFilterDate.Value.Date));

                query += " ORDER BY a.AppointmentDate DESC";

                DataTable filteredTable = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
                dgvAppointments.DataSource = filteredTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching appointments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadAppointments();
        }

        private void DgvAppointments_SelectionChanged(object? sender, EventArgs e)
        {
            bool hasSelection = dgvAppointments.SelectedRows.Count > 0;
            btnUpdate.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
        }

        private void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (dgvAppointments.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvAppointments.SelectedRows[0];
                int appointmentId = Convert.ToInt32(selectedRow.Cells["AppointmentID"].Value);
                string currentNotes = selectedRow.Cells["Notes"].Value?.ToString() ?? "";

                UpdateAppointmentForm updateForm = new UpdateAppointmentForm(appointmentId, currentNotes);
                if (updateForm.ShowDialog() == DialogResult.OK)
                {
                    LoadAppointments(); // Refresh the grid
                }
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvAppointments.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvAppointments.SelectedRows[0];
                int appointmentId = Convert.ToInt32(selectedRow.Cells["AppointmentID"].Value);
                string patientName = selectedRow.Cells["PatientName"].Value.ToString();
                string doctorName = selectedRow.Cells["DoctorName"].Value.ToString();
                DateTime appointmentDate = Convert.ToDateTime(selectedRow.Cells["AppointmentDate"].Value);

                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete the appointment for {patientName} with {doctorName} on {appointmentDate:MM/dd/yyyy hh:mm tt}?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DeleteAppointment(appointmentId);
                }
            }
        }

        private void DeleteAppointment(int appointmentId)
        {
            try
            {
                string query = "DELETE FROM Appointments WHERE AppointmentID = @AppointmentID";
                SqlParameter[] parameters = { new SqlParameter("@AppointmentID", appointmentId) };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Appointment deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAppointments(); // Refresh the grid
                }
                else
                {
                    MessageBox.Show("Failed to delete appointment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting appointment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClose_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}
