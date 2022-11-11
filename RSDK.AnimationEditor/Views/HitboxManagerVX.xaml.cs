using AnimationEditor.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RSDK.AnimationEditor.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HitboxManagerVX : Microsoft.UI.Xaml.Window
    {
        public HitboxV3EditorViewModel ViewModel => RootGrid.DataContext as HitboxV3EditorViewModel;
        private AppWindow m_AppWindow;
        public HitboxManagerVX(MainViewModel vm)
        {
            InitializeComponent();
            RootGrid.DataContext = new HitboxV3EditorViewModel(vm);
            m_AppWindow = GetAppWindowForCurrentWindow();
            var titleBar = m_AppWindow.TitleBar;
            titleBar.ExtendsContentIntoTitleBar = true;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }
        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
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
