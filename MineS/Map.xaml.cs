using CompositionSampleGallery.Samples.LightInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.Devices.Notification;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MineS
{
    public class Point
    {
        public int x, y;
    }
    public class MapData
    {
        public int w, h, mine;
        public string mode;
        public Frame local;
    }
    public sealed partial class Map : Page, INotifyPropertyChanged
    {
        Frame rootFrame;
        Stopwatch watch = new Stopwatch();
        Button[,] ButtonCollection;
        int OpenedButton = 0;
        int AllNum = 0;
        string Mode;
        bool[,] Mine;
        int[,] MapNum;
        bool[,] Marked;
        bool[,] isOpened;
        Point[] direction = new Point[] {new Point (){ x=0, y=1 }, new Point() { x = 0, y = -1 },new Point (){ x=1, y=1 },new Point (){ x=1, y=0 },
        new Point (){ x=1, y=-1 },new Point (){ x=-1, y=1 },new Point (){ x=-1, y=0 },new Point (){ x=-1, y=-1 }};
        int Heigh;
        int Widt;
        bool _finished = false;
        private Style _MineButtonStyle;
        public Style MineButtonStyle
        {
            get { return this._MineButtonStyle; }
            set
            {
                this._MineButtonStyle = value;
                this.OnPropertyChanged();
            }
        }
        public bool finished
        {
            get { return this._finished; }
            set
            {
                this._finished = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        void SetMine(Point notMine)
        {
            Random r = new Random();
            int x, y;
            for (int i = 0; i < AllNum; i++)
            {
                x = r.Next(Heigh);
                y = r.Next(Widt);
                if (x == notMine.x && y == notMine.y)
                {
                    i--;
                }
                else if (Mine[x, y])
                {
                    i--;
                }
                else
                {
                    Mine[x, y] = true;
                }
            }

            for (int i = 0; i < Heigh; i++)
            {
                for (int j = 0; j < Widt; j++)
                {
                    if (Mine[i, j])
                    {
                        MapNum[i, j] = -10;
                        for (int k = 0; k < 8; k++)
                        {
                            try
                            {
                                MapNum[i + direction[k].x, j + direction[k].y]++;
                            }
                            catch { }

                        }
                    }

                }
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var data =
         ((MapData)e.Parameter);
            int H = data.h, W = data.w, N = data.mine;
            rootFrame = data.local;
            Mode = data.mode;
            Heigh = H;
            Widt = W;
            AllNum = N;
            Mine = new bool[H, W];
            Marked = new bool[H, W];
            MapNum = new int[H, W];
            isOpened = new bool[H, W];
            for (int i = 0; i < H; i++)
            {
                Map1.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) });
            }

            for (int i = 0; i < W; i++)
            {
                Map1.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50) });
            }
            ButtonCollection = new Button[H, W];
            for (int i = 0; i < H; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    ButtonCollection[i, j] = new Button();
                    //ButtonCollection[i, j].Name = i.ToString() + j.ToString();
                    ButtonCollection[i, j].HorizontalAlignment = HorizontalAlignment.Stretch;
                    ButtonCollection[i, j].Margin = new Thickness(2);
                    ButtonCollection[i, j].VerticalAlignment = VerticalAlignment.Stretch;
                    try
                    {
                        var ss = LocalTheme.Local.mineColor.Remove(0, 1);

                        ButtonCollection[i, j].Background = new SolidColorBrush(Color.FromArgb(Convert.ToByte(ss.Substring(0, 2), 16), Convert.ToByte(ss.Substring(2, 2), 16), Convert.ToByte(ss.Substring(4, 2), 16), Convert.ToByte(ss.Substring(6, 2), 16)));
                    }
                    catch { }
                  
                    Binding binding = new Binding();
                    binding.Path = new PropertyPath("MineButtonStyle");
                    binding.Mode = BindingMode.OneWay;
                    ButtonCollection[i, j].SetBinding(Button.StyleProperty, binding);
                    ButtonCollection[i, j].Tapped += B_Click;
                    ButtonCollection[i, j].RightTapped += B_RightTapped;
                    ButtonCollection[i, j].Holding += Map_Holding;
                   // ButtonCollection[i, j].Unloaded += Map_Unloaded;
                   ButtonCollection[i, j].Tag = new Point() { x = i, y = j };
                    Map1.Children.Add(ButtonCollection[i, j]);
                    Grid.SetRow(ButtonCollection[i, j], i);
                    Grid.SetColumn(ButtonCollection[i, j], j);
                }
            }

        }

        private void Map_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var P = (sender as Button).Tag as Point;
            Marked[P.x, P.y] = !Marked[P.x, P.y];
            if (Marked[P.x, P.y])
            {


                (sender as Button).Style = (Style)ioh["ButtonRevealMarkedStyle"];

            }
            else
            {
                (sender as Button).Style = (Style)Resources["ButtonRevealStyle"];
            }
        }

        public Map()
        {

            MineButtonStyle = (Style)Resources["ButtonRevealStyle"];
            this.InitializeComponent();

        }

        private async
        Task
