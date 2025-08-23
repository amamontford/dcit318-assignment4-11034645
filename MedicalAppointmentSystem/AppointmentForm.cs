using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace MedicalAppointmentSystem
{
    public partial class AppointmentForm : Form
    {
        private ComboBox cmbDoctors;
        private ComboBox cmbPatients;
        private DateTimePicker dtpAppointmentDate;
        private TextBox txtNotes;
        private Button btnBook;
        private Button btnClear;
        private Button btnClose;

        public AppointmentForm()
        {
            InitializeComponent();
            LoadDoctors();
            LoadPatients();
        }

        private void InitializeComponent()
        {
            this.Text = "Book New Appointment";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Create title label
            Label titleLabel = new Label
            {
                Text = "Book New Appointment",
                Font = new Font("Arial", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(400, 30),
                Location = new Point(50, 20)
            };

            // Create labels
            Label lblDoctor = new Label
            {
                Text = "Select Doctor:",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Size = new Size(120, 20),
                Location = new Point(50, 70)
            };

            Label lblPatient = new Label
            {
                Text = "Select Patient:",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Size = new Size(120, 20),
                Location = new Point(50, 120)
            };

            Label lblDate = new Label
            {
                Text = "Appointment Date:",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Size = new Size(120, 20),
                Location = new Point(50, 170)
            };

            Label lblNotes = new Label
            {
                Text = "Notes:",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Size = new Size(120, 20),
                Location = new Point(50, 220)
            };

            // Create controls
            cmbDoctors = new ComboBox
            {
                Location = new Point(180, 70),
                Size = new Size(250, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbPatients = new ComboBox
            {
                Location = new Point(180, 120),
                Size = new Size(250, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            dtpAppointmentDate = new DateTimePicker
            {
                Location = new Point(180, 170),
                Size = new Size(250, 25),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "MM/dd/yyyy hh:mm tt",
                ShowUpDown = true
            };

            txtNotes = new TextBox
            {
                Location = new Point(180, 220),
                Size = new Size(250, 60),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            // Create buttons
            btnBook = new Button
            {
                Text = "Book Appointment",
                Size = new Size(120, 35),
                Location = new Point(50, 320),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnBook.Click += BtnBook_Click;

            btnClear = new Button
            {
                Text = "Clear Form",
                Size = new Size(120, 35),
                Location = new Point(190, 320),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnClear.Click += BtnClear_Click;

            btnClose = new Button
            {
                Text = "Close",
                Size = new Size(120, 35),
                Location = new Point(330, 320),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnClose.Click += BtnClose_Click;

            // Add controls to form
            this.Controls.AddRange(new Control[]
            {
                titleLabel,
                lblDoctor,
                lblPatient,
                lblDate,
                lblNotes,
                cmbDoctors,
                cmbPatients,
                dtpAppointmentDate,
                txtNotes,
                btnBook,
                btnClear,
                btnClose
            });
        }

        private void LoadDoctors()
        {
            try
            {
                string query = @"
                    SELECT DoctorID, FullName + ' - ' + Specialty AS DoctorInfo
                    FROM Doctors 
                    WHERE Availability = 1
                    ORDER BY FullName";

                DataTable doctorsTable = DatabaseHelper.ExecuteQuery(query);
                cmbDoctors.DataSource = doctorsTable;
                cmbDoctors.DisplayMember = "DoctorInfo";
                cmbDoctors.ValueMember = "DoctorID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading doctors: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPatients()
        {
            try
            {
                string query = @"
                    SELECT PatientID, FullName + ' (' + Email + ')' AS PatientInfo
                    FROM Patients 
                    ORDER BY FullName";

                DataTable patientsTable = DatabaseHelper.ExecuteQuery(query);
                cmbPatients.DataSource = patientsTable;
                cmbPatients.DisplayMember = "PatientInfo";
                cmbPatients.ValueMember = "PatientID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading patients: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBook_Click(object? sender, EventArgs e)
        {
            if (ValidateForm())
            {
                BookAppointment();
            }
        }

        private bool ValidateForm()
        {
            if (cmbDoctors.SelectedValue == null)
            {
                MessageBox.Show("Please select a doctor.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbPatients.SelectedValue == null)
            {
                MessageBox.Show("Please select a patient.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dtpAppointmentDate.Value <= DateTime.Now)
            {
                MessageBox.Show("Please select a future date and time for the appointment.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void BookAppointment()
        {
            try
            {
                // Check if the doctor is available at the selected time
                if (!IsDoctorAvailable())
                {
                    MessageBox.Show("The selected doctor is not available at the chosen time. Please select a different time or doctor.", "Booking Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string query = @"
                    INSERT INTO Appointments (DoctorID, PatientID, AppointmentDate, Notes)
                    VALUES (@DoctorID, @PatientID, @AppointmentDate, @Notes)";

                SqlParameter[] parameters = {
                    new SqlParameter("@DoctorID", cmbDoctors.SelectedValue),
                    new SqlParameter("@PatientID", cmbPatients.SelectedValue),
                    new SqlParameter("@AppointmentDate", dtpAppointmentDate.Value),
                    new SqlParameter("@Notes", txtNotes.Text.Trim())
                };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Appointment booked successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Failed to book appointment. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error booking appointment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsDoctorAvailable()
        {
            try
            {
                string query = @"
                    SELECT COUNT(*) 
                    FROM Appointments 
                    WHERE DoctorID = @DoctorID 
                    AND AppointmentDate = @AppointmentDate";

                SqlParameter[] parameters = {
                    new SqlParameter("@DoctorID", cmbDoctors.SelectedValue),
                    new SqlParameter("@AppointmentDate", dtpAppointmentDate.Value)
                };

                object result = DatabaseHelper.ExecuteScalar(query, parameters);
                return result != null && Convert.ToInt32(result) == 0;
            }
            catch
            {
                return false;
            }
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            cmbDoctors.SelectedIndex = -1;
            cmbPatients.SelectedIndex = -1;
            dtpAppointmentDate.Value = DateTime.Now.AddDays(1);
            txtNotes.Clear();
        }

        private void BtnClose_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}
