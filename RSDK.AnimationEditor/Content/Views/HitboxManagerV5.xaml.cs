using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RSDK.AnimationEditor.Content.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RSDK.AnimationEditor.Content.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HitboxManagerV5 : Window
    {
        public HitboxV5EditorViewModel ViewModel => RootGrid.DataContext as HitboxV5EditorViewModel;
        public HitboxManagerV5(MainViewModel vm)
        {
            InitializeComponent();
            RootGrid.DataContext = new HitboxV5EditorViewModel(vm);
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.HitboxTypeItems.Add("EMPTY");
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsValueSelected)
                ViewModel.HitboxTypeItems.RemoveAt(ViewModel.SelectedIndex);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                ViewModel.UpdateList(textBox.Text);
            }
        }
    }
}
