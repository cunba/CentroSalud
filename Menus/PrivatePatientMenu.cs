using Models;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;
using System.Text.Json;

namespace Menus
{
    public class PrivatePatientMenu(
            string appointmentsPath,
            string doctorsPath,
            string patientsPath,
            string specialtiesPath,
            Patient patient,
            int appointmentId
        )
    {
        private string AppointmentsPath = appointmentsPath;
        private string DoctorsPath = doctorsPath;
        private string PatientsPath = patientsPath;
        private string SpecialtiesPath = specialtiesPath;
        private Patient Patient = patient;
        private int AppointmentId = appointmentId;

        public void Show()
        {
            AnsiConsole.Write(new Rule("PRIVATE PATIENT MENU"));
            Console.WriteLine();

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select what you want to do:")
                    .AddChoices(new[] {
                        "See user information",
                        "Create a new appointment",
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
                case "Create a new appointment":
                    Console.Clear();
                    CreateAppointment();
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
            AnsiConsole.Write(new Rule("PRIVATE PATIENT MENU"));
            Console.WriteLine();

            Patient.Show();

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
            AnsiConsole.Write(new Rule("PRIVATE PATIENT MENU"));
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
                        "Age",
                        "Weight",
                        "Height"
                    }));

            var updatePatient = new Patient(Patient);
            foreach (string attribute in attributes)
            {
                switch (attribute)
                {
                    case "Name":
                        updatePatient.Name = AnsiConsole.Ask<string>("New name: ");
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

                            if (oldPassword != Patient.Password)
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
                        while (oldPassword != Patient.Password || newPassword != newRepeatPassword || newPassword == "");
                        updatePatient.Password = newPassword;
                        break;
                    case "Age":
                        updatePatient.Age = AnsiConsole.Ask<int>("New age: ");
                        break;
                    case "Weight":
                        updatePatient.Weight = AnsiConsole.Ask<decimal>("New weight: ");
                        break;
                    case "Height":
                        updatePatient.Height = AnsiConsole.Ask<decimal>("New height: ");
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
                    Patient = updatePatient;
                    Patient.Update(PatientsPath);
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

        private void CreateAppointment()
        {
            AnsiConsole.Write(new Rule("PRIVATE PATIENT MENU"));
            Console.WriteLine();

            var jsonString = File.ReadAllText(SpecialtiesPath);
            var specialties = JsonSerializer.Deserialize<Dictionary<int, Specialty>>(jsonString)!;
            var specialtiesSelect = new List<string>();

            foreach (var specialty in specialties.Values)
            {
                specialtiesSelect.Add(specialty.Name);
            }

            var specialtyOptions = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select specialty:")
                .AddChoices(specialtiesSelect));

            var key = specialties.FirstOrDefault(x => x.Value.Name == specialtyOptions).Key;
            var specialtySelected = new Specialty(specialties[key]);

            jsonString = File.ReadAllText(DoctorsPath);
            var doctors = JsonSerializer.Deserialize<Dictionary<int, Doctor>>(jsonString)!;
            var doctorsSelect = new List<string>();
            var specialtyDoctors = doctors
                .Where(x => x.Value.Specialty.Id == specialtySelected.Id && x.Value.Active)
                .ToDictionary(x => x.Key, x => x.Value);

            foreach (var doctor in specialtyDoctors.Values)
            {
                doctorsSelect.Add(doctor.Name);
            }

            var doctorsOption = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select available doctor:")
                .AddChoices(doctorsSelect));

            key = doctors.FirstOrDefault(x => x.Value.Name == doctorsOption).Key;
            var doctorSelected = new Doctor(doctors[key]);
            Dictionary<int, Appointment> doctorSelectedAppointments = new Dictionary<int, Appointment>();
            try
            {
                jsonString = File.ReadAllText(AppointmentsPath);
                var appointments = JsonSerializer.Deserialize<Dictionary<int, Appointment>>(jsonString)!;
                doctorSelectedAppointments = appointments
                    .Where(x => x.Value.Doctor.Id == doctorSelected.Id && x.Value.Date.Year == DateTime.Now.Year && x.Value.Date.Month == DateTime.Now.Month)
                    .ToDictionary(x => x.Key, x => x.Value);
            }
            catch { }

            var calendar = new Calendar(DateTime.Now.Year, DateTime.Now.Month);

            foreach (var appointment in doctorSelectedAppointments.Values)
            {
                calendar.AddCalendarEvent(appointment.Date.Year, appointment.Date.Month, appointment.Date.Day);
                calendar.HighlightStyle(Style.Parse("red bold"));
            }
            AnsiConsole.Write(calendar);

            var day = 0;
            do
            {

                day = AnsiConsole.Ask<int>("Write day: ");
            } while (day == 0 || day > 31);

            var observations = AnsiConsole.Prompt(
                new TextPrompt<bool>("Any observations?")
                    .AddChoice(true)
                    .AddChoice(false)
                    .WithConverter(choice => choice ? "y" : "n"));

            var observation = "";
            if (observations)
                observation = AnsiConsole.Ask<string>("Observation: ");

            var optionSave = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select an option:")
                .AddChoices(new[] {
                        "Save",
                        "Go back",
                        "Exit"
                }));

            if (optionSave == "Save")
            {
                var newAppointment = new Appointment(
                    AppointmentId,
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, day, 17, 0, 0),
                    observation,
                    Patient,
                    doctorSelected
                );

                newAppointment.Save(AppointmentsPath);
                AppointmentId++;

                Console.WriteLine("Appointment saved");
                Thread.Sleep(2000);
                Console.Clear();
                Show();
            }
            else if (optionSave == "Go back")
            {
                Console.Clear();
                Show();
            }
            else if (optionSave == "Exit")
            {
                AnsiConsole.Write(new Rule("GOODBYE"));
            }
        }

        private void ShowCalendar()
        {
            AnsiConsole.Write(new Rule("PRIVATE PATIENT MENU"));
            Console.WriteLine();

            var calendar = new Calendar(DateTime.Now.Year, DateTime.Now.Month);
            var jsonString = File.ReadAllText(AppointmentsPath);
            var appointments = JsonSerializer.Deserialize<Dictionary<int, Appointment>>(jsonString)!;
            var monthAppointments = appointments
                .Where(x => x.Value.Patient.Id == Patient.Id && x.Value.Date.Year == DateTime.Now.Year && x.Value.Date.Month == DateTime.Now.Month)
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
                                "Delete",
                                "Go back",
                                "Exit"
                            }));
                    if (options == "Delete")
                    {
                        var appId = AnsiConsole.Ask<int>("Write appointment ID to delete: ");
                        var appToDelete = dayAppointments.Select(a => a.Value).FirstOrDefault(a => a.Id == appId);
                        if (appToDelete == null)
                        {
                            Console.WriteLine("Appointment not found. Data not deleted.");
                        }
                        else
                        {
                            appToDelete.Delete(AppointmentsPath);
                            Console.WriteLine("Data deleted.");
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
            AnsiConsole.Write(new Rule("PRIVATE PATIENT MENU"));
            Console.WriteLine();

            dynamic options = "";
            try
            {
                var jsonString = File.ReadAllText(AppointmentsPath);
                var appointments = JsonSerializer.Deserialize<Dictionary<int, Appointment>>(jsonString)!;
                var monthAppointments = appointments
                    .Where(x => x.Value.Patient.Id == Patient.Id && x.Value.Date.Year == DateTime.Now.Year && x.Value.Date.Month == DateTime.Now.Month)
                    .ToDictionary(x => x.Key, x => x.Value);

                foreach (var appointment in monthAppointments.Values)
                {
                    appointment.ShowList();
                }

                options = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select an option:")
                        .AddChoices(new[] {
                        "Delete",
                        "Go back",
                        "Exit"
                        }));

                if (options == "Delete")
                {
                    var appId = AnsiConsole.Ask<int>("Write appointment ID to delete: ");
                    var appToDelete = appointments.Select(a => a.Value).FirstOrDefault(a => a.Id == appId);
                    if (appToDelete == null)
                    {
                        Console.WriteLine("Appointment not found. Data not deleted.");
                    }
                    else
                    {
                        appToDelete.Delete(AppointmentsPath);
                        Console.WriteLine("Data deleted.");
                    }
                    Thread.Sleep(2000);
                    Console.Clear();
                    Show();
                    return;
                }
            }
            catch
            {
                Console.WriteLine("No appointments to show for this month.");
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