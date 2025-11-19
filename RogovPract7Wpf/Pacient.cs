using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RogovPract7Wpf
{
    public class Pacient : INotifyPropertyChanged
    {
        private int _id = 0;
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }
    
        private string _name = "";
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }
    
        private string _surname = "";
        public string Surname
        {
            get => _surname;
            set { _surname = value; OnPropertyChanged(); }
        }
    
        private string _patronimic = "";
        public string Patronimic
        {
            get => _patronimic;
            set { _patronimic = value; OnPropertyChanged(); }
        }
    
        private DateTime _birthday = new DateTime(1920, 01, 01);
        public DateTime Birthday
        {
            get => _birthday;
            set { _birthday = value; OnPropertyChanged(); }
        }
    
        private DateTime _lastAppointment = DateTime.Today;
        public DateTime LastAppointment
        {
            get => _lastAppointment;
            set { _lastAppointment = value; OnPropertyChanged(); }
        }
    
        private int _lastDoctor = 0;
        public int LastDoctor
        {
            get => _lastDoctor;
            set { _lastDoctor = value; OnPropertyChanged(); }
        }
    
        private string _diagnosis = "";
        public string Diagnosis
        {
            get => _diagnosis;
            set { _diagnosis = value; OnPropertyChanged(); }
        }
    
        private string _recomendations = "";
        public string Recomendations
        {
            get => _recomendations;
            set { _recomendations = value; OnPropertyChanged(); }
        }
    
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    
        public Dictionary<int, Pacient> pacients = new Dictionary<int, Pacient>();
        public Random rnd = new Random();
    
        public void LoadPacients()
        {
            string pacientDir = "Pacients";
            if (!Directory.Exists(pacientDir))
                return;
    
            string[] pacientFiles = Directory.GetFiles(pacientDir, "P_*.json");
            foreach (string file in pacientFiles)
            {
                var jsonString = File.ReadAllText(file);
                Pacient pacient = JsonSerializer.Deserialize<Pacient>(jsonString);
                pacients[pacient.Id] = pacient;
            }
        }
    
        public void SearchPacient()
        {
            if (pacients.ContainsKey(Id))
            {
                var foundPacient = pacients[Id];
                Name = foundPacient.Name;
                Surname = foundPacient.Surname;
                Patronimic = foundPacient.Patronimic;
                Birthday = foundPacient.Birthday;
                LastAppointment = foundPacient.LastAppointment;
                LastDoctor = foundPacient.LastDoctor;
                Diagnosis = foundPacient.Diagnosis;
                Recomendations = foundPacient.Recomendations;
            }
            else
                throw new ArgumentException("Пациент с таким ID не найден");
        }

        public void SaveChanges()
        {
            if (!pacients.ContainsKey(Id))
                throw new ArgumentException("Пациент с таким ID не найден");

            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Surname))
                throw new ArgumentException("Имя и фамилия обязательны");

            var pacientToUpdate = pacients[Id];
            pacientToUpdate.Name = Name;
            pacientToUpdate.Surname = Surname;
            pacientToUpdate.Patronimic = Patronimic;
            pacientToUpdate.Birthday = Birthday;
            pacientToUpdate.LastAppointment = LastAppointment;
            pacientToUpdate.LastDoctor = LastDoctor;
            pacientToUpdate.Diagnosis = Diagnosis;
            pacientToUpdate.Recomendations = Recomendations;

            var jsonString = JsonSerializer.Serialize(pacientToUpdate);
            var path = Path.Combine("Pacients", $"P_{Id}.json");
            File.WriteAllText(path, jsonString, Encoding.UTF8);
        }

        public void Reset()
        {
            Id = 0;
            Name = Surname = Patronimic = Diagnosis = Recomendations = "";
            Birthday = LastAppointment = DateTime.MinValue;
        }
    }
}
