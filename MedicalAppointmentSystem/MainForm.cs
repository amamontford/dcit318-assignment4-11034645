using System;
using System.Drawing;
using System.Windows.Forms;

namespace MedicalAppointmentSystem
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Medical Appointment Booking System";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Create title label
            Label titleLabel = new Label
            {
                Text = "Medical Appointment",
                Font = new Font("Arial", 18, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(500, 40),
                Location = new Point(50, 30)
            };

            // Create subtitle label
            Label subtitleLabel = new Label
            {
                Text = "Welcome to the Medical Appointment",
                Font = new Font("Arial", 12),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(500, 30),
                Location = new Point(50, 80)
            };

            // Create navigation buttons
            Button btnViewDoctors = new Button
            {
                Text = "View Available Appointments",
                Size = new Size(200, 40),
                Location = new Point(50, 150),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnViewDoctors.Click += BtnViewDoctors_Click;

            Button btnBookAppointment = new Button
            {
                Text = "Book New Appointment",
                Size = new Size(200, 40),
                Location = new Point(300, 150),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnBookAppointment.Click += BtnBookAppointment_Click;

            Button btnManageAppointments = new Button
            {
                Text = "Manage Appointments",
                Size = new Size(200, 40),
                Location = new Point(50, 220),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnManageAppointments.Click += BtnManageAppointments_Click;

            Button btnExit = new Button
            {
                Text = "Exit Application",
                Size = new Size(200, 40),
                Location = new Point(300, 220),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnExit.Click += BtnExit_Click;

            // Add controls to form
            this.Controls.AddRange(new Control[] 
            {
                titleLabel,
                subtitleLabel,
                btnViewDoctors,
                btnBookAppointment,
                btnManageAppointments,
                btnExit
            });
        }

        private void BtnViewDoctors_Click(object? sender, EventArgs e)
        {
            DoctorListForm doctorForm = new DoctorListForm();
            doctorForm.Show();
        }

        private void BtnBookAppointment_Click(object? sender, EventArgs e)
        {
            AppointmentForm appointmentForm = new AppointmentForm();
            appointmentForm.Show();
        }

        private void BtnManageAppointments_Click(object? sender, EventArgs e)
        {
            ManageAppointmentsForm manageForm = new ManageAppointmentsForm();
            manageForm.Show();
        }

        private void BtnExit_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
