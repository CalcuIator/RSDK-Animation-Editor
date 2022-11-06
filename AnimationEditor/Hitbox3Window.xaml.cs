using AnimationEditor.ViewModels;
using System.Windows;

namespace AnimationEditor
{
    /// <summary>
    /// Interaction logic for Hitbox3Window.xaml
    /// </summary>
    public partial class Hitbox3Window : Window
    {
        public HitboxV3EditorViewModel ViewModel => DataContext as HitboxV3EditorViewModel;

        public Hitbox3Window(MainViewModel vm)
        {
            InitializeComponent();
            DataContext = new HitboxV3EditorViewModel(vm);
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddHitboxEntry();
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveHitboxEntry();
        }
    }
}
