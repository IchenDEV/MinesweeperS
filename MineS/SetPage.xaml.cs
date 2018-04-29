
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
using Windows.Services.Store;
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

        private void Button_Click_0(object sender, RoutedEventArgs e)
        {
            PurchaseAddOn("9P44R6NSLH1P");
           
        }
        private StoreContext context = null;
        public async void PurchaseAddOn(string storeId)
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
                // If your app is a desktop app that uses the Desktop Bridge, you
                // may need additional code to configure the StoreContext object.
                // For more info, see https://aka.ms/storecontext-for-desktop.
            }

            workingProgressRing.IsActive = true;
            StorePurchaseResult result = await context.RequestPurchaseAsync(storeId);
            workingProgressRing.IsActive = false;

            // Capture the error message for the operation, if any.
            string extendedError = string.Empty;
            if (result.ExtendedError != null)
            {
                extendedError = result.ExtendedError.Message;
            }

            switch (result.Status)
            {
                case StorePurchaseStatus.AlreadyPurchased:
                    // textBlock.Text = "The user has already purchased the product.";
                    ConsumeAddOn("9P44R6NSLH1P");
                    break;

                case StorePurchaseStatus.Succeeded:
                    //  textBlock.Text = "The purchase was successful.";
                    ConsumeAddOn("9P44R6NSLH1P");
                    break;

                case StorePurchaseStatus.NotPurchased:
                //    textBlock.Text = "The purchase did not complete. " +
                      //  "The user may have cancelled the purchase. ExtendedError: " + extendedError;
                    break;

                case StorePurchaseStatus.NetworkError:
                 //   textBlock.Text = "The purchase was unsuccessful due to a network error. " +
                    //    "ExtendedError: " + extendedError;
                    break;

                case StorePurchaseStatus.ServerError:
                 //   textBlock.Text = "The purchase was unsuccessful due to a server error. " +
                  //      "ExtendedError: " + extendedError;
                    break;

                default:
                 //   textBlock.Text = "The purchase was unsuccessful due to an unknown error. " +
                  //      "ExtendedError: " + extendedError;
                    break;
            }
        }
        public async void ConsumeAddOn(string addOnStoreId)
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
                // If your app is a desktop app that uses the Desktop Bridge, you
                // may need additional code to configure the StoreContext object.
                // For more info, see https://aka.ms/storecontext-for-desktop.
            }

            // This is an example for a Store-managed consumable, where you specify the actual number
            // of units that you want to report as consumed so the Store can update the remaining
            // balance. For a developer-managed consumable where you maintain the balance, specify 1
            // to just report the add-on as fulfilled to the Store.
            uint quantity = 1;
        
            Guid trackingId = Guid.NewGuid();

            workingProgressRing.IsActive = true;
            StoreConsumableResult result = await context.ReportConsumableFulfillmentAsync(
                addOnStoreId, quantity, trackingId);
            workingProgressRing.IsActive = false;

            // Capture the error message for the operation, if any.
            string extendedError = string.Empty;
            if (result.ExtendedError != null)
            {
                extendedError = result.ExtendedError.Message;
            }

            switch (result.Status)
            {
                case StoreConsumableStatus.Succeeded:
              
                    break;

                case StoreConsumableStatus.InsufficentQuantity:
             
                    break;

                case StoreConsumableStatus.NetworkError:
               
                    break;

                case StoreConsumableStatus.ServerError:
                
                    break;

                default:
                
                    break;
            }
        }
    }

}
