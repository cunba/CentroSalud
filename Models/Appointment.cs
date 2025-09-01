using System.Text.Json;
using Spectre.Console;

namespace Models
{
    public class Appointment : IDBFunctions<Appointment>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Observations { get; set; }
        public bool Approved { get; set; } = false;
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }

        public Appointment() { }

        public Appointment(int id) { Id = id; }

        public Appointment(
            int id,
            DateTime date,
            string observations,
            Patient patient,
            Doctor doctor
        )
        {
            Id = id;
            Date = date;
            Observations = observations;
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

            // Add rows
            table.AddRow("Date", Date.ToString());
            table.AddRow("Patient", Patient.Name);
            table.AddRow("Doctor", Doctor.Name);
            table.AddRow("Address", Doctor.Address);
            table.AddRow("Specialty", Doctor.Specialty.Name);
            table.AddRow("Observations", Observations);
            table.AddRow("Approved", Approved.ToString());

            table.Centered();
            table.Caption("Press e to exit");
            table.Columns[0].Padding(2, 4);

            // Render the table to the console
            AnsiConsole.Write(table);
        }

        public void ShowList()
        {
            var grid = new Grid();
            grid.AddColumn();
            grid.AddColumn();

            grid.AddRow(new Text[]{
                new Text("Date:").RightJustified(),
                new Text(Date.ToString())
            });
            grid.AddRow(new Text[]{
                new Text("Patient:").RightJustified(),
                new Text(Patient.Name)
            });
            grid.AddRow(new Text[]{
                new Text("Doctor:").RightJustified(),
                new Text(Doctor.Name)
            });
            grid.AddRow(new Text[]{
                new Text("Address:").RightJustified(),
                new Text(Doctor.Address)
            });
            grid.AddRow(new Text[]{
                new Text("Specialty:").RightJustified(),
                new Text(Doctor.Specialty.Name)
            });
            grid.AddRow(new Text[]{
                new Text("Observations:").RightJustified(),
                new Text(Observations)
            });
            grid.AddRow(new Text[]{
                new Text("Approved:").RightJustified(),
                new Text(Approved.ToString())
            });

            var panel = new Panel(grid)
            .Border(BoxBorder.Rounded)
            .Header($"ID {Id.ToString()}", Justify.Center);
            panel.Width = 65;

            AnsiConsole.Write(panel);
        }
    }
}