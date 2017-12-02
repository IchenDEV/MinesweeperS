using CompositionSampleGallery.Samples.LightInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MineS
{
    public class Point
    {
        public int x, y;
    }
    public sealed partial class Map : Page, INotifyPropertyChanged
    {
        Stopwatch watch = new Stopwatch();
        Button[,] ButtonCollection;
        int OpenedButton = 0;
        int AllNum = 0;
        string Mode;
        bool[,] Mine;
        int[,] MapNum;
        bool[,] Marked;
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

        public Map(int H, int W, int N)
        {

            MineButtonStyle = (Style)Resources["ButtonRevealStyle"];
            this.InitializeComponent();
            Heigh = H;
            Widt = W;
            AllNum = N;
            Mine = new bool[H, W];
            Marked = new bool[H, W];
            MapNum = new int[H, W];

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

                    Binding binding = new Binding();
                    binding.Path = new PropertyPath("MineButtonStyle");
                    binding.Mode = BindingMode.OneWay;
                    ButtonCollection[i, j].SetBinding(Button.StyleProperty, binding);
                    ButtonCollection[i, j].Click += B_Click;
                    ButtonCollection[i, j].RightTapped += B_RightTapped;
                    ButtonCollection[i, j].Unloaded += Map_Unloaded;
                    ButtonCollection[i, j].Tag = new Point() { x = i, y = j };
                    Map1.Children.Add(ButtonCollection[i, j]);
                    Grid.SetRow(ButtonCollection[i, j], i);
                    Grid.SetColumn(ButtonCollection[i, j], j);
                }
            }
        }

        private async void Map_Unloaded(object sender, RoutedEventArgs e)
        {
            OpenedButton++;
            var P = (sender as Button).Tag as Point;
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
                s.Background = new MaterialBrush();
                s.Lights.Add(new HoverLight());
                s.Lights.Add(new AmbLight());
                s.Tag = P;
                s.DoubleTapped += S_DoubleTapped;
                s.Children.Add(text);
                Grid.SetRow(s, P.x);
                Grid.SetColumn(s, P.y);
                Map1.Children.Add(s);
            }
            else if (MapNum[P.x, P.y] == 0)
            {
                for (int k = 0; k < 8; k++)
                {
                    try
                    {
                        if (Map1.Children.Contains((ButtonCollection[(P.x + direction[k].x), (P.y + direction[k].y)])))
                        {
                            Marked[P.x + direction[k].x, P.y + direction[k].y] = false;
                            Map1.Children.Remove((ButtonCollection[(P.x + direction[k].x), (P.y + direction[k].y)]));

                        }

                    }
                    catch { }
                }
            }
            else
            {
                finished = true;
            }
            if (OpenedButton == Widt * Heigh - AllNum&&!finished)
            {
                watch.Stop();
                var time = watch.Elapsed.TotalSeconds;
                int source = (int)((AllNum * AllNum)*100 / (Widt * Heigh * time));
                WinDialog win = new WinDialog() { Source = source, Mode = String.Format("{0},{1},{2}",Widt,Heigh,AllNum) };
                root.Children.Add(win);

            }
        }

        private void B_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var P = (sender as Button).Tag as Point;
            Marked[P.x, P.y] = !Marked[P.x, P.y];
            if (Marked[P.x, P.y])
            {
                (sender as Button).Style = (Style)Resources["ButtonRevealMarkedStyle"];
            }
            else
            {
                (sender as Button).Style = (Style)Resources["ButtonRevealStyle"];
            }
        }



        private void S_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
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
                        if (Marked[P.x + direction[k].x, P.y + direction[k].y] == false)
                        {
                            Map1.Children.Remove((ButtonCollection[(P.x + direction[k].x), (P.y + direction[k].y)]));
                        }

                    }
                    catch { }
                }
            }
        }

        private  void B_Click(object sender, RoutedEventArgs e)
        {
            var P = (sender as Button).Tag as Point;
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
            Map1.Children.Remove((sender as Button));
           
        }

        private void Mapv_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                float szoom = (float)Math.Min(Mapv.ActualWidth / (50 * Widt), Mapv.ActualHeight / (50 * Heigh));
                Mapv.ChangeView(0, 0, szoom);
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