Unload(Point P)
        {


            if (MapNum[P.x, P.y] == 0)
            {
                for (int k = 0; k < 8; k++)
                {
                    try
                    {
                        if (!isOpened[(P.x + direction[k].x), (P.y + direction[k].y)])
                        {
                            OpenedButton++;
                            Marked[P.x + direction[k].x, P.y + direction[k].y] = false;
                            isOpened[(P.x + direction[k].x), (P.y + direction[k].y)] = true;

                            Unload(new Point() { x = P.x + direction[k].x, y = P.y + direction[k].y });
                        }




                    }
                    catch { }

                }

            }

            else if (MapNum[P.x, P.y] < 0)
            {
                finished = true;
                watch.Stop();
                LostDialog lost = new LostDialog();
                root.Children.Add(lost);
                lost.PropertyChanged += Win_PropertyChanged;

                foreach (var item in ButtonCollection)
                {
                    item.IsEnabled = false;
                }



            }
            if (OpenedButton == Widt * Heigh - AllNum && !finished)
            {
                watch.Stop();
                var time = watch.Elapsed.TotalSeconds;
                int source = (int)((AllNum * AllNum) * 1000 / (Widt * Heigh * time));
                WinDialog win = new WinDialog() { Source = source, Mode = String.Format("{0},{1},{2}\n{3}", Widt, Heigh, AllNum, Mode) };
                root.Children.Add(win);
                win.PropertyChanged += Win_PropertyChanged;
                foreach (var item in ButtonCollection)
                {
                    item.IsEnabled = false;
                }
                EnterStoryboard.Begin();
                
            }
        }

        private void Win_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Back")
            {
                rootFrame.Navigate(typeof(SetPage), rootFrame);
            }
        }

        private void B_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var P = (sender as Button).Tag as Point;
            Marked[P.x, P.y] = !Marked[P.x, P.y];
            if (Marked[P.x, P.y])
            {


                (sender as Button).Style = (Style)ioh["ButtonRevealMarkedStyle"];

            }
            else
            {
                (sender as Button).Style = (Style)Resources["ButtonRevealStyle"];
            }
        }



        private async void S_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var P = (sender as Grid).Tag as Point;
            int ActMark = 0;
            for (int k = 0; k < 8; k++)
            {
                try
                {
                    if (Marked[P.x + direction[k].x, P.y + direction[k].y])
                    {
                        ActMark++;

                    }
                }
                catch { }
            }
            if (((sender as Grid).Children[0] as TextBlock).Text == ActMark.ToString())
            {
                for (int k = 0; k < 8; k++)
                {
                    try
                    {
                        if (Marked[P.x + direction[k].x, P.y + direction[k].y] == false && !isOpened[P.x + direction[k].x, P.y + direction[k].y])
                        {
                            OpenedButton++;
                            isOpened[(P.x + direction[k].x), (P.y + direction[k].y)] = true;
                            pr.IsActive = true;
                            await Unload(new Point() { x = P.x + direction[k].x, y = P.y + direction[k].y });
                            pr.IsActive = false;
                        }

                    }
                    catch { }
                }
            }
            Refresh();
        }

        private async void B_Click(object sender, RoutedEventArgs e)
        {
            var P = (sender as Button).Tag as Point;
         
            M3.Play();
            pr.IsActive = true;
            if (Marked[P.x, P.y])
            {
                return;
            }
            if (OpenedButton == 0)
            {
                SetMine((sender as Button).Tag as Point);
                watch.Start();
            }
            else
            {

            }
            OpenedButton++;
            isOpened[P.x, P.y] = true;

            await Unload(P);

            Refresh();
            pr.IsActive = false;


        }

        private void Refresh()
        {
            for (int i = 0; i < Heigh; i++)
            {
                for (int j = 0; j < Widt; j++)
                {
                    if (isOpened[i, j])
                    {

                        if (ButtonCollection[i, j].Visibility == Visibility.Visible)
                        {
                            var P = new Point() { x = i, y = j };
                            if (MapNum[P.x, P.y] > 0)
                            {
                                TextBlock text = new TextBlock()
                                {
                                    Text = MapNum[P.x, P.y].ToString(),
                                    FontSize = 25,
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Center

                                };

                                Grid s = new Grid();
                                if (LocalTheme.Local.IsHighMode)
                                {
                                    s.Background = new MaterialBrush();
                                    s.Lights.Add(new HoverLight());
                                    s.Lights.Add(new AmbLight());
                                }

                                s.Tag = P;
                                s.DoubleTapped += S_DoubleTapped;
                                s.Children.Add(text);
                                Grid.SetRow(s, P.x);
                                Grid.SetColumn(s, P.y);
                                Map1.Children.Add(s);
                            }
                            ButtonCollection[i, j].Visibility = Visibility.Collapsed;
                        }


                    }

                }
            }
        }

        public static void VibrateOnce(double hs = 300)
        {

            VibrationDevice vb = VibrationDevice.GetDefault();
            vb.Vibrate(TimeSpan.FromMilliseconds(hs));

        }

        private void Mapv_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                float szoom = (float)Math.Min(Mapv.ActualWidth / (50 * Widt), Mapv.ActualHeight / (50 * Heigh));
              // Mapv.ChangeView(0,0 , szoom);
                Mapv.ZoomToFactor(szoom);
            }
            catch
            {

            }


        }
        private void ShButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            finished = true;
        }
    }
}
