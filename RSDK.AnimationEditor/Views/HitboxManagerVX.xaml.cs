using AnimationEditor.ViewModels;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RSDK.AnimationEditor.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HitboxManagerVX : Window
    {
        public HitboxV3EditorViewModel ViewModel => RootGrid.DataContext as HitboxV3EditorViewModel;
        WindowsSystemDispatcherQueueHelper m_wsdqHelper;
        MicaController m_micaController;
        SystemBackdropConfiguration m_configurationSource;
        public HitboxManagerVX(MainViewModel vm)
        {
            InitializeComponent();
            RootGrid.DataContext = new HitboxV3EditorViewModel(vm);
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
        }

        private void AddHitbox(object sender, RoutedEventArgs e)
        {
            ViewModel.AddHitboxEntry();
        }

        private void RemoveHitbox(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveHitboxEntry();
        }
    }
}
