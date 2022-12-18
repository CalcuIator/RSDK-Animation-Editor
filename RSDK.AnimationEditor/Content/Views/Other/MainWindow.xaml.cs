using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using WinRT;

namespace RSDK.AnimationEditor.Content.Views.Other
{
    public partial class MainWindow : WinUIEx.WindowEx
    {
        public static MainWindow XamlWindow { get; set; }
        WindowsSystemDispatcherQueueHelper m_wsdqHelper;
        MicaController m_micaController;
        SystemBackdropConfiguration m_configurationSource;

        public MainWindow()
        {
            InitializeComponent();
            TrySetMicaBackdrop();
            XamlWindow = this;

            var manager = WinUIEx.WindowManager.Get(this);
            manager.PersistenceId = "MWPersistance";
            manager.MinWidth = 500;
            manager.MinHeight = 350;

            ExtendsContentIntoTitleBar = true;
        }


        /*
        private void UpdateCaptionColors()
        {
            var isThemeDark = Application.Current.RequestedTheme == ApplicationTheme.Dark;

            appWindow.TitleBar.ButtonHoverBackgroundColor = isThemeDark ? Color.FromArgb(15, 255, 255, 255) : Color.FromArgb(10, 0, 0, 0);
            appWindow.TitleBar.ButtonPressedBackgroundColor = isThemeDark ? Color.FromArgb(10, 255, 255, 255) : Color.FromArgb(6, 0, 0, 0);
        }
        */

        private void Window_ThemeChanged(FrameworkElement sender, object args)
        {
            if (m_configurationSource != null)
            {
                SetConfigurationSourceTheme();
            }
            //UpdateCaptionColors();
        }
        public bool TrySetMicaBackdrop()
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

        private void SetConfigurationSourceTheme()
        {
            switch (((FrameworkElement)Content).ActualTheme)
            {
                case ElementTheme.Dark: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Dark; break;
                case ElementTheme.Light: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Light; break;
                case ElementTheme.Default: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Default; break;
            }
        }

    }
}
