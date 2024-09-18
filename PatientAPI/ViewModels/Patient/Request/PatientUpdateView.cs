namespace PatientAPI.ViewModels.Patient.Request
{
    public class PatientUpdateView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Diagnosis { get; set; }
    }
}
