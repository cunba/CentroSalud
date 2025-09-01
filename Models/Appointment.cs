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
    }
}