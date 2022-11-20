using AnimationEditor.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage.Pickers;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RSDK.AnimationEditor.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TextureManager : Page
    {
        public string BasePath { get; private set; }
        public TextureWindowViewModel ViewModel => DataContext as TextureWindowViewModel;
        public MainViewModel MainViewModel { get; }

        public int SelectedIndex
        {
            get => ViewModel.SelectedIndex;
            set => ViewModel.SelectedIndex = value;
        }

        public TextureManager(MainViewModel mainViewModel)
        {
            InitializeComponent();
            MainViewModel = mainViewModel;
            DataContext = new TextureWindowViewModel(mainViewModel, BasePath);
        }

        public TextureManager(MainViewModel mainViewModel, string basePath)
        {
            InitializeComponent();

            BasePath = basePath;
            MainViewModel = mainViewModel;
            DataContext = new TextureWindowViewModel(mainViewModel, BasePath);
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
            IntPtr hwnd = WindowNative.GetWindowHandle(MainWindow.XamlWindow);
            InitializeWithWindow.Initialize(Picker, hwnd);
            Windows.Storage.StorageFile File = await Picker.PickSingleFileAsync();
            if (File != null)
            {
                try
                {
                    ViewModel.AddTexture(File.Path);
                    SelectedIndex = ViewModel.Count - 1;
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

            /*
            ContentDialog Dialog = new ContentDialog();

            Dialog.XamlRoot = MainWindow.XamlWindow.Content.XamlRoot;
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
            */

            //ConfirmDelete.IsOpen = true;


        }

        #endregion


    }
}
