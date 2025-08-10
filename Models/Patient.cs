namespace Models
{
    public class Patient(
        int id,
        string name,
        int age,
        decimal weight,
        decimal height
        ) : User(id, name), User
    {
        public int Age { get; set; } = age;
        public decimal Weight { get; set; } = weight;
        public decimal Height { get; set; } = height;
    }

}
