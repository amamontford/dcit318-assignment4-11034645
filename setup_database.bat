@echo off
echo Medical Appointment System - Database Setup
echo ==========================================
echo.

echo Checking if LocalDB is available...
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "SELECT @@VERSION" >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: LocalDB is not available or sqlcmd is not installed.
    echo Please install SQL Server LocalDB or SQL Server Express.
    echo.
    pause
    exit /b 1
)

echo LocalDB is available. Creating database...
echo.

echo Running CreateDatabase.sql script...
sqlcmd -S "(localdb)\MSSQLLocalDB" -i "MedicalAppointmentSystem\Database\CreateDatabase.sql"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Database created successfully!
    echo.
    echo Verifying database contents...
    echo.
    echo Doctors in database:
    sqlcmd -S "(localdb)\MSSQLLocalDB" -d "MedicalDB" -Q "SELECT FullName, Specialty FROM Doctors ORDER BY FullName"
    echo.
    echo Patients in database:
    sqlcmd -S "(localdb)\MSSQLLocalDB" -d "MedicalDB" -Q "SELECT FullName, Email FROM Patients ORDER BY FullName"
    echo.
    echo Database setup complete! You can now run your application.
) else (
    echo.
    echo ERROR: Failed to create database. Please check the error messages above.
)

echo.
pause
