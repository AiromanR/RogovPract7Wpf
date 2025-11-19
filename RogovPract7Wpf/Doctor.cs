using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace RogovPract7Wpf
{
    public class Doctor : INotifyPropertyChanged
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

        private string _specialization = "";
        public string Specialization
        {
            get => _specialization;
            set { _specialization = value; OnPropertyChanged(); }
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        private string _confirmPassword = "";
        [JsonIgnore]
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Doctor() { }
        private Dictionary<int, string> ids = new Dictionary<int, string>();
        public Random rnd = new Random();

        public void LoadDoctors()
        {
            string doctorDir = "Doctors";
            if (!Directory.Exists(doctorDir))
                return;

            string[] doctorFiles = Directory.GetFiles(doctorDir, "D_*.json");
            foreach (string file in doctorFiles)
            {
                string jsonString = File.ReadAllText(file);
                Doctor doctor = JsonSerializer.Deserialize<Doctor>(jsonString);
                ids[doctor.Id] = doctor.Password;
            }
        }

        public void Registration(string name, string surname, string patronimic, string specialization, string password, string confPass)
        {
            if (name == "" || surname == "" || patronimic == "" || specialization == "" || password == "" || confPass == "")
                throw new ArgumentException("Все поля должны быть заполнены");

            if (password != confPass)
                throw new ArgumentException("Пароли не совпадают");

            int id;
            do { id = rnd.Next(10000, 100000); }
            while (ids.ContainsKey(id));
            Id = id;
            Name = name;
            Surname = surname;
            Patronimic = patronimic;
            Specialization = specialization;
            Password = password;
            ids[id] = password;
            var jsonString = JsonSerializer.Serialize(this);
            var path = Path.Combine("Doctors", $"D_{Id}.json");
            File.WriteAllText(path, jsonString, Encoding.UTF8);
        }

        public void Login(string id, string password)
        {
            if (id == "0" && password == "")
                throw new ArgumentException("Все поля должны быть заполнены");

            var ID = int.Parse(id);
            if (!ids.ContainsKey(ID) || ids[ID] != password)
                throw new ArgumentException("Неверный логин или пароль");


            var path = $"Doctors\\D_{ID}.json";
            if (!File.Exists(path))
                throw new FileNotFoundException("Пользователь не найден");

            var jsonString = File.ReadAllText(path);
            Doctor doctor = JsonSerializer.Deserialize<Doctor>(jsonString);

            this.Id = doctor.Id;
            this.Name = doctor.Name;
            this.Surname = doctor.Surname;
            this.Patronimic = doctor.Patronimic;
            this.Specialization = doctor.Specialization;
            this.Password = doctor.Password;
        }

        public Pacient AddPacient(string name, string surname, string patronimic, DateTime bd, DateTime lastAppointment, int lastDoc, string diagnosis, string recomendations)
        {
            if (name == "" || surname == "" || patronimic == "" || diagnosis == "" || recomendations == "")
                throw new ArgumentException("Все поля должны быть заполнены");

            if (bd > lastAppointment)
                throw new ArgumentException("Дата рождения не должна быть позже последнего визита");

            Pacient pacient = new Pacient();
            int id;
            do { id = rnd.Next(1000000, 10000000); }
            while (ids.ContainsKey(id));
            pacient.Id = id;
            pacient.Name = name;
            pacient.Surname = surname;
            pacient.Patronimic = patronimic;
            pacient.Birthday = bd;
            pacient.LastDoctor = lastDoc;
            pacient.LastAppointment = lastAppointment;
            pacient.Diagnosis = diagnosis;
            pacient.Recomendations = recomendations;
            pacient.pacients[id] = pacient;

            var jsonString = JsonSerializer.Serialize(pacient);
            var path = Path.Combine("pacients", $"P_{pacient.Id}.json");
            File.WriteAllText(path, jsonString, Encoding.UTF8);
            return pacient;
        }
    }
}
