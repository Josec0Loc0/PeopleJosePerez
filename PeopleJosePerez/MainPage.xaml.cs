using PeopleJosePerez.ViewModels;

namespace PeopleJosePerez
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainViewModelJP();
        }
    }
}