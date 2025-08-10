namespace Models
{
    public abstract class User(int id, string name, string password)
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;
        public string Password { get; set; } = password;
        public DateTime Created { get; set; } = DateTime.Now;
        public bool Active { get; set; } = true;
        public List<Appointment> Appointments = new List<Appointment>();

        public abstract void Save(string filePath);

        public abstract void Delete(string filePath);

        public abstract void Update(string filePath);

        public abstract void Upload(string filePath);

        public abstract void Show();
    }
}
