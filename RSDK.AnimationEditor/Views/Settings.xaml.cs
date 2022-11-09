using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RSDK.AnimationEditor.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
            Window.XamlWindow.SetTitleBar(DragRegion);

            var Version = Package.Current.Id.Version;
            AppString.Text = $"RSDK Animation Editor {string.Format("{0}.{1}.{2}", Version.Major, Version.Minor, Version.Build)}";
        }

        private void _SizeChanged(object sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
        {

            //I know this is terrible, don't blame pls ;-;
            //I'll find a way to define this in xaml eventually

            if (ActualWidth > 960)
            {
                //Fully expanded
                Grid.SetRow(AboutContainer, 0);
                Grid.SetColumn(AboutContainer, 1);
                AboutContainer.Margin = new Thickness(0, 8, 0, 8);
                ContentContainer.ColumnSpacing = 56;
            }
            else if (ActualWidth < 960)
            {
                if (ActualWidth != 645)
                {
                    //Medium state
                    Grid.SetRow(AboutContainer, 1);
                    Grid.SetColumn(AboutContainer, 0);
                    AboutContainer.Margin = new Thickness(0, 24, 0, 0);
                    ContentContainer.ColumnSpacing = 0;
                }
                if (ActualWidth > 645)
                {
                    //Check if the state is compact before reverting changes
                    //we'll just use the FontSize :)
                    if (SettingHeader.FontSize == 28)
                    {
                        RootContainer.Margin = new Thickness(0, 28, 0, 0);
                        ContentContainer.Margin = new Thickness(52, 24, 52, 24);
                        SettingHeader.Margin = new Thickness(52, 0, 0, 0);
                        SettingHeader.FontSize = 40;
                    }
                }
                if (ActualWidth < 645)
                {
                    //Compact state
                    RootContainer.Margin = new Thickness(0, 16, 0, 0);
                    ContentContainer.Margin = new Thickness(16);
                    SettingHeader.Margin = new Thickness(16, 0, 0, 0);
                    SettingHeader.FontSize = 28;
                }
            }

        }

        private void Back_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            Frame.GoBack();
            //Page is cached, title bar has to be set again
            Window.XamlWindow.SetTitleBar(MainPage.MainTitleBar);
            UnloadObject(this);
        }


    }
}
