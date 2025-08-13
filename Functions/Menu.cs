using Spectre.Console;
using Models;
using System;
using System.Text.Json;

namespace Functions
{
    public class Menu(
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
            var rule = new Rule("WELCOME TO SALUD CONSOLE");
            AnsiConsole.Write(rule);
            Console.WriteLine();

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select what you want to do:")
                    .PageSize(10)
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
                    SeeSpecialties(specialtiesPath);
                    break;
                case "See doctors":
                    Console.Clear();
                    SeeDoctors(doctorsPath);
                    break;
                case "Log in":
                    Console.Clear();
                    LogIn(patientsPath, doctorsPath);
                    break;
                case "Register":
                    Console.Clear();
                    Console.WriteLine("resgister");
                    break;
                case "Exit":
                    Console.Clear();
                    Console.WriteLine("exit");
                    break;
            }
        }

        static void SeeSpecialties(string filePath)
        {
            var rule = new Rule("AVAILABLE SPECIALTIES");
            AnsiConsole.Write(rule);

            Console.WriteLine();

            var jsonString = File.ReadAllText(filePath);
            var specialties = JsonSerializer.Deserialize<Dictionary<int, Specialty>>(jsonString)!;

            foreach (var specialty in specialties.Values)
            {
                new Specialty(specialty).ShowList();
            }
        }

        static void SeeDoctors(string filePath)
        {
            var rule = new Rule("AVAILABLE DOCTORS");
            AnsiConsole.Write(rule);

            Console.WriteLine();

            var jsonString = File.ReadAllText(filePath);
            var doctors = JsonSerializer.Deserialize<Dictionary<int, Doctor>>(jsonString)!;

            foreach (var doctor in doctors.Values)
            {
                new Doctor(doctor).ShowList();
            }
        }

        static void LogIn(string patientsPath, string doctorsPath)
        {
            var id = AnsiConsole.Ask<int>("ID: ");
            var password = AnsiConsole.Prompt(new TextPrompt<string>("Password:").Secret());

            // Echo the name and age back to the terminal
            AnsiConsole.WriteLine($"So you're {id} and you're password {password}");
        }
    }
}