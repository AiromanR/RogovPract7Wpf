using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RogovPract7Wpf
{
    public class Status : INotifyPropertyChanged
    {
        private int _doctorsCount;
        private int _pacientsCount;

        public int DoctorsCount
        {
            get => _doctorsCount;
            set { _doctorsCount = value; OnPropertyChanged(); }
        }

        public int PacientsCount
        {
            get => _pacientsCount;
            set { _pacientsCount = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateCounts()
        {
            string doctorsDir = "Doctors";
            if (Directory.Exists(doctorsDir))
            {
                string[] doctorFiles = Directory.GetFiles(doctorsDir, "D_*.json");
                DoctorsCount = doctorFiles.Length;
            }
            else
                DoctorsCount = 0;

            string pacientsDir = "Pacients";
            if (Directory.Exists(pacientsDir))
            {
                string[] pacientFiles = Directory.GetFiles(pacientsDir, "P_*.json");
                PacientsCount = pacientFiles.Length;
            }
            else
                PacientsCount = 0;
        }
    }
}
