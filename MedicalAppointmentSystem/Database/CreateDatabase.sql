-- Medical Appointment Booking System Database
-- Create the MedicalDB database

USE master;
GO

-- Drop database if it exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'MedicalDB')
BEGIN
    ALTER DATABASE MedicalDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE MedicalDB;
END
GO

-- Create the database
CREATE DATABASE MedicalDB;
GO

USE MedicalDB;
GO

-- Create Doctors table
CREATE TABLE Doctors (
    DoctorID INT IDENTITY(1,1) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Specialty VARCHAR(100) NOT NULL,
    Availability BIT DEFAULT 1
);

-- Create Patients table
CREATE TABLE Patients (
    PatientID INT IDENTITY(1,1) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL
);

-- Create Appointments table
CREATE TABLE Appointments (
    AppointmentID INT IDENTITY(1,1) PRIMARY KEY,
    DoctorID INT NOT NULL,
    PatientID INT NOT NULL,
    AppointmentDate DATETIME NOT NULL,
    Notes VARCHAR(500),
    FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID),
    FOREIGN KEY (PatientID) REFERENCES Patients(PatientID)
);

-- Insert sample data into Doctors table
INSERT INTO Doctors (FullName, Specialty, Availability) VALUES
('Dr. Ewurabena Johnson', 'Cardiology', 1),
('Dr. Jayden Osafo', 'Orthopedics', 1),
('Dr. Ama Montford', 'Pediatrics', 1),
('Dr. Kwabena Osei', 'Neurology', 1),
('Dr. Jeremy Doku', 'Dermatology', 1),
('Dr. Banyan Otchere', 'Internal Medicine', 1),
('Dr. Joycelyn Anning', 'Gynecology', 1),
('Dr. Gideon Asare', 'Psychiatry', 1);

-- Insert sample data into Patients table
INSERT INTO Patients (FullName, Email) VALUES
('Nhyira Yawson', 'nhyira.yawson@email.com'),
('Benedict Wiafe', 'benedict.wiafe@email.com'),
('Belly Fischer', 'belly.fischer@email.com'),
('Robert Taylor', 'robert.taylor@email.com'),
('Eunice Ayisi', 'eunice.ayisi@email.com'),
('Grace Obeng Mensah', 'grace.obengmensah@email.com'),
('Timothy Ninson', 'timothy.ninson@email.com'),
('Janice Agyakwa', 'janice.agyakwa@gmail.com'),
('Mikaela Jessie', 'mikaela.jessie@gmail.com');

-- Insert sample appointments
INSERT INTO Appointments (DoctorID, PatientID, AppointmentDate, Notes) VALUES
(1, 1, '2024-01-15 10:00:00', 'Regular checkup'),
(2, 2, '2024-01-16 14:30:00', 'Knee pain consultation'),
(3, 3, '2024-01-17 09:00:00', 'Child vaccination'),
(4, 4, '2024-01-18 11:00:00', 'Headache evaluation'),
(5, 5, '2024-01-19 15:00:00', 'Skin rash examination');

GO

-- Create indexes for better performance
CREATE INDEX IX_Appointments_DoctorID ON Appointments(DoctorID);
CREATE INDEX IX_Appointments_PatientID ON Appointments(PatientID);
CREATE INDEX IX_Appointments_Date ON Appointments(AppointmentDate);

GO

PRINT 'MedicalDB database created successfully with sample data!';
