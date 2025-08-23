Write-Host "Medical Appointment System - Database Setup" -ForegroundColor Green
Write-Host "=========================================" -ForegroundColor Green
Write-Host ""

Write-Host "Checking if LocalDB is available..." -ForegroundColor Yellow
try {
    $result = sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "SELECT @@VERSION" 2>$null
    if ($LASTEXITCODE -ne 0) {
        throw "LocalDB connection failed"
    }
} catch {
    Write-Host "ERROR: LocalDB is not available or sqlcmd is not installed." -ForegroundColor Red
    Write-Host "Please install SQL Server LocalDB or SQL Server Express." -ForegroundColor Red
    Write-Host ""
    Read-Host "Press Enter to continue"
    exit 1
}

Write-Host "LocalDB is available. Creating database..." -ForegroundColor Green
Write-Host ""

Write-Host "Running CreateDatabase.sql script..." -ForegroundColor Yellow
$scriptPath = "MedicalAppointmentSystem\Database\CreateDatabase.sql"
$result = sqlcmd -S "(localdb)\MSSQLLocalDB" -i $scriptPath

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "Database created successfully!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Verifying database contents..." -ForegroundColor Yellow
    Write-Host ""
    
    Write-Host "Doctors in database:" -ForegroundColor Cyan
    sqlcmd -S "(localdb)\MSSQLLocalDB" -d "MedicalDB" -Q "SELECT FullName, Specialty FROM Doctors ORDER BY FullName"
    Write-Host ""
    
    Write-Host "Patients in database:" -ForegroundColor Cyan
    sqlcmd -S "(localdb)\MSSQLLocalDB" -d "MedicalDB" -Q "SELECT FullName, Email FROM Patients ORDER BY FullName"
    Write-Host ""
    
    Write-Host "Database setup complete! You can now run your application." -ForegroundColor Green
} else {
    Write-Host ""
    Write-Host "ERROR: Failed to create database. Please check the error messages above." -ForegroundColor Red
}

Write-Host ""
Read-Host "Press Enter to continue"
