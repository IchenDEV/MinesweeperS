using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Input.Inking;
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

        public WinDialog()
        {
            this.InitializeComponent();
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
            // Initialize the InkCanvas

            inkCanvas.InkPresenter.InputDeviceTypes =

                Windows.UI.Core.CoreInputDeviceTypes.Mouse | Windows.UI.Core.CoreInputDeviceTypes.Touch |

                Windows.UI.Core.CoreInputDeviceTypes.Pen;
            inkCanvas.ManipulationStarted += InkCanvas_ManipulationStarted;
            try
            {
                if (ActualTheme == ElementTheme.Dark)
                {
                    var attr = new InkDrawingAttributes
                    {
                        Color = Colors.White, //颜色
                        IgnorePressure = false,  //是否忽略数字化器表面上的接触压力
                        PenTip = PenTipShape.Circle, //笔尖类型设置
                        Size = new Size(4, 10), //画笔粗细

                    };
                    //更新画笔
                    inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(attr);
                }
            }
            catch (Exception)
            {


            }




        }

        private void InkCanvas_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            sin.Visibility = Visibility.Collapsed;
        }

        private StorageFile _tempExportFile;
        private async Task StoImg()
        {
            var bitmap = new RenderTargetBitmap();

            _tempExportFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("win.jpg", CreationCollisionOption.ReplaceExisting);

            await bitmap.RenderAsync(((MainPage)((Grid)((Frame)((Map)((Grid)this.Parent).Parent).Parent).Parent).Parent));

            var buffer = await bitmap.GetPixelsAsync();

            using (IRandomAccessStream stream = await _tempExportFile.OpenAsync(FileAccessMode.ReadWrite))

            {

                var encod = await BitmapEncoder.CreateAsync(

                    BitmapEncoder.JpegEncoderId, stream);

                encod.SetPixelData(BitmapPixelFormat.Bgra8,

                    BitmapAlphaMode.Ignore,

                    (uint)bitmap.PixelWidth,

                    (uint)bitmap.PixelHeight,

                    DisplayInformation.GetForCurrentView().LogicalDpi,

                    DisplayInformation.GetForCurrentView().LogicalDpi,

                    buffer.ToArray()

                   );

                await encod.FlushAsync();

            }
        }
        private async void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {

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

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            await StoImg();
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
            ((Map)((Grid)this.Parent).Parent).WinBack();
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
