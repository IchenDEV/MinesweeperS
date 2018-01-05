using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Core;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace MineS
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ThemeSet : Page, INotifyPropertyChanged
    {
        private Themes _localtheme = LocalTheme.Local;

        public Themes localtheme
        {
            get { return this._localtheme; }
            set
            {
                this._localtheme = value;
                this.OnPropertyChanged();
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;



            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty. 
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty. 
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public async void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            try
            {
                LocalTheme.Local = localtheme;
                await LocalTheme.write(LocalTheme.TojsonData(localtheme));
            }
            catch { }

        }
        public ThemeSet()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            this.InitializeComponent();
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fileOpen = new FileOpenPicker();
            fileOpen.FileTypeFilter.Add(".png");
            var file = await fileOpen.PickSingleFileAsync();
            var dest = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Theme", CreationCollisionOption.OpenIfExists);
            var fli = await file.CopyAsync(dest, file.Name, NameCollisionOption.ReplaceExisting);
            LocalTheme.Local.BackIMage[0]=(fli.Path);
            localtheme = LocalTheme.Local;
            try
            {
                await LocalTheme.write(LocalTheme.TojsonData(localtheme));
            }
            catch
            {

            }

        }



        private async void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            try
            {
                LocalTheme.Local.IsHighMode = (sender as ToggleSwitch).IsOn;
                localtheme = LocalTheme.Local;
                await LocalTheme.write(LocalTheme.TojsonData(localtheme));
            }
            catch { }


        }


        private async void cp_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            try
            {
                LocalTheme.Local.mineColor = args.NewColor.ToString();
                localtheme = LocalTheme.Local;
                await LocalTheme.write(LocalTheme.TojsonData(localtheme));
            }
            catch
            {

            }

            rect.Fill = new SolidColorBrush(args.NewColor);
        }

        private async void ColorAuto_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                LocalTheme.Local.mineColor = "transparent";
                localtheme = LocalTheme.Local;
                await LocalTheme.write(LocalTheme.TojsonData(localtheme));
            }
            catch { }
        }
    }
}
