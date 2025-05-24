using System.Windows;

using UroborosApp.Model;

namespace UroborosApp.Utils
{
    public partial class EditMaterialWindow : Window
    {
        public Material Material { get; private set; }

        public EditMaterialWindow(Material material)
        {
            InitializeComponent();
            Material = material;
            DataContext = Material; 
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}