using System.Text.Json;
using Spectre.Console;

namespace Models
{
    public class Doctor : User, IDBFunctions<Doctor>
    {
        public string Address { get; set; }
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
                Specialty = doctor.Specialty;
                Active = doctor.Active;
                Created = doctor.Created;
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

            // Add rows
            table.AddRow("Name", Name);
            table.AddRow("Password", "*********");
            table.AddRow("Address", Address);
            table.AddRow("Specialty", Specialty.Name);
            table.AddRow("Created at", Created.ToString());
            table.AddRow("Active", Active.ToString());

            table.Centered();
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
