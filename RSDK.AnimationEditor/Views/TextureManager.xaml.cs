using AnimationEditor.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage.Pickers;
using WinRT;
using WinRT.Interop;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RSDK.AnimationEditor.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TextureManager : WindowEx
    {
        public string BasePath { get; private set; }
        public TextureWindowViewModel ViewModel => RootGrid.DataContext as TextureWindowViewModel;
        public MainViewModel MainViewModel { get; }

        public int SelectedIndex
        {
            get => ViewModel.SelectedIndex;
            set => ViewModel.SelectedIndex = value;
        }

        private AppWindow m_AppWindow;
        WindowsSystemDispatcherQueueHelper m_wsdqHelper;
        MicaController m_micaController;
        SystemBackdropConfiguration m_configurationSource;
        public TextureManager(MainViewModel mainViewModel)
        {
            InitializeComponent();
            MainViewModel = mainViewModel;
            RootGrid.DataContext = new TextureWindowViewModel(mainViewModel, BasePath);
        }

        public TextureManager(MainViewModel mainViewModel, string basePath)
        {
            InitializeComponent();

            BasePath = basePath;
            MainViewModel = mainViewModel;
            RootGrid.DataContext = new TextureWindowViewModel(mainViewModel, BasePath);

            m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();
            TrySetMicaBackdrop();
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

        #region Functionality
        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            //var dialog = FileDialog.Factory(this, FileDialog.Behavior.Open, FileDialog.Type.ImageGif);
            //if (dialog.ShowDialog() == true)
            //{
            //   var fileName = AddTextureToDirectory(dialog.FileName);
            //    if (fileName != null)
            //    {
            //        ViewModel.AddTexture(fileName);
            //        SelectedIndex = ViewModel.Count - 1;
            //    }
            // }

            FileOpenPicker Picker = new();
            Picker.ViewMode = PickerViewMode.Thumbnail;
            Picker.FileTypeFilter.Add(".gif");
            IntPtr hwnd = WindowNative.GetWindowHandle(this);
            InitializeWithWindow.Initialize(Picker, hwnd);
            Windows.Storage.StorageFile File = await Picker.PickSingleFileAsync();
            if (File != null)
            {
                try
                {
                    ViewModel.AddTexture(File.Path);
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                //No file selected, do nothing
            }

        }

        private async void Remove_Click(object sender, RoutedEventArgs e)
        {
            /*const string message = "Do you want to delete the file? If you click No, it will be deleted only from this application.";

            var index = SelectedIndex;
            switch (MessageBox.Show(message, "Delete confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning))
            {
                case MessageBoxResult.Yes:
                    ViewModel.RemoveTexture(SelectedIndex, true);
                    break;
                case MessageBoxResult.No:
                    ViewModel.RemoveTexture(SelectedIndex, false);
                    break;
                default:
                    // Do nothing.
                    index = -1;
                    break;
            }
            if (index >= 0)
            {
                if (index >= ViewModel.Count)
                    index--;
                SelectedIndex = index;
            }*/
            ContentDialog Dialog = new ContentDialog();

            Dialog.XamlRoot = RootGrid.XamlRoot;
            Dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog.Title = "Remove file?";
            Dialog.Content = $"{ViewModel.SelectedValue} will be removed from the list. This does not delete the file.";
            Dialog.PrimaryButtonText = "Remove";
            Dialog.CloseButtonText = "Cancel";
            var result = await Dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                ViewModel.RemoveTexture(SelectedIndex, false);
                //UnloadObject(Dialog);
            }
        }

        #endregion

        #region

        bool TrySetMicaBackdrop()
        {
            if (Microsoft.UI.Composition.SystemBackdrops.MicaController.IsSupported())
            {
                m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
                m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

                // Hooking up the policy object
                m_configurationSource = new Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration();
                Activated += Window_Activated;
                Closed += Window_Closed;
                ((FrameworkElement)Content).ActualThemeChanged += Window_ThemeChanged;

                // Initial configuration state.
                m_configurationSource.IsInputActive = true;
                SetConfigurationSourceTheme();

                m_micaController = new Microsoft.UI.Composition.SystemBackdrops.MicaController();

                // Enable the system backdrop.
                // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
                m_micaController.AddSystemBackdropTarget(this.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
                m_micaController.SetSystemBackdropConfiguration(m_configurationSource);
                return true; // succeeded
            }

            return false; // Mica is not supported on this system
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            m_configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
        }

        private void Window_Closed(object sender, WindowEventArgs args)
        {
            // Make sure any Mica/Acrylic controller is disposed so it doesn't try to
            // use this closed window.
            if (m_micaController != null)
            {
                m_micaController.Dispose();
                m_micaController = null;
            }
            Activated -= Window_Activated;
            m_configurationSource = null;
        }

        private void Window_ThemeChanged(FrameworkElement sender, object args)
        {
            if (m_configurationSource != null)
            {
                SetConfigurationSourceTheme();
            }
        }

        private void SetConfigurationSourceTheme()
        {
            switch (((FrameworkElement)Content).ActualTheme)
            {
                case ElementTheme.Dark: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Dark; break;
                case ElementTheme.Light: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Light; break;
                case ElementTheme.Default: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Default; break;
            }
        }

        #endregion

    }
}
