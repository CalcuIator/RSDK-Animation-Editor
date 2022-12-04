using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RSDK.AnimationEditor.Content.Views;
using RSDK.AnimationEditor.Content.Views.Other;
using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;


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
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            MainWindow window = new();
            window.Activate();

            window.DispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Low, () =>
            {
                Frame rootFrame = window.Content as Frame;

                if (rootFrame == null)
                {
                    // Create a Frame to act as the navigation context and navigate to the first page
                    rootFrame = new Frame();
                    //rootFrame.Background = (SolidColorBrush)App.Current.Resources["ApplicationPageBackgroundThemeBrush"];
                    window.Content = rootFrame;
                }
                if (rootFrame.Content == null)
                {

                    rootFrame.Navigate(typeof(MainPage));
                    MainWindow.XamlWindow.TrySetMicaBackdrop();

                }
            });

            await AddJumpListItems();
        }
        private async Task AddJumpListItems()
        {
            var isThemeDark = Application.Current.RequestedTheme == ApplicationTheme.Dark;
            JumpList list = await JumpList.LoadCurrentAsync();
            list.Items.Clear();
            var item = JumpListItem.CreateWithArguments("LibraryJumpList", "Library");
            item.Logo = isThemeDark ? new Uri("ms-appx:///Assets/LibraryDark.png") : new Uri("ms-appx:///Assets/LibraryLight.png");
            list.Items.Add(item);
            await list.SaveAsync();
        }
    }
}