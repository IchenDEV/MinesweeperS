using CompositionSampleGallery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace MineS
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //LightInterop light = new LightInterop();
            //root.Children.Add(light);


            Map M = new Map(set.MapHeigh, set.MapWidth, set.NumMine);
            root.Children.Add(M);
            M.HorizontalAlignment = HorizontalAlignment.Stretch;
            M.VerticalAlignment = VerticalAlignment.Stretch;
            M.PropertyChanged += M_PropertyChanged;
            Showpp.Visibility = Visibility.Collapsed;
        }

        private void M_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           if((sender as Map).finished)
            {
                root.Children.Remove((sender as Map));
               Showpp.Visibility = Visibility.Visible;
            }
        }
    }
}
