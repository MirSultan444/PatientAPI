using Microsoft.EntityFrameworkCore;
using PatientAPI.Interfaces;
using PatientAPI.Models;
using PatientAPI.ViewModels.Patient.Request;
using PatientAPI.ViewModels.Patient.Response;
using OfficeOpenXml;
using System.IO;

namespace PatientAPI.Services
{
    public class PatientService : IPatientService
    {
        private readonly AppDbContext _context;

        public PatientService(AppDbContext context)
        {
            _context = context;
        }

        public void CreatePatient(PatientCreateView request)
        {
            var patient = new Patient
            {
                Name = request.Name,
                Age = request.Age,
                Diagnosis = request.Diagnosis
            };

            _context.Patients.Add(patient);
            _context.SaveChanges();
        }

        public void UpdatePatient(PatientUpdateView request)
        {
            var patient = _context.Patients.Find(request.Id);
            if (patient != null)
            {
                patient.Name = request.Name;
                patient.Age = request.Age;
                patient.Diagnosis = request.Diagnosis;

                _context.SaveChanges();
            }
        }

        public void DeletePatient(int patientId)
        {
            var patient = _context.Patients.Find(patientId);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                _context.SaveChanges();
            }
        }

        public PatientDetailView GetPatient(int patientId)
        {
            var patient = _context.Patients.Find(patientId);
            if (patient == null) return null;

            return new PatientDetailView
            {
                Id = patient.Id,
                Name = patient.Name,
                Age = patient.Age,
                Diagnosis = patient.Diagnosis
            };
        }

        public async Task<PatientListView> GetPatients()
        {
            var patients = await _context.Patients.ToListAsync();
            return new PatientListView
            {
                Patients = patients.Select(p => new PatientDetailView
                {
                    Id = p.Id,
                    Name = p.Name,
                    Age = p.Age,
                    Diagnosis = p.Diagnosis
                }).ToList()
            };
        }

        public byte[] ExportPatientsToExcel()
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Patients");

            worksheet.Cells[1, 1].Value = "Id";
            worksheet.Cells[1, 2].Value = "Name";
            worksheet.Cells[1, 3].Value = "Age";
            worksheet.Cells[1, 4].Value = "Diagnosis";

            var patients = _context.Patients.ToList();
            for (int i = 0; i < patients.Count; i++)
            {
                var patient = patients[i];
                worksheet.Cells[i + 2, 1].Value = patient.Id;
                worksheet.Cells[i + 2, 2].Value = patient.Name;
                worksheet.Cells[i + 2, 3].Value = patient.Age;
                worksheet.Cells[i + 2, 4].Value = patient.Diagnosis;
            }

            return package.GetAsByteArray();
        }

        public async Task<Patient> GetPatientById(int patientId)
        {
            return await _context.Patients.FindAsync(patientId);
        }

        public async Task UpdatePatient(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePatientPhoto(int patientId, string photoUrl)
        {
            var patient = await GetPatientById(patientId);
            if (patient != null)
            {
                patient.PhotoLink = photoUrl;
                await UpdatePatient(patient);
            }
        }

    }
}