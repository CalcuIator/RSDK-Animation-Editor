using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using RSDK.AnimationEditor.Content.ViewModels;
using RSDK.AnimationEditor.Content.Views.Other;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RSDK.AnimationEditor.Content.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private MainViewModel ViewModel => (MainViewModel)DataContext;
        public static Grid MDragRegion { get; set; }
        bool hasLoaded = false;
        public MainPage()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            MainWindow.XamlWindow.SetTitleBar(AppTitleBar);
            MDragRegion = AppTitleBar;
            MainWindow.XamlWindow.Activated += _Activated;
        }

        private void _Activated(object sender, WindowActivatedEventArgs args)
        {
            if (args.WindowActivationState == WindowActivationState.Deactivated)
            {
                AppTitleTextBlock.Foreground =
                    (SolidColorBrush)App.Current.Resources["WindowCaptionForegroundDisabled"];
            }
            else
            {
                AppTitleTextBlock.Foreground =
                    (SolidColorBrush)App.Current.Resources["WindowCaptionForeground"];
            }
        }

        #region Menubar

        private async void FileOpen_Click(object sender, RoutedEventArgs e)
        {

            //var fd = Xe.Tools.Wpf.Dialogs.FileDialog.Factory(this,
            //    Xe.Tools.Wpf.Dialogs.FileDialog.Behavior.Open,
            //    Xe.Tools.Wpf.Dialogs.FileDialog.Type.Any, false);
            //if (fd.ShowDialog() == true)
            //{
            //    ViewModel.FileOpen(fd.FileName);
            //}
            var Picker = new FileOpenPicker();
            Picker.FileTypeFilter.Add(".ani");
            Picker.FileTypeFilter.Add(".bin");

            IntPtr hwnd = WindowNative.GetWindowHandle(MainWindow.XamlWindow);
            InitializeWithWindow.Initialize(Picker, hwnd);

            Windows.Storage.StorageFile File = await Picker.PickSingleFileAsync();

            if (File != null)
            {
                try
                {
                    //Load file
                    ViewModel.FileOpen(File.Path);

                    /*Title has to be updated from C# since
                    Window doesn't have a DataContext property*/

                    MainWindow.XamlWindow.Title = AppTitleTextBlock.Text;

                    if (hasLoaded == false)
                    {
                        DispatcherQueue.TryEnqueue(async () =>
                        {
                            await Task.Delay(16);
                            FindName("Column0");
                            await Task.Delay(16);
                            FindName("Column1");
                            await Task.Delay(16);
                            FindName("Column2");
                        });
                        hasLoaded = true;
                    }
                }
                catch (Exception ex)
                {
                    ContentDialog Dialog = new();

                    Dialog.XamlRoot = XamlRoot;
                    Dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                    Dialog.Title = "Something went wrong";
                    Dialog.Content = "There was a problem loading this file. " + ex.Message;
                    Dialog.PrimaryButtonText = "Close";
                    var result = await Dialog.ShowAsync();
                    if (result == ContentDialogResult.Primary)
                    {
                        OpenFromDialog();
                        //UnloadObject(Dialog);
                    }
                };
            }
            else
            {
                //No file selected, do nothing
            }
        }

        private void FileSave_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.FileSave();
        }

        /*private void FileSaveAs_Click(object sender, RoutedEventArgs e)
        {
            var fd = Xe.Tools.Wpf.Dialogs.FileDialog.Factory(this,
                Xe.Tools.Wpf.Dialogs.FileDialog.Behavior.Save,
                Xe.Tools.Wpf.Dialogs.FileDialog.Type.Any, false);
            if (fd.ShowDialog() == true)
            {
                ViewModel.FileSave(fd.FileName);
            }
        }*/

        private void FileExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.XamlWindow.Close();
        }
        private void ViewHitbox_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.isHitboxVX)
            {
                new HitboxManagerVX(ViewModel).Activate();
            }
            else if (ViewModel.IsHitboxV5)
            {
                new HitboxManagerV5(ViewModel).Activate();
            }
        }

        private async void ViewTexture_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsAnimationDataLoaded)
            {
                var basePath = Path.Combine(Path.GetDirectoryName(ViewModel.FileName), ViewModel.PathMod);
                new TextureManager(ViewModel, basePath).Activate();
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoForward)
            {
                Frame.GoForward();
            }
            else
            {
                Frame.Navigate(typeof(Settings));
            }
            MainWindow.XamlWindow.DispatcherQueue.TryEnqueue(() => MainWindow.XamlWindow.SetTitleBar(Settings.SDragRegion));
        }

        #endregion

        #region Column 2

        private void Column2Frame_Loaded(object sender, RoutedEventArgs e)
        {
            if (Column2Frame.Content == null)
            {
                DispatcherQueue.TryEnqueue(() => Column2Frame.Navigate(typeof(SpritePropertiesContent)));
            }
        }

        private void SegmentedColumn0_Clicked(object sender, RoutedEventArgs e)
        {
            Grid.SetColumn(SegmentedSelection, 0);
            Column2Frame.Navigate(typeof(SpritePropertiesContent));
        }

        private void SegmentedColumn1_Clicked(object sender, RoutedEventArgs e)
        {
            Grid.SetColumn(SegmentedSelection, 1);
            Column2Frame.Navigate(typeof(HitboxContent));
        }

        #endregion

        #region Misc

        private async void OpenFromDialog()
        {
            //var fd = Xe.Tools.Wpf.Dialogs.FileDialog.Factory(this,
            //    Xe.Tools.Wpf.Dialogs.FileDialog.Behavior.Open,
            //    Xe.Tools.Wpf.Dialogs.FileDialog.Type.Any, false);
            //if (fd.ShowDialog() == true)
            //{
            //    ViewModel.FileOpen(fd.FileName);
            //}

            FileOpenPicker Picker = new();
            Picker.FileTypeFilter.Add(".ani");
            Picker.FileTypeFilter.Add(".bin");
            IntPtr hwnd = WindowNative.GetWindowHandle(MainWindow.XamlWindow);
            InitializeWithWindow.Initialize(Picker, hwnd);
            Windows.Storage.StorageFile File = await Picker.PickSingleFileAsync();
            if (File != null)
            {
                try
                {
                    //Load file
                    ViewModel.FileOpen(File.Path);
                }
                catch (Exception ex)
                {
                    ContentDialog Dialog = new ContentDialog();

                    Dialog.XamlRoot = XamlRoot;
                    Dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                    Dialog.Title = "Something went wrong";
                    Dialog.Content = "There was a problem loading this file. " + ex.Message;
                    Dialog.PrimaryButtonText = "Close";
                    var result = await Dialog.ShowAsync();
                    if (result == ContentDialogResult.Primary)
                    {
                        OpenFromDialog();
                        UnloadObject(Dialog);
                    }
                };
            }
            else
            {
                try
                {
                    /*
                    Incase the user clicks cancel
                    on the file dialog, wait for
                    them to open a file
                    */
                    ViewModel.FileOpen(File.Path);
                }
                catch (Exception ex)
                {
                    ContentDialog Dialog = new ContentDialog();

                    Dialog.XamlRoot = XamlRoot;
                    Dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                    Dialog.Title = "Something went wrong";
                    Dialog.Content = "There was a problem loading this file. " + ex.Message;
                    Dialog.PrimaryButtonText = "Close";
                    var result = await Dialog.ShowAsync();
                    if (result == ContentDialogResult.Primary)
                    {
                        OpenFromDialog();
                        UnloadObject(Dialog);
                    }
                };
            }
        }

        #endregion

        private void AnimList_RightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            var flyout = FlyoutBase.GetAttachedFlyout((FrameworkElement)sender);
            var options = new FlyoutShowOptions()
            {
                Position = e.GetPosition((FrameworkElement)sender),
                ShowMode = FlyoutShowMode.Standard,
                Placement= FlyoutPlacementMode.BottomEdgeAlignedLeft
            };

            AnimationListContextMenu.ShowAt((FrameworkElement)sender, options);
        }

        private async void Rename_Click(object sender, RoutedEventArgs e)
        {
            RenameDialog Dialog = new();

            Dialog.XamlRoot = XamlRoot;
            var result = await Dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {

            }
        }
    }
}
