using Microsoft.EntityFrameworkCore;
using PatientAPI.Interfaces;
using PatientAPI.Models;
using PatientAPI.ViewModels.Patient.Request;
using PatientAPI.ViewModels.Patient.Response;


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
    }
}
