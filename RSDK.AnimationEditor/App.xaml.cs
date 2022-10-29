using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using RSDK.AnimationEditor.Views;
using System;
using System.Threading.Tasks;

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
        public static Microsoft.UI.Xaml.Window acwindow { get; set; }
        public App()
        {
            this.InitializeComponent();
            Windows.ApplicationModel.Core.CoreApplication.EnablePrelaunch(true);
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected async override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            /* 







             window.Content = new SplashContent();

             //Frame rootFrame = window.Content as Frame;


             */


            Views.Window window = new Views.Window();
            window.ExtendsContentIntoTitleBar = true;
            Frame rootFrame = window.Content as Frame;



            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;
                window.Content = rootFrame;
            }
            if (rootFrame.Content == null)
            {
                //Temporary, staying until I find a way to
                //lower load times
                rootFrame.Navigate(typeof(SplashContent));
                window.Activate();


                await Task.Delay(100);
                rootFrame.Navigate(typeof(Views.MainPage));




                /*var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                Microsoft.UI.WindowId W32WindowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
                AppWindow Win32Window = AppWindow.GetFromWindowId(W32WindowId);
                DisplayArea displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(W32WindowId, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);

                var StartPos = Win32Window.Position;
                StartPos.X = ((displayArea.WorkArea.Width - Win32Window.Size.Width) / 2);
                StartPos.Y = ((displayArea.WorkArea.Height - Win32Window.Size.Height) / 2);
                Win32Window.Move(StartPos);*/

            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
    }
}