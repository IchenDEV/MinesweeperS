
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace MineS
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SetPage : Page, INotifyPropertyChanged

    {
       private UISettings uisetting = new UISettings();
        public SolidColorBrush ForeColor = new SolidColorBrush(Colors.Black);
        public SetPage()
        {

            //uisetting.ColorValuesChanged += Uisetting_ColorValuesChanged;
            this.InitializeComponent();
            //if (SystemInformation.DeviceFamily == "Windows.Mobile")
            //{
            //    Sett.IsEnabled = false;
            //}

        }

        private void Uisetting_ColorValuesChanged(UISettings sender, object args)
        {
            try
            {
 ForeColor = new SolidColorBrush ( sender.GetColorValue(UIColorType.Accent));
            }
            catch (Exception)
            {

               
            }
           
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            band =
          ((Frame)e.Parameter);

            EnterStoryboard.Begin();
        }
        public Frame band;
        private int _MapHeigh = 0;
        private int _MapWidth = 0;
        private int _NumMine = 0;
        private string _Mode;
        private bool _isCustom = false;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public int MapHeigh
        {
            get { return this._MapHeigh; }
            set
            {
                this._MapHeigh = value;
                this.OnPropertyChanged();
            }
        }
        public int MapWidth
        {
            get { return this._MapWidth; }
            set
            {
                this._MapWidth = value;
                this.OnPropertyChanged();
            }
        }
        public int NumMine
        {
            get { return this._NumMine; }
            set
            {
                this._NumMine = value;
                this.OnPropertyChanged();
            }
        }
        public bool isCustom
        {
            get { return this._isCustom; }
            set
            {
                this._isCustom = value;
                this.OnPropertyChanged();
            }
        }
        public string Mode
        {
            get { return this._Mode; }
            set
            {
                this._Mode = value;
                this.OnPropertyChanged();
            }
        }



        void goToPlay()
        {
            MapData md = new MapData() { w = MapWidth, h = MapHeigh, mine = NumMine, mode = Mode, local = band };
            band.Navigate(typeof(Map), md);
        }
        private void Easy_Click(object sender, RoutedEventArgs e)
        {
            isCustom = false;
            MapHeigh = 10; MapWidth = 10; NumMine = 9; Mode = "easy";
            goToPlay();
        }

        private void Mid_Click(object sender, RoutedEventArgs e)
        {
            isCustom = false;
            MapHeigh = 20; MapWidth = 15; NumMine = 30; Mode = "mid";
            goToPlay();
        }

        private void Hatd_Click(object sender, RoutedEventArgs e)
        {
            isCustom = false;
            MapHeigh = 16; MapWidth = 30; NumMine = 99; Mode = "hard";
            goToPlay();
        }

        private void Custom_Click(object sender, RoutedEventArgs e)
        {
            isCustom = true; Mode = "Custom";
            //  EnterStoryboard.Begin();
            // tl.IsOpen = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Cur.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            rootFrame.Navigate(typeof(ThemeSet));

        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

            if (MapHeigh * MapWidth > 2 * NumMine)
            {
                goToPlay();
            }
            else
            {
                int duration = 2000;
                ExampleInAppNotification.Show("Wrong input.", duration);
            }

        }

        private void super_Loaded(object sender, RoutedEventArgs e)
        {
            EnterStoryboard.Begin();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            rootFrame.Navigate(typeof(Achievement));

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            rootFrame.Navigate(typeof(About));
        }
    }

}
