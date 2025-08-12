using System.Text.Json;
using Spectre.Console;

namespace Models
{
    public class Doctor(
        int id,
        string name,
        string password,
        string direction,
        Specialty specialty
        ) : User(id, name, password)
    {
        public string Direction { get; set; } = direction;
        public bool ScheduleFull { get; set; } = false;
        public int NAppPerMonth { get; set; } = 10;
        public Specialty Specialty { get; set; } = specialty;

        public override void Save(string filePath)
        {
            try
            {
                var jsonString = File.ReadAllText(filePath);
                var doctors = JsonSerializer.Deserialize<Dictionary<int, Doctor>>(jsonString)!;
                doctors.Add(Id, this);

                var options = new JsonSerializerOptions { WriteIndented = true };
                jsonString = JsonSerializer.Serialize(doctors, options);
                File.WriteAllText(filePath, jsonString);
            }
            catch
            {
                var doctors = new Dictionary<int, Doctor>();
                doctors.Add(Id, this);


                var options = new JsonSerializerOptions { WriteIndented = true };
                var jsonString = JsonSerializer.Serialize(doctors, options);
                File.WriteAllText(filePath, jsonString);
            }
        }

        public override void Delete(string filePath)
        {
            try
            {
                var jsonString = File.ReadAllText(filePath);
                var doctors = JsonSerializer.Deserialize<Dictionary<int, Doctor>>(jsonString)!;
                if (!doctors.TryGetValue(Id, out Doctor doctor))
                {
                    Console.WriteLine("No user found");
                    return;
                }
                doctors.Remove(Id);

                var options = new JsonSerializerOptions { WriteIndented = true };
                jsonString = JsonSerializer.Serialize(doctors, options);
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
                var doctors = JsonSerializer.Deserialize<Dictionary<int, Doctor>>(jsonString)!;
                if (!doctors.TryGetValue(Id, out Doctor doctor))
                {
                    Console.WriteLine("No user found");
                    return;
                }
                else
                {
                    doctors[Id] = this;
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                jsonString = JsonSerializer.Serialize(doctors, options);
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
                var doctors = JsonSerializer.Deserialize<Dictionary<int, Doctor>>(jsonString)!;
                if (!doctors.TryGetValue(Id, out Doctor doctor))
                {
                    Console.WriteLine("No user found");
                    return;
                }
                else
                {
                    Id = doctor.Id;
                    Name = doctor.Name;
                    Password = doctor.Password;
                    Direction = doctor.Direction;
                    ScheduleFull = doctor.ScheduleFull;
                    NAppPerMonth = doctor.NAppPerMonth;
                    Specialty = doctor.Specialty;
                    Active = doctor.Active;
                    Created = doctor.Created;
                    Appointments = doctor.Appointments;
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
                Title = new TableTitle($"Doctor {Id} information")
            };

            table.AddColumn("Attributtes");
            table.AddColumn("Information");
            table.AddColumn("Change it");

            // Add rows
            table.AddRow("Name", Name, "Press 1");
            table.AddRow("Password", "*********", "Press 2");
            table.AddRow("Direction", Direction, "Press 3");
            table.AddRow("Schedule full", ScheduleFull.ToString(), "Press 4");
            table.AddRow("Appointments per month", NAppPerMonth.ToString(), "Press 5");
            table.AddRow("Specialty", Specialty.Name, "Press 6");
            table.AddRow("Created at", Created.ToString(), "");
            table.AddRow("Active", Active.ToString(), "Press 7");

            table.Centered();
            table.Caption("Press 8 to exit");
            table.Columns[0].Padding(2, 4);

            // Render the table to the console
            AnsiConsole.Write(table);
        }
    }
}
