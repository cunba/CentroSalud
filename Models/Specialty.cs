using System.Text.Json;
using Spectre.Console;

namespace Models
{
    public class Specialty : IDBFunctions<Specialty>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Availability { get; set; } = 100m;
        public DateTime CloseScheduleDate { get; set; }
        public bool ScheduleFull { get; set; }

        public Specialty() { }

        public Specialty(int id) { Id = id; }

        public Specialty(
            int id,
            string name,
            DateTime closeScheduleDate,
            bool scheduleFull
        )
        {
            Id = id;
            Name = name;
            CloseScheduleDate = closeScheduleDate;
            ScheduleFull = scheduleFull;
        }

        public Specialty(Specialty specialty)
        {
            Id = specialty.Id;
            Name = specialty.Name;
            CloseScheduleDate = specialty.CloseScheduleDate;
            ScheduleFull = specialty.ScheduleFull;
        }

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

            // Add rows
            table.AddRow("Name", Name);
            table.AddRow("Close schedule date", CloseScheduleDate.ToString());
            table.AddRow("Schedule full", ScheduleFull.ToString());

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
                new Text("Name:").RightJustified(),
                new Text(Name)
            });
            grid.AddRow(new Text[]{
                new Text("Close schedule date:").RightJustified(),
                new Text(CloseScheduleDate.ToString())
            });
            grid.AddRow(new Text[]{
                new Text("Schedule full:").RightJustified(),
                new Text(ScheduleFull.ToString())
            });

            var panel = new Panel(grid)
            .Border(BoxBorder.Rounded)
            .Header($"ID {Id.ToString()}", Justify.Center);

            AnsiConsole.Write(Align.Center(panel));
        }
    }
}