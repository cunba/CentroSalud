namespace Models
{
    public class Appointment(
        int id,
        DateTime date,
        string observations,
        bool approved,
        Patient patient,
        Doctor doctor
    )
    {
        public int Id { get; set; } = id;
        public DateTime Date { get; set; } = date;
        public string Observations { get; set; } = observations;
        public bool Approved { get; set; } = approved;
        public Patient Patient { get; set; } = patient;
        public Doctor Doctor { get; set; } = doctor;

        public void Create(string filePath)
        {

        }

        public void Update(string filePath)
        {

        }

        public void Delete(string filePath)
        {

        }

        public void Show()
        {

        }
    }
}