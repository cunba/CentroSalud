using System.Text.Json;
using Spectre.Console;

namespace Models
{
    public class Appointment : IDBFunctions<Appointment>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Observations { get; set; }
        public bool Approved { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }

        public Appointment() { }

        public Appointment(int id) { Id = id; }

        public Appointment(
            int id,
            DateTime date,
            string observations,
            bool approved,
            Patient patient,
            Doctor doctor
        )
        {
            Id = id;
            Date = date;
            Observations = observations;
            Approved = approved;
            Patient = patient;
            Doctor = doctor;
        }

        public void Save(string filePath)
        {

            ((IDBFunctions<Appointment>)this).Save(filePath, this, Id);
        }

        public void Delete(string filePath)
        {
            ((IDBFunctions<Appointment>)this).Delete(filePath, Id);
        }

        public void Update(string filePath)
        {
            ((IDBFunctions<Appointment>)this).Update(filePath, this, Id);
        }

        public void Get(string filePath)
        {
            try
            {
                var appointment = ((IDBFunctions<Appointment>)this).Get<Appointment>(filePath, Id);

                if (appointment != null)
                {
                    Id = appointment.Id;
                    Date = appointment.Date;
                    Observations = appointment.Observations;
                    Approved = appointment.Approved;
                    Patient = appointment.Patient;
                    Doctor = appointment.Doctor;
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
                Title = new TableTitle($"Appointment {Id} information")
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
            table.Caption("Press e to exit");
            table.Columns[0].Padding(2, 4);

            // Render the table to the console
            AnsiConsole.Write(table);
        }
    }
}