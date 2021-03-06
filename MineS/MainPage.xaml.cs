﻿using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
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
            LocalTheme.Initialize();
            this.InitializeComponent();

            froot.Navigate(typeof(SetPage), froot);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void M_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if ((sender as Map).finished)
            {
                root.Children.Remove((sender as Map));

            }
        }

        private void froot_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.SourcePageType == typeof(Map))
            {
                newGame.Visibility = Visibility.Visible;
            }
            else
            {
                newGame.Visibility = Visibility.Collapsed;
            }
        }

        private void ShButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            froot.Navigate(typeof(SetPage), froot);
            // newGame.Visibility = Visibility.Collapsed;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                im.Source = new BitmapImage(new Uri((await(LocalTheme.Initialize())).BackIMage[0]));
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

            }
            catch (Exception)
            {
                while (true)
                {
                    try
                    {
                        im.Source = new BitmapImage(new Uri((await (LocalTheme.Initialize())).BackIMage[0]));
                        return;
                    }
                    catch (Exception)
                    {

                        //throw;
                    }
                }



            }
        }
    }
}
