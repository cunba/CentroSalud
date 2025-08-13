using Spectre.Console;
using Models;
using System;
using System.Text.Json;


// create storage dir if not exists
string dirPath = "Storage";
string appointmentsPath = $"{dirPath}/Appointments.json";
string doctorsPath = $"{dirPath}/Doctors.json";
string patientsPath = $"{dirPath}/Patients.json";
string specialtiesPath = $"{dirPath}/Specialties.json";

if (!Directory.Exists(dirPath))
    Directory.CreateDirectory(dirPath);

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
        Console.WriteLine("log in");
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