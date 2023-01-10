using Microsoft.UI.Windowing;
using Microsoft.UI;
using System.Threading.Tasks;
using WinRT;
using WinRT.Interop;

namespace RSDK.AnimationEditor.Content.Views.Other
{
    public partial class MainWindow : WinUIEx.WindowEx
    {
        public static MainWindow XamlWindow { get; set; }
        WindowsSystemDispatcherQueueHelper m_wsdqHelper;
        Microsoft.UI.Composition.SystemBackdrops.MicaController m_micaController;
        Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration m_configurationSource;

        public MainWindow()
        {
            InitializeComponent();
            XamlWindow = this;

            var manager = WinUIEx.WindowManager.Get(this);
            AppWindow appWindow = GetAppWindowForCurrentWindow();

            manager.PersistenceId = "MWPersistance";
            manager.MinWidth = 500;
            manager.MinHeight = 350;

            appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            appWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            appWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            TrySetMicaBackdrop();
        }

        #region

        private AppWindow GetAppWindowForCurrentWindow()
        {
            return AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(WindowNative.GetWindowHandle(this)));
        }
        
        public bool TrySetMicaBackdrop()
        {
            if (!Microsoft.UI.Composition.SystemBackdrops.MicaController.IsSupported()) return false;

            m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

            m_configurationSource = new Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration();
            m_configurationSource.IsInputActive = true;
            SetConfigurationSourceTheme();

            m_micaController = new Microsoft.UI.Composition.SystemBackdrops.MicaController();
            m_micaController.AddSystemBackdropTarget(this.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
            m_micaController.SetSystemBackdropConfiguration(m_configurationSource);

            Activated += Window_Activated;
            Closed += Window_Closed;
            ((Microsoft.UI.Xaml.FrameworkElement)Content).ActualThemeChanged += Window_ThemeChanged;

            return true;
        }


        private void Window_Activated(object sender, Microsoft.UI.Xaml.WindowActivatedEventArgs args)
        {
            m_configurationSource.IsInputActive = args.WindowActivationState != Microsoft.UI.Xaml.WindowActivationState.Deactivated;
        }

        private void Window_Closed(object sender, Microsoft.UI.Xaml.WindowEventArgs args)
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

        private void Window_ThemeChanged(Microsoft.UI.Xaml.FrameworkElement sender, object args)
        {
            if (m_configurationSource != null)
            {
                SetConfigurationSourceTheme();
            }
        }
        private void SetConfigurationSourceTheme()
        {
            switch (((Microsoft.UI.Xaml.FrameworkElement)Content).ActualTheme)
            {
                case Microsoft.UI.Xaml.ElementTheme.Dark: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Dark; break;
                case Microsoft.UI.Xaml.ElementTheme.Light: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Light; break;
                case Microsoft.UI.Xaml.ElementTheme.Default: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Default; break;
            }
        }

        #endregion


    }
}
