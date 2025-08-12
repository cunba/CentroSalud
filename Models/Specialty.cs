namespace Models
{
    public class Specialty(
        int id,
        string name,
        DateTime closeScheduleDate,
        bool scheduleFull
    )
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;
        public decimal Availability { get; set; } = 100m;
        public DateTime CloseScheduleDate { get; set; } = closeScheduleDate;
        public bool ScheduleFull { get; set; } = scheduleFull;
        public List<Doctor> Doctors { get; set; } = new List<Doctor>();

        public void Save(string filePath)
        {

            ((IDBFunctions<Specialty>)this).Save(filePath, this, Id);
        }

        public void Delete(string filePath)
        {
            ((IDBFunctions<Specialty>)this).Delete(filePath, Id);
        }

        public void Update(string filePath)
        {
            ((IDBFunctions<Specialty>)this).Update(filePath, this, Id);
        }

        public void Get(string filePath)
        {
            try
            {
                var appointment = ((IDBFunctions<Specialty>)this).Get<Specialty>(filePath, Id);

                if (appointment != null)
                {
                    Id = appointment.Id;
                    Name = appointment.Name;
                    Availability = appointment.Availability;
                    CloseScheduleDate = appointment.CloseScheduleDate;
                    ScheduleFull = appointment.ScheduleFull;
                    Doctors = appointment.Doctors;
                }
            }
            catch
            {
                Console.WriteLine("Something went wrong");
            }
        }

        public void Show()
        {
            // Create a table
            var table = new Table
            {
                Title = new TableTitle($"Specialty {Id} information")
            };

            table.AddColumn("Attributtes");
            table.AddColumn("Information");
            table.AddColumn("Change it");

            // Add rows
            table.AddRow("Date", Date.ToString(), "Press 1");
            table.AddRow("Patient", Patient.Name, "Press 2");
            table.AddRow("Doctor", Doctor.Name, "Press 3");
            table.AddRow("Address", Doctor.Address, "");
            table.AddRow("Specialty", Doctor.Specialty.Name, "Press 4");
            table.AddRow("Observations", Observations, "Press 5");
            table.AddRow("Approved", Approved.ToString(), "");

            table.Centered();
            table.Caption("Press 8 to exit");
            table.Columns[0].Padding(2, 4);

            // Render the table to the console
            AnsiConsole.Write(table);
        }
    }
}