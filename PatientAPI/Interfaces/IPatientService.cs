using PatientAPI.ViewModels.Patient.Request;
using PatientAPI.ViewModels.Patient.Response;
using System.Threading.Tasks;

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
    }
}
