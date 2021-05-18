using System.Windows;
using Prism;
using Prism.Navigation;
using Prism.Regions;

namespace PaletteGenerator.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IRegionManager regonManager)
        {
            InitializeComponent();
            regonManager.RegisterViewWithRegion("ContentRegion", typeof(PaletteView));
        }
    }
}
