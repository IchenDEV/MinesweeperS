using System;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using System.Numerics;
using Windows.UI.Xaml.Hosting;
using SamplesCommon;
using Windows.UI.ViewManagement;

namespace MineS
{
    public sealed partial class ShButton : UserControl
    {




        // Sample metadata


        public ShButton()
        {
            var uisetting = new UISettings();
            uisetting.ColorValuesChanged += Uisetting_ColorValuesChanged;

            this.InitializeComponent();

        }

        private async void Uisetting_ColorValuesChanged(UISettings sender, object args)
        {
            try
            {
                Color bg, fg;
                bg = sender.GetColorValue(UIColorType.AccentLight1);
            
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                    () =>
                    {
                        SolidColorBrush brush = bt.Background as SolidColorBrush;
                        if (brush == null)
                        {
                            brush = new SolidColorBrush();
                        }
                        brush.Color = bg;
                        bt.Background = new SolidColorBrush(bg);
                    });

            }
            catch (Exception)
            {

              
            }

        }





        private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Switch.Begin();
        }

        private void but_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Switch.SkipToFill();
            Switch.Stop();
        }


    }
}
