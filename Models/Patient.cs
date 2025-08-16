using System.Text.Json;
using Spectre.Console;

namespace Models
{
    public class Patient : User, IDBFunctions<Patient>
    {
        public int Age { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }

        public Patient(
            int id,
            string name,
            string password,
            int age,
            decimal weight,
            decimal height
            ) : base(id, name, password)
        {
            Age = age;
            Weight = weight;
            Height = height;
        }

        public Patient(int id) : base(id) { }

        public Patient() { }

        public Patient(Patient patient)
        {
            Id = patient.Id;
            Name = patient.Name;
            Password = patient.Password;
            Age = patient.Age;
            Weight = patient.Weight;
            Height = patient.Height;
            Active = patient.Active;
            Created = patient.Created;
        }

        public override void Save(string filePath)
        {

            ((IDBFunctions<Patient>)this).Save(filePath, this, Id);
        }

        public override void Delete(string filePath)
        {
            ((IDBFunctions<Patient>)this).Delete(filePath, Id);
        }

        public override void Update(string filePath)
        {
            ((IDBFunctions<Patient>)this).Update(filePath, this, Id);
        }

        public override void Get(string filePath)
        {
            var patient = ((IDBFunctions<Patient>)this).Get<Patient>(filePath, Id);

            if (patient != null)
            {
                Id = patient.Id;
                Name = patient.Name;
                Password = patient.Password;
                Age = patient.Age;
                Weight = patient.Weight;
                Height = patient.Height;
                Active = patient.Active;
                Created = patient.Created;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public override void Show()
        {
            // Create a table
            var table = new Table
            {
                Title = new TableTitle($"Patient {Id} information")
            };

            table.AddColumn("Attributtes");
            table.AddColumn("Information");

            // Add rows
            table.AddRow("Name:", Name);
            table.AddRow("Password:", "*********");
            table.AddRow("Age", Age.ToString());
            table.AddRow("Weight", Weight.ToString());
            table.AddRow("Height", Height.ToString());
            table.AddRow("Created at", Created.ToString());
            table.AddRow("Active", Active.ToString());

            table.Centered();
            table.Columns[0].Padding(2, 4);

            // Render the table to the console
            AnsiConsole.Write(table);
        }
    }
}
