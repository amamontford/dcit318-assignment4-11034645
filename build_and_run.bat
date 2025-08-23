@echo off
echo Medical Appointment Booking System - Build and Run Script
echo ========================================================
echo.

echo Building the application...
dotnet build MedicalAppointmentSystem/MedicalAppointmentSystem.csproj

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Build successful! Running the application...
    echo.
    dotnet run --project MedicalAppointmentSystem/MedicalAppointmentSystem.csproj
) else (
    echo.
    echo Build failed! Please check the error messages above.
    pause
)
