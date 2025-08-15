using Spectre.Console;
using Models;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Menus
{
    public class PublicMenu(
        string appointmentsPath,
        string doctorsPath,
        string patientsPath,
        string specialtiesPath
    )
    {
        public string AppointmentsPath = appointmentsPath;
        public string DoctorsPath = doctorsPath;
        public string PatientsPath = patientsPath;
        public string SpecialtiesPath = specialtiesPath;

        public void Show()
        {
            AnsiConsole.Write(new Rule("WELCOME TO SALUD CONSOLE"));
            Console.WriteLine();

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select what you want to do:")
                    .AddChoices(new[] {
                        "See available specialties",
                        "See doctors",
                        "Log in",
                        "Register",
                        "Exit"
                    }));

            switch (option)
            {
                case "See available specialties":
                    Console.Clear();
                    SeeSpecialties();
                    break;
                case "See doctors":
                    Console.Clear();
                    SeeDoctors();
                    break;
                case "Log in":
                    Console.Clear();
                    LogIn();
                    break;
                case "Register":
                    Console.Clear();
                    Register();
                    break;
                case "Exit":
                    Console.WriteLine("Exiting...");
                    AnsiConsole.Write(new Rule("GOODBYE"));
                    break;
            }
        }

        void SeeSpecialties()
        {
            AnsiConsole.Write(new Rule("AVAILABLE SPECIALTIES"));

            Console.WriteLine();

            var jsonString = File.ReadAllText(SpecialtiesPath);
            var specialties = JsonSerializer.Deserialize<Dictionary<int, Specialty>>(jsonString)!;

            foreach (var specialty in specialties.Values)
            {
                new Specialty(specialty).ShowList();
            }
        }

        void SeeDoctors()
        {
            AnsiConsole.Write(new Rule("AVAILABLE DOCTORS"));

            Console.WriteLine();

            var jsonString = File.ReadAllText(DoctorsPath);
            var doctors = JsonSerializer.Deserialize<Dictionary<int, Doctor>>(jsonString)!;

            foreach (var doctor in doctors.Values)
            {
                if (doctor.Active == true)
                    new Doctor(doctor).ShowList();
            }
        }

        void LogIn()
        {
            var id = AnsiConsole.Ask<int>("ID: ");
            var password = AnsiConsole.Prompt(new TextPrompt<string>("Password:").Secret());

            dynamic user = new Patient(id);
            try
            {
                user.Get(PatientsPath);
            }
            catch
            {
                user = new Doctor(id);

                try
                {
                    user.Get(DoctorsPath);
                    user.Show();
                }
                catch
                {
                    return;
                }
            }

            // user.GetType();
            Console.WriteLine(user.GetType().FullName);
        }

        void Register()
        {
            var id = AnsiConsole.Ask<int>("ID(DNI number): ");
            var name = AnsiConsole.Ask<string>("Complete name: ");
            var password = AnsiConsole.Prompt(new TextPrompt<string>("Password:").Secret());
            var type = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Type of account:")
                    .AddChoices(new[] {
                        "Patient",
                        "Doctor"
                    }));

            Console.WriteLine($"Type: {type}");
            dynamic user;

            if (type == "Patient")
            {
                var age = AnsiConsole.Ask<int>("Age: ");
                var weight = AnsiConsole.Ask<decimal>("Weight: ");
                var height = AnsiConsole.Ask<decimal>("Height: ");

                user = new Patient(id, name, password, age, weight, height);
            }
            else
            {
                var address = AnsiConsole.Ask<string>("Address: ");

                var jsonString = File.ReadAllText(SpecialtiesPath);
                var specialties = JsonSerializer.Deserialize<Dictionary<int, Specialty>>(jsonString)!;
                var specialtiesSelect = new List<string>();

                foreach (var specialty in specialties.Values)
                {
                    specialtiesSelect.Add(specialty.Name);
                }

                var optionRegister = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select what you want to do:")
                    .AddChoices(specialtiesSelect));

                var key = specialties.FirstOrDefault(x => x.Value.Name == optionRegister).Key;
                var specialtySelected = new Specialty(specialties[key]);
                Console.WriteLine($"Specialty: {specialtySelected.Name}");

                user = new Doctor(id, name, password, address, specialtySelected);
            }

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an option:")
                    .AddChoices(new[] {
                        "Register",
                        "Go back",
                        "Exit"
                    }));

            if (option == "Register")
            {
                user.Save(type == "Patient" ? PatientsPath : DoctorsPath);
                Console.WriteLine("User created");
                Thread.Sleep(2000);
                Console.Clear();
                Show();
            }
            else if (option == "Go back")
            {
                Console.Clear();
                Show();
            }
            else if (option == "Exit")
            {
                AnsiConsole.Write(new Rule("GOODBYE"));
            }
        }
    }
}