using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace MineS
{
    public sealed partial class WinDialog : UserControl, INotifyPropertyChanged
    {
        private int _Source;
        private string _Mode;
        private bool _back;
        public bool Back
        {
            get { return this._back; }
            set
            {
                this._back = value;
                this.OnPropertyChanged();
            }
        }
        public WinDialog()
        {
            this.InitializeComponent();
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
            // Initialize the InkCanvas

            inkCanvas.InkPresenter.InputDeviceTypes =

                Windows.UI.Core.CoreInputDeviceTypes.Mouse | Windows.UI.Core.CoreInputDeviceTypes.Touch |

                Windows.UI.Core.CoreInputDeviceTypes.Pen;

        }
        private StorageFile _tempExportFile;
        private async void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var _tempExportFile = await ApplicationData.Current.LocalFolder.GetFileAsync("win.jpg");
            try
            {
                DataPackage requestData = args.Request.Data;
                requestData.Properties.Title = "Share";
                requestData.Properties.Description = "Beat me in Here";

                List<IStorageItem> imageItems = new List<IStorageItem> { _tempExportFile };
                requestData.SetStorageItems(imageItems);

                RandomAccessStreamReference imageStreamRef = RandomAccessStreamReference.CreateFromFile(_tempExportFile);
                requestData.Properties.Thumbnail = imageStreamRef;
                requestData.SetBitmap(imageStreamRef);
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message, "爆了").ShowAsync();
            }
        }

        public int Source
        {
            get { return this._Source; }
            set
            {
                this._Source = value;
                this.OnPropertyChanged();
            }
        }

        public String Mode
        {
            get { return this._Mode; }
            set
            {
                this._Mode = value;
                this.OnPropertyChanged();
            }
        }



        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            sin.Visibility = Visibility.Collapsed;
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            ExitStoryboard.Begin();
            ExitStoryboard.Completed += ExitStoryboard_Completed;

        }

        private void ExitStoryboard_Completed(object sender, object e)
        {
            Back = true;

        }

        private void inkCanvas_CharacterReceived(object sender, ManipulationStartedRoutedEventArgs e)
        {
            sin.Visibility = Visibility.Collapsed;
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            EnterStoryboard.Begin();
        }
    }
}
