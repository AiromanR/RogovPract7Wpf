using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RogovPract7Wpf
{
    public partial class MainWindow : Window
    {
        public Doctor doctor;
        public Pacient pacientAdd;
        public Pacient pacientWork;
        public Status sys;
        public MainWindow()
        {
            Directory.CreateDirectory("Doctors");
            Directory.CreateDirectory("Pacients");

            doctor = new Doctor();
            pacientAdd = new Pacient();
            pacientWork = new Pacient();
            sys = new Status();

            InitializeComponent();
            AddPacient.Visibility = EditPacient.Visibility = SavePatient.Visibility = SearchPatient.Visibility = DocInfo.Visibility = Visibility.Collapsed;

            DataContext = doctor;
            DocInfo.DataContext = null;
            AddPacient.DataContext = pacientAdd;
            EditPacient.DataContext = SearchPatient.DataContext = SavePatient.DataContext = pacientWork;
            SysStatus.DataContext = sys;

            pacientWork.Reset();
            doctor.LoadDoctors();
            pacientAdd.LoadPacients();
            pacientWork.LoadPacients();
            sys.UpdateCounts();
        }
        private void ClearTextBoxes(Grid panel)
        {
            foreach (TextBox textBox in panel.Children.OfType<TextBox>())
                textBox.Clear();
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                doctor.Registration(doctor.Name, doctor.Surname, doctor.Patronimic, doctor.Specialization, doctor.Password, doctor.ConfirmPassword);
                MessageBox.Show($"Вы зарегистрированы. Ваш ID = {doctor.Id}", "Успешно");
                ClearTextBoxes(Reg);
                sys.UpdateCounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                doctor.Login(doctor.Id.ToString(), doctor.Password);
                ClearTextBoxes(Reg);
                ClearTextBoxes(Log);
                DocInfo.DataContext = doctor;
                AddPacient.Visibility = EditPacient.Visibility = SavePatient.Visibility = DocInfo.Visibility = SearchPatient.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AddPacient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newPac = doctor.AddPacient(pacientAdd.Name, pacientAdd.Surname, pacientAdd.Patronimic,
                                 pacientAdd.Birthday, pacientAdd.LastAppointment, doctor.Id,
                                 "", "");
                MessageBox.Show($"Пациент добавлен. ID: {newPac.Id}", "Успешно");

                pacientAdd.pacients[newPac.Id] = newPac;
                pacientWork.pacients[newPac.Id] = newPac;
                pacientAdd.Reset();
                sys.UpdateCounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ResetPacient_Click(object sender, RoutedEventArgs e)
        {
            pacientWork.Reset();
        }
        private void SaveChangesPacient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                pacientWork.SaveChanges();
                MessageBox.Show("Изменения сохранены", "Успешно");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SearchPacient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                pacientWork.SearchPacient();
                MessageBox.Show("Пациент найден", "Успешно");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}