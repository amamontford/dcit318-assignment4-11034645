Write-Host "Medical Appointment Booking System - Build and Run Script" -ForegroundColor Green
Write-Host "========================================================" -ForegroundColor Green
Write-Host ""

Write-Host "Building the application..." -ForegroundColor Yellow
dotnet build MedicalAppointmentSystem/MedicalAppointmentSystem.csproj

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "Build successful! Running the application..." -ForegroundColor Green
    Write-Host ""
    dotnet run --project MedicalAppointmentSystem/MedicalAppointmentSystem.csproj
} else {
    Write-Host ""
    Write-Host "Build failed! Please check the error messages above." -ForegroundColor Red
    Read-Host "Press Enter to continue"
}
