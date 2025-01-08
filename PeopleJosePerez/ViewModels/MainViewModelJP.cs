using PeopleJosePerez.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PeopleJosePerez.ViewModels
{
    public class MainViewModelJP : INotifyPropertyChanged
    {
        private string _newPersonName;
        private string _statusMessage;

        public string NewPersonName
        {
            get => _newPersonName;
            set => SetProperty(ref _newPersonName, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public ObservableCollection<PersonJP> People { get; }
        public ICommand AddPersonCommand { get; }
        public ICommand GetPeopleCommand { get; }
        public ICommand DeletePersonCommand { get; }

        public MainViewModelJP()
        {
            People = new ObservableCollection<PersonJP>();
            AddPersonCommand = new Command(AddPerson);
            GetPeopleCommand = new Command(GetPeople);
            DeletePersonCommand = new Command<int>(DeletePerson);
        }

        private void AddPerson()
        {
            if (string.IsNullOrWhiteSpace(NewPersonName))
            {
                StatusMessage = "Por favor, ingresa un nombre válido.";
                return;
            }

            App.PersonRepo.AddNewPerson(NewPersonName);
            StatusMessage = App.PersonRepo.StatusMessage;
            NewPersonName = string.Empty;
            GetPeople();
        }

        private void GetPeople()
        {
            try
            {
                People.Clear();
                foreach (var person in App.PersonRepo.GetAllPeople())
                {
                    People.Add(person);
                }
                StatusMessage = "Se han guardado las personas que ingresó.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al cargar personas: {ex.Message}";
            }
        }

        private async void DeletePerson(int id)
        {
            App.PersonRepo.DeletePerson(id);
            StatusMessage = App.PersonRepo.StatusMessage;

            // Mostrar mensaje personalizado
            if (App.PersonRepo.StatusMessage.Contains("eliminado"))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Registro Eliminado",
                    "Jose Perez acaba de eliminar un registro.",
                    "OK");
            }

            GetPeople();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
