
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace MineS
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SetPage : UserControl, INotifyPropertyChanged

    {
        public SetPage()
        {
            this.InitializeComponent();
        }
        private int _MapHeigh = 0;
        private int _MapWidth = 0;
        private int _NumMine = 0;
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




        private void Easy_Click(object sender, RoutedEventArgs e)
        {
            isCustom = false;
            MapHeigh = 10; MapWidth = 10; NumMine = 9;

        }

        private void Mid_Click(object sender, RoutedEventArgs e)
        {
            isCustom = false;
            MapHeigh = 10; MapWidth = 20; NumMine = 9;
        }

        private void Hatd_Click(object sender, RoutedEventArgs e)
        {
            isCustom = false;
            MapHeigh = 15; MapWidth = 15; NumMine = 9;
        }

        private void Custom_Click(object sender, RoutedEventArgs e)
        {
            isCustom = true;
        }

       
    }
}
