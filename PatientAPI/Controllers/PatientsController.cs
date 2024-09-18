using Microsoft.AspNetCore.Mvc;
using PatientAPI.Interfaces;
using PatientAPI.ViewModels.Patient.Request;
using PatientAPI.ViewModels.Patient.Response;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using System.Linq;


namespace PatientAPI.Controllers
{
    [Route("api/v1/patient")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;


        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }


        [HttpPost]
        public void CreatePatient([FromBody] PatientCreateView request)
        {
            _patientService.CreatePatient(request);
        }


        [HttpPatch]
        public void UpdatePatient([FromBody] PatientUpdateView request)
        {
            _patientService.UpdatePatient(request);
        }


        [HttpDelete]
        public void DeletePatient(int patientId)
        {
            _patientService.DeletePatient(patientId);
        }


        [HttpGet]
        public PatientDetailView GetPatient(int patientId)
        {
            var result = _patientService.GetPatient(patientId);
            return result;
        }


        [HttpGet("list")]
        public async Task<PatientListView> GetPatients()
        {
            var result = await _patientService.GetPatients();
            return result;
        }

        [HttpGet("export")]
        public IActionResult ExportPatients()
        {
            var excelFile = _patientService.ExportPatientsToExcel();
            var fileName = $"Patients_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
