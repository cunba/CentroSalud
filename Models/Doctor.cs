namespace Models
{
    public class Doctor(
        int id,
        string name,
        string direction,
        Specialty specialty
        ) : User(id, name), User
    {
        public string Direction { get; set; } = direction;
        public bool ScheduleFull { get; set; } = false;
        public Specialty Specialty { get; set; } = new Specialty();
    }
}
