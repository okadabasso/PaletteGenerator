using Prism.Mvvm;

namespace PaletteGenerator.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Palette Generator";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
