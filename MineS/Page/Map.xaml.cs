﻿using CompositionSampleGallery.Samples.LightInterop;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
                        if (LocalTheme.Local.mineColor == "transparent")
                        {
                            UISettings ui = new UISettings();
                            ButtonCollection[i, j].Background = new SolidColorBrush(ui.GetColorValue(UIColorType.Accent));
                        }
                        else
                        {
                            var ss = LocalTheme.Local.mineColor.Remove(0, 1);
                            ButtonCollection[i, j].Background = new SolidColorBrush(Color.FromArgb(Convert.ToByte(ss.Substring(0, 2), 16), Convert.ToByte(ss.Substring(2, 2), 16), Convert.ToByte(ss.Substring(4, 2), 16), Convert.ToByte(ss.Substring(6, 2), 16)));
                        }

                    }
                    catch { }


                    ButtonCollection[i, j].Style = (Style)ioh[LocalTheme.Local.NomalButtonResouceName];
                    ButtonCollection[i, j].Tapped += B_Click;
                    ButtonCollection[i, j].RightTapped += B_RightTapped;
                    ButtonCollection[i, j].Holding += Map_Holding;
                    // ButtonCollection[i, j].Unloaded += Map_Unloaded;
                    ButtonCollection[i, j].Tag = new Point() { x = i, y = j };
                    Map1.Children.Add(ButtonCollection[i, j]);
                    Grid.SetRow(ButtonCollection[i, j], i);
                    Grid.SetColumn(ButtonCollection[i, j], j);
                }
                EnterStoryboard2.Begin();
            }

        }

        private void Map_Holding(object sender, HoldingRoutedEventArgs e)
        {
            M1.Play();
            var P = (sender as Button).Tag as Point;
            Marked[P.x, P.y] = !Marked[P.x, P.y];
            if (Marked[P.x, P.y])
            {

                (sender as Button).Style = (Style)ioh[LocalTheme.Local.MarkMineResouceName];
             //   (sender as Button).Background =;
            }
            else
            {
                (sender as Button).Style = (Style)ioh[LocalTheme.Local.NomalButtonResouceName];
            }
        }

        public Map()
        {


            this.InitializeComponent();
            MineButtonStyle = (Style)ioh[LocalTheme.Local.NomalButtonResouceName];
        }

        private void Unload(Point P)
        {
            if (!isOpened[P.x, P.y])
            {
                OpenedButton++;
            }

            if (MapNum[P.x, P.y] == 0 || isOpened[P.x, P.y])
            {
                Point[] bbs = new Point[1000];
                int  tail = 1;
                bbs[0] = P;
                for (int i = 0; i < tail; i++)
                {
                    var Pt = bbs[i];
                    for (int k = 0; k < 8; k++)
                    {
                        try
                        {
                            var Np = new Point() { x = Pt.x + direction[k].x, y = Pt.y + direction[k].y };
                            if (!isOpened[Np.x, Np.y] && !Marked[Np.x, Np.y])
                            {
                                OpenedButton++;
                                Marked[Np.x, Np.y] = false;
                                isOpened[Np.x, Np.y] = true;
                                if (MapNum[Np.x, Np.y] == 0)
                                {
                                    bbs[tail] = Np;
                                    tail++;
                                }
                                JudgeWinLost(Np);
                            }
                        }

                        catch { }

                    }
                }


            }
            else
            {
                isOpened[P.x, P.y] = true;
            }
            Refresh();
            JudgeWinLost(P);

        }

        private void JudgeWinLost(Point P)
        {
            if (MapNum[P.x, P.y] < 0)
            {
                watch.Stop();
                AchievementInfo.AllPlayTime = AchievementInfo.AllPlayTime.Add(watch.Elapsed);
                LostDialog lost = new LostDialog();

                AchievementInfo.GameoverTime++;
                roots.Children.Add(lost);
                try
                {
                    finished = true;


                    //M2.Play();

                    foreach (var item in ButtonCollection)
                    {
                        item.IsEnabled = false;
                        var p = (Point)item.Tag;
                        if (MapNum[p.x, p.y] < 0)
                        {
                            item.Content = "💣";

                        }

                    }
                }
                catch
                {

                }



            }
            if (OpenedButton > Widt * Heigh - AllNum && !finished)
            {
                finished = true;
                watch.Stop();
                var time = watch.Elapsed.TotalSeconds;
                AchievementInfo.AllPlayTime = AchievementInfo.AllPlayTime.Add(watch.Elapsed);
                // AchievementInfo.SinglePlayTime = AchievementInfo.SinglePlayTime watch.Elapsed;
                int source = (int)((AllNum * AllNum) * 1000 / (Widt * Heigh * time));
                AchievementInfo.MaxSocore = Math.Max(AchievementInfo.MaxSocore, source);
                AchievementInfo.AllSocore += source;

                WinDialog win = new WinDialog() { Source = source, Mode = String.Format("{3}({0},{1},{2})", Widt, Heigh, AllNum, Mode) };
                AchievementInfo.WinTime++;

                foreach (var item in ButtonCollection)
                {
                    try
                    {
                        item.IsEnabled = false;
                    }
                    catch { }
                }

                roots.Children.Add(win);
                M3.Play();
                EnterStoryboard.Begin();








            }
        }

        public void WinBack()
        {

            rootFrame.Navigate(typeof(SetPage), rootFrame);

        }

        private void B_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            try
            {
                M1.Play();
                var P = (sender as Button).Tag as Point;
                Marked[P.x, P.y] = !Marked[P.x, P.y];
                if (Marked[P.x, P.y])
                {

                    (sender as Button).Style = (Style)ioh[LocalTheme.Local.MarkMineResouceName];

                }
                else
                {
                    (sender as Button).Style = (Style)ioh[LocalTheme.Local.NomalButtonResouceName];
                }
            }
            catch
            {

            }

        }

        private void S_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            try
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
                    Unload(P);
                }
                Refresh();
            }
            catch (Exception)
            {


            }

        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            AchievementInfo.ClickTime++;
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

            try
            {
                Unload(P);
            }
            catch (Exception)
            {


            }

            try
            {
                Refresh();
            }
            catch (Exception)
            {


            }

            pr.IsActive = false;


        }
        /// <summary>
        /// 刷新视图
        /// </summary>
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
                                s.CornerRadius = new CornerRadius(54);
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

        //public static void VibrateOnce(double hs = 300)
        //{

        //    VibrationDevice vb = VibrationDevice.GetDefault();
        //    vb.Vibrate(TimeSpan.FromMilliseconds(hs));

        //}

        private void Mapv_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                float szoom = (float)Math.Min(Mapv.ActualWidth / (52 * Widt), Mapv.ActualHeight / (52 * Heigh));
                Mapv.ChangeView(0, 0, szoom);
                //  Mapv.ZoomToFactor(szoom);
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
