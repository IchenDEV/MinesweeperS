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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MineS
{
    public class Point
    {
        public int x, y;
    }
    public sealed partial class Map : Page, INotifyPropertyChanged
    {
        int OpenedButton = 0;
        int AllNum = 0;
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
            for (int i = 0; i < H; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    Button b = new Button
                    {
                        Name = i.ToString() + j.ToString(),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Margin = new Thickness(5),
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    Binding binding = new Binding();
                    binding.Path = new PropertyPath("MineButtonStyle");
                    binding.Mode = BindingMode.OneWay;
                    b.SetBinding(Button.StyleProperty, binding);
                    b.Click += B_Click;
                    b.RightTapped += B_RightTapped;
                    b.Unloaded += B_Unloaded;
                    b.Tag = new Point() { x = i, y = j };
                    Map1.Children.Add(b);
                    Grid.SetRow(b, i);
                    Grid.SetColumn(b, j);
                }
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

        private void B_Unloaded(object sender, RoutedEventArgs e)
        {
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
                s.DoubleTapped += S_DoubleTapped;
                s.Children.Add(text);
                Grid.SetRow(s, P.x);
                Grid.SetColumn(s, P.y);
                s.Tag = P;
                Map1.Children.Add(s);
            }
            else if (MapNum[P.x, P.y] == 0)
            {
                for (int k = 0; k < 8; k++)
                {
                    try
                    {
                        Marked[P.x + direction[k].x, P.y + direction[k].y] = false;
                        Map1.Children.Remove((FindName((P.x + direction[k].x).ToString() + (P.y + direction[k].y).ToString()) as Button));
                    }
                    catch { }
                }
            }
            else
            {
                finished = true;
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
                            Map1.Children.Remove((FindName((P.x + direction[k].x).ToString() + (P.y + direction[k].y).ToString()) as Button));
                        }

                    }
                    catch { }
                }
            }
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            var P = (sender as Button).Tag as Point;
            if (Marked[P.x, P.y])
            {
                return;
            }
            OpenedButton++;
            if (OpenedButton == 1)
            {
                SetMine((sender as Button).Tag as Point);
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
                float szoom = (float)Math.Min(Mapv.ActualWidth / (50*Widt), Mapv.ActualHeight / (50*Heigh));
                Mapv.ChangeView(0, 0, szoom);
            }
            catch
            {

            }


        }
    }
}
