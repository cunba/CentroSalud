using System.Text.Json;
using Spectre.Console;

namespace Models
{
    public class Doctor : User, IDBFunctions<Doctor>
    {
        public string Address { get; set; }
        public bool ScheduleFull { get; set; } = false;
        public int NAppPerMonth { get; set; } = 10;
        public Specialty Specialty { get; set; }

        public Doctor() { }

        public Doctor(int id) { Id = id; }

        public Doctor(
            int id,
            string name,
            string password,
            string address,
            Specialty specialty
        ) : base(id, name, password)
        {
            Address = address;
            Specialty = specialty;
        }

        public Doctor(Doctor doctor) : base(doctor.Id, doctor.Name, doctor.Password)
        {
            Address = doctor.Address;
            Specialty = doctor.Specialty;
        }

        public override void Save(string filePath)
        {

            ((IDBFunctions<Doctor>)this).Save(filePath, this, Id);
        }

        public override void Delete(string filePath)
        {
            ((IDBFunctions<Doctor>)this).Delete(filePath, Id);
        }

        public override void Update(string filePath)
        {
            ((IDBFunctions<Doctor>)this).Update(filePath, this, Id);
        }

        public override void Get(string filePath)
        {
            var doctor = ((IDBFunctions<Doctor>)this).Get<Doctor>(filePath, Id);

            if (doctor != null)
            {
                Id = doctor.Id;
                Name = doctor.Name;
                Password = doctor.Password;
                Address = doctor.Address;
                ScheduleFull = doctor.ScheduleFull;
                NAppPerMonth = doctor.NAppPerMonth;
                Specialty = doctor.Specialty;
                Active = doctor.Active;
                Created = doctor.Created;
                Appointments = doctor.Appointments;
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
                Title = new TableTitle($"Doctor {Id} information")
            };

            table.AddColumn("Attributtes");
            table.AddColumn("Information");
            table.AddColumn("Change it");

            // Add rows
            table.AddRow("Name", Name, "Press 1");
            table.AddRow("Password", "*********", "Press 2");
            table.AddRow("Address", Address, "Press 3");
            table.AddRow("Schedule full", ScheduleFull.ToString(), "Press 4");
            table.AddRow("Appointments per month", NAppPerMonth.ToString(), "Press 5");
            table.AddRow("Specialty", Specialty.Name, "Press 6");
            table.AddRow("Created at", Created.ToString(), "");
            table.AddRow("Active", Active.ToString(), "Press 7");

            table.Centered();
            table.Caption("Press b to go back");
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
                new Text("Name:").RightJustified(),
                new Text(Name)
            });
            grid.AddRow(new Text[]{
                new Text("Address:").RightJustified(),
                new Text(Address)
            });
            grid.AddRow(new Text[]{
                new Text("Schedule full:").RightJustified(),
                new Text(ScheduleFull.ToString())
            });
            grid.AddRow(new Text[]{
                new Text("Appointments per month:").RightJustified(),
                new Text(NAppPerMonth.ToString())
            });
            grid.AddRow(new Text[]{
                new Text("Specialty:").RightJustified(),
                new Text(Specialty.Name)
            });

            var panel = new Panel(grid)
            .Border(BoxBorder.Rounded)
            .Header($"ID {Id.ToString()}", Justify.Center);
            panel.Width = 65;

            AnsiConsole.Write(panel);
        }
    }
}
