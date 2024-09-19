using PatientAPI.Models;
using PatientAPI.ViewModels.Patient.Request;
using PatientAPI.ViewModels.Patient.Response;

namespace PatientAPI.Interfaces
{
    public interface IPatientService
    {
        void CreatePatient(PatientCreateView request);
        void UpdatePatient(PatientUpdateView request);
        void DeletePatient(int patientId);
        PatientDetailView GetPatient(int patientId);
        Task<PatientListView> GetPatients();
        byte[] ExportPatientsToExcel();
        Task<Patient> GetPatientById(int patientId);
        Task UpdatePatient(Patient patient);
        Task UpdatePatientPhoto(int patientId, string photoUrl);
    }
}
