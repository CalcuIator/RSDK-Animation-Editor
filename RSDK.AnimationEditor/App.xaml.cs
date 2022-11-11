using Microsoft.UI.Xaml;
using RSDK.AnimationEditor.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RSDK.AnimationEditor
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Views.Window window = new Views.Window();
            window.Activate();

            Views.Window.RootFrame.Navigate(typeof(MainPage));
            //Views.Window.RootFrame.Navigate(typeof(UpdatePage));




            /*Window window = new Views.Window();
            Frame rootFrame = Window.Current.Content as Frame;
            //window.Activate();
            

            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                window.Content = rootFrame;

            }
            if (rootFrame.Content == null)
            {
                window.Activate();
                //rootFrame.Navigate(typeof(Views.MainPage));
            }*/
        }
    }
}