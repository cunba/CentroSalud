using System.Text.Json;
using Spectre.Console;

namespace Models
{
    public class Patient : User
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

        public override void Save(string filePath)
        {
            try
            {
                var jsonString = File.ReadAllText(filePath);
                var patients = JsonSerializer.Deserialize<Dictionary<int, Patient>>(jsonString)!;
                patients.Add(Id, this);

                var options = new JsonSerializerOptions { WriteIndented = true };
                jsonString = JsonSerializer.Serialize(patients, options);
                File.WriteAllText(filePath, jsonString);
            }
            catch
            {
                var patients = new Dictionary<int, Patient>();
                patients.Add(Id, this);


                var options = new JsonSerializerOptions { WriteIndented = true };
                var jsonString = JsonSerializer.Serialize(patients, options);
                File.WriteAllText(filePath, jsonString);
            }
        }

        public override void Delete(string filePath)
        {
            try
            {
                var jsonString = File.ReadAllText(filePath);
                var patients = JsonSerializer.Deserialize<Dictionary<int, Patient>>(jsonString)!;
                if (!patients.TryGetValue(Id, out Patient patient))
                {
                    Console.WriteLine("No user found");
                    return;
                }
                patients.Remove(Id);

                var options = new JsonSerializerOptions { WriteIndented = true };
                jsonString = JsonSerializer.Serialize(patients, options);
                File.WriteAllText(filePath, jsonString);
            }
            catch
            {
                Console.WriteLine("Something went wrong");
            }
        }

        public override void Update(string filePath)
        {
            try
            {
                var jsonString = File.ReadAllText(filePath);
                var patients = JsonSerializer.Deserialize<Dictionary<int, Patient>>(jsonString)!;
                if (!patients.TryGetValue(Id, out Patient patient))
                {
                    Console.WriteLine("No user found");
                    return;
                }
                else
                {
                    patients[Id] = this;
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                jsonString = JsonSerializer.Serialize(patients, options);
                File.WriteAllText(filePath, jsonString);
            }
            catch
            {
                Console.WriteLine("Something went wrong");
            }
        }

        public override void Get(string filePath)
        {
            try
            {
                var jsonString = File.ReadAllText(filePath);
                var patients = JsonSerializer.Deserialize<Dictionary<int, Patient>>(jsonString)!;
                if (!patients.TryGetValue(Id, out Patient patient))
                {
                    Console.WriteLine("No user found");
                    return;
                }
                else
                {
                    Id = patient.Id;
                    Name = patient.Name;
                    Password = patient.Password;
                    Age = patient.Age;
                    Weight = patient.Weight;
                    Height = patient.Height;
                    Active = patient.Active;
                    Created = patient.Created;
                    Appointments = patient.Appointments;
                }
            }
            catch
            {
                Console.WriteLine("Something went wrong");
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
            table.AddColumn("Change it");

            // Add rows
            table.AddRow("Name", Name, "Press 1");
            table.AddRow("Password", "*********", "Press 2");
            table.AddRow("Age", Age.ToString(), "Press 3");
            table.AddRow("Weight", Weight.ToString(), "Press 4");
            table.AddRow("Height", Height.ToString(), "Press 5");
            table.AddRow("Created at", Created.ToString(), "");
            table.AddRow("Active", Active.ToString(), "Press 6");

            table.Centered();
            table.Columns[0].Padding(2, 4);
            table.Caption("Press 8 to exit");

            // Render the table to the console
            AnsiConsole.Write(table);
        }
    }
}
