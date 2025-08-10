namespace Models
{
    public class Specialty(
        int id,
        string name,
        decimal availability,
        DateTime closeScheduleDate,
        bool scheduleFull
    )
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;
        public decimal Availability { get; set; } = availability;
        public DateTime CloseScheduleDate { get; set; } = closeScheduleDate;
        public bool ScheduleFull { get; set; } = scheduleFull;
        public List<Doctors> Doctors { get; set; } = new List<Doctors>();

        public void Create(string filePath)
        {

        }

        public void Update(string filePath)
        {

        }

        public void AddDoctor(Doctor doctor)
        {

        }

        public void Show()
        {
            
        }
    }
}