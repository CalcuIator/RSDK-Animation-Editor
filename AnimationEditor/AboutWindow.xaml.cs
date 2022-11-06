using System.Windows;

namespace AnimationEditor
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public string AppVersion
        {
            get
            {
                var version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
                return $"{version.Major}.{version.Minor}.{version.Build}";
            }
        }

        public AboutWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
