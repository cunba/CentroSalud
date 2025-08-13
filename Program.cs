using Functions;

// create storage dir if not exists
string dirPath = "Storage";
string appointmentsPath = $"{dirPath}/Appointments.json";
string doctorsPath = $"{dirPath}/Doctors.json";
string patientsPath = $"{dirPath}/Patients.json";
string specialtiesPath = $"{dirPath}/Specialties.json";

if (!Directory.Exists(dirPath))
    Directory.CreateDirectory(dirPath);

var menu = new Menu(appointmentsPath, doctorsPath, patientsPath, specialtiesPath);
menu.Show();