using Microsoft.AspNetCore.Mvc;
using PatientAPI.Interfaces;
using PatientAPI.ViewModels.Patient.Request;
using PatientAPI.ViewModels.Patient.Response;


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

        [HttpPost("upload-photo/{patientId}")]
        public async Task<IActionResult> UploadPhoto(int patientId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var fileName = $"{patientId}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", fileName);

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var photoUrl = $"/photos/{fileName}";
            await _patientService.UpdatePatientPhoto(patientId, photoUrl);

            return Ok(new { PhotoUrl = photoUrl });
        }


    }
}