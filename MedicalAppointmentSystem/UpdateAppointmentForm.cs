using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace MedicalAppointmentSystem
{
    public partial class UpdateAppointmentForm : Form
    {
        private int appointmentId;
        private DateTimePicker dtpNewDate;
        private TextBox txtNewNotes;
        private Button btnUpdate;
        private Button btnCancel;

        public UpdateAppointmentForm(int appointmentId, string currentNotes)
        {
            this.appointmentId = appointmentId;
            InitializeComponent();
            txtNewNotes.Text = currentNotes;
        }

        private void InitializeComponent()
        {
            this.Text = "Update Appointment";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Create title label
            Label titleLabel = new Label
            {
                Text = "Update Appointment",
                Font = new Font("Arial", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(300, 25),
                Location = new Point(50, 20)
            };

            // Create labels
            Label lblNewDate = new Label
            {
                Text = "New Date & Time:",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Size = new Size(120, 20),
                Location = new Point(50, 70)
            };

            Label lblNewNotes = new Label
            {
                Text = "New Notes:",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Size = new Size(120, 20),
                Location = new Point(50, 120)
            };

            // Create controls
            dtpNewDate = new DateTimePicker
            {
                Location = new Point(180, 70),
                Size = new Size(150, 25),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "MM/dd/yyyy hh:mm tt",
                ShowUpDown = true,
                Value = DateTime.Now.AddDays(1)
            };

            txtNewNotes = new TextBox
            {
                Location = new Point(180, 120),
                Size = new Size(150, 60),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            // Create buttons
            btnUpdate = new Button
            {
                Text = "Update",
                Size = new Size(100, 30),
                Location = new Point(100, 200),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnUpdate.Click += BtnUpdate_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Size = new Size(100, 30),
                Location = new Point(220, 200),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnCancel.Click += BtnCancel_Click;

            // Add controls to form
            this.Controls.AddRange(new Control[]
            {
                titleLabel,
                lblNewDate,
                lblNewNotes,
                dtpNewDate,
                txtNewNotes,
                btnUpdate,
                btnCancel
            });
        }

        private void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (ValidateForm())
            {
                UpdateAppointment();
            }
        }

        private bool ValidateForm()
        {
            if (dtpNewDate.Value <= DateTime.Now)
            {
                MessageBox.Show("Please select a future date and time for the appointment.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void UpdateAppointment()
        {
            try
            {
                // Check if the new time conflicts with existing appointments
                if (!IsTimeSlotAvailable())
                {
                    MessageBox.Show("The selected time slot is not available. Please choose a different time.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string query = @"
                    UPDATE Appointments 
                    SET AppointmentDate = @AppointmentDate, Notes = @Notes
                    WHERE AppointmentID = @AppointmentID";

                SqlParameter[] parameters = {
                    new SqlParameter("@AppointmentDate", dtpNewDate.Value),
                    new SqlParameter("@Notes", txtNewNotes.Text.Trim()),
                    new SqlParameter("@AppointmentID", appointmentId)
                };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Appointment updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to update appointment. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating appointment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsTimeSlotAvailable()
        {
            try
            {
                string query = @"
                    SELECT COUNT(*) 
                    FROM Appointments 
                    WHERE AppointmentID != @AppointmentID 
                    AND AppointmentDate = @AppointmentDate";

                SqlParameter[] parameters = {
                    new SqlParameter("@AppointmentID", appointmentId),
                    new SqlParameter("@AppointmentDate", dtpNewDate.Value)
                };

                object result = DatabaseHelper.ExecuteScalar(query, parameters);
                return result != null && Convert.ToInt32(result) == 0;
            }
            catch
            {
                return false;
            }
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
