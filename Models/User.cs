namespace Models
{
    public abstract class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Password { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.Now;
        public bool Active { get; set; } = true;

        public User(int id, string name, string password)
        {
            Id = id;
            Name = name;
            Password = password;
        }

        public User(int id) { Id = id; }

        public User() { }

        public abstract void Save(string dirPath);

        public abstract void Delete(string dirPath);

        public abstract void Update(string dirPath);

        public abstract void Get(string dirPath);

        public abstract void Show();
    }
}
