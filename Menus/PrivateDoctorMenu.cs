using Models;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;
using System.Text.Json;

namespace Menus
{
    public class PrivateDoctorMenu(
        string appointmentsPath,
            string doctorsPath,
            string patientsPath,
            string specialtiesPath,
            Doctor doctor,
            int appointmentId
        )
    {
        private string AppointmentsPath = appointmentsPath;
        private string DoctorsPath = doctorsPath;
        private string PatientsPath = patientsPath;
        private string SpecialtiesPath = specialtiesPath;
        private Doctor Doctor = doctor;
        private int AppointmentId = appointmentId;

        public void Show()
        {
            AnsiConsole.Write(new Rule("PRIVATE DOCTOR MENU"));
            Console.WriteLine();

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select what you want to do:")
                    .AddChoices(new[] {
                        "See user information",
                        "See calendar",
                        "See list of appointments",
                        "Exit"
                    }));

            switch (option)
            {
                case "See user information":
                    Console.Clear();
                    SeeInformation();
                    break;
                case "See calendar":
                    Console.Clear();
                    ShowCalendar();
                    break;
                case "See list of appointments":
                    Console.Clear();
                    ShowListAppointments();
                    break;
                case "Exit":
                    Console.WriteLine("Exiting...");
                    AnsiConsole.Write(new Rule("GOODBYE"));
                    break;
            }
        }

        private void SeeInformation()
        {
            AnsiConsole.Write(new Rule("PRIVATE DOCTOR MENU"));
            Console.WriteLine();

            Doctor.Show();

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(new[] {
                        "Manage information",
                        "Back",
                        "Exit"
                    }));

            switch (option)
            {
                case "Manage information":
                    Console.Clear();
                    ManageInformation();
                    break;
                case "Back":
                    Console.Clear();
                    Show();
                    break;
                case "Exit":
                    Console.WriteLine("Exiting...");
                    AnsiConsole.Write(new Rule("GOODBYE"));
                    break;
            }
        }

        private void ManageInformation()
        {
            AnsiConsole.Write(new Rule("PRIVATE DOCTOR MENU"));
            Console.WriteLine();

            var attributes = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("Select attributes to change:")
                    .NotRequired()
                    .InstructionsText(
                        "[grey](Press [blue]<space>[/] to select an attribute, " +
                        "[green]<enter>[/] to accept)[/]")
                    .AddChoices(new[] {
                        "Name",
                        "Password",
                        "Address"
                    }));

            var updateDoctor = new Doctor(Doctor);
            foreach (string attribute in attributes)
            {
                switch (attribute)
                {
                    case "Name":
                        updateDoctor.Name = AnsiConsole.Ask<string>("New name: ");
                        break;
                    case "Password":
                        var oldPassword = "";
                        var newPassword = "";
                        var newRepeatPassword = "";
                        do
                        {
                            oldPassword = AnsiConsole.Prompt(new TextPrompt<string>("Old password:").Secret());
                            newPassword = AnsiConsole.Prompt(new TextPrompt<string>("New password:").Secret());
                            newRepeatPassword = AnsiConsole.Prompt(new TextPrompt<string>("Repeat password:").Secret());

                            if (oldPassword != Doctor.Password)
                            {
                                AnsiConsole.Markup("[red3]Wrong current password[/]");
                                Console.WriteLine();
                            }
                            else if (newPassword != newRepeatPassword)
                            {
                                AnsiConsole.Markup("[red3]Passwords do not match[/]");
                                Console.WriteLine();
                            }
                        }
                        while (oldPassword != Doctor.Password || newPassword != newRepeatPassword || newPassword == "");
                        updateDoctor.Password = newPassword;
                        break;
                    case "Address":
                        updateDoctor.Address = AnsiConsole.Ask<string>("New address: ");
                        break;

                }
            }

            dynamic option = "";
            if (attributes.Count < 1)
                option = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .AddChoices(new[] {
                            "Back",
                            "Exit"
                        }));
            else
                option = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .AddChoices(new[] {
                            "Update",
                            "Back",
                            "Exit"
                        }));

            switch (option)
            {
                case "Update":
                    Console.Clear();
                    Doctor = updateDoctor;
                    Doctor.Update(DoctorsPath);
                    Console.WriteLine("User updated");
                    Thread.Sleep(2000);
                    Console.Clear();
                    Show();
                    break;
                case "Back":
                    Console.Clear();
                    Show();
                    break;
                case "Exit":
                    Console.WriteLine("Exiting...");
                    AnsiConsole.Write(new Rule("GOODBYE"));
                    break;
            }
        }

        private void ShowCalendar()
        {
            var calendar = new Calendar(DateTime.Now.Year, DateTime.Now.Month);
            var jsonString = File.ReadAllText(AppointmentsPath);
            var appointments = JsonSerializer.Deserialize<Dictionary<int, Appointment>>(jsonString)!;
            var monthAppointments = appointments
                .Where(x => x.Value.Doctor.Id == Doctor.Id && x.Value.Date.Year == DateTime.Now.Year && x.Value.Date.Month == DateTime.Now.Month)
                .ToDictionary(x => x.Key, x => x.Value);

            foreach (var appointment in monthAppointments.Values)
            {
                calendar.AddCalendarEvent(appointment.Date.Year, appointment.Date.Month, appointment.Date.Day);
                calendar.HighlightStyle(Style.Parse("red bold"));
            }
            AnsiConsole.Write(calendar);

            dynamic options = "";
            if (monthAppointments.Count() != 0)
                options = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an option:")
                    .AddChoices(new[] {
                        "See day appointments",
                        "Go back",
                        "Exit"
                    }));
            else
                options = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an option:")
                    .AddChoices(new[] {
                        "Go back",
                        "Exit"
                    }));

            if (options == "See day appointments")
            {
                var day = 0;
                do
                {
                    day = AnsiConsole.Ask<int>("Write day: ");
                } while (day == 0 || day > 31);

                var dayAppointments = monthAppointments.Where(x => x.Value.Date.Day == day).ToDictionary(x => x.Key, x => x.Value);
                if (dayAppointments.Count() == 0)
                {
                    Console.WriteLine("No data");
                    Console.WriteLine();
                    options = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Select an option:")
                            .AddChoices(new[] {
                                "Go back",
                                "Exit"
                            }));
                }
                else
                {
                    foreach (var appointment in dayAppointments.Values)
                    {
                        appointment.ShowList();
                    }

                    options = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Select an option:")
                            .AddChoices(new[] {
                                "Approve",
                                "Go back",
                                "Exit"
                            }));
                    
                    if (options == "Approve")
                    {
                        var appId = AnsiConsole.Ask<int>("Write appointment ID to approved: ");
                        var appToApprove = dayAppointments.Select(a => a.Value).FirstOrDefault(a => a.Id == appId);
                        if (appToApprove == null)
                        {
                            Console.WriteLine("Appointment not found. Data not approved.");
                        }
                        else
                        {
                            appToApprove.Approved = true;
                            appToApprove.Update(AppointmentsPath);
                            Console.WriteLine("Data approved.");
                        }
                        Thread.Sleep(2000);
                        Console.Clear();
                        Show();
                        return;
                    }
                }
            }

            if (options == "Go back")
            {
                Console.Clear();
                Show();
            }
            else if (options == "Exit")
            {
                AnsiConsole.Write(new Rule("GOODBYE"));
            }
        }

        private void ShowListAppointments()
        {
            dynamic options = "";
            try
            {
                var jsonString = File.ReadAllText(AppointmentsPath);
                var appointments = JsonSerializer.Deserialize<Dictionary<int, Appointment>>(jsonString)!;
                var monthAppointments = appointments
                    .Where(x => x.Value.Doctor.Id == Doctor.Id && x.Value.Date.Year == DateTime.Now.Year && x.Value.Date.Month == DateTime.Now.Month)
                    .ToDictionary(x => x.Key, x => x.Value);

                foreach (var appointment in monthAppointments.Values)
                {
                    appointment.ShowList();
                }

                options = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select an option:")
                        .AddChoices(new[] {
                        "Approve",
                        "Go back",
                        "Exit"
                        }));

                if (options == "Approve")
                {
                    var appId = AnsiConsole.Ask<int>("Write appointment ID to approve: ");
                    var appToApprove = appointments.Select(a => a.Value).FirstOrDefault(a => a.Id == appId);
                    if (appToApprove == null)
                    {
                        Console.WriteLine("Appointment not found. Data not approved.");
                    }
                    else
                    {
                        appToApprove.Approved = true;
                        appToApprove.Update(AppointmentsPath);
                        Console.WriteLine("Data approved.");
                    }
                    Thread.Sleep(2000);
                    Console.Clear();
                    Show();
                    return;
                }
            }
            catch
            {
                Console.WriteLine("No appointments to show for this month");
                options = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select an option:")
                        .AddChoices(new[] {
                            "Go back",
                            "Exit"
                        }));
            }

            if (options == "Go back")
            {
                Console.Clear();
                Show();
            }
            else if (options == "Exit")
            {
                AnsiConsole.Write(new Rule("GOODBYE"));
            }
        }
    }
}