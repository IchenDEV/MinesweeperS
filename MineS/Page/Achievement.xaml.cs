using System;
using System.Collections.ObjectModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace MineS
{
    public class Achievements
    {
        public int Id ;
        public string name = "";
        public string caption = "";
        public string img;
        public DateTime GetTime;
    }
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Achievement : Page
    {
        public Achievement()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            // AchievementInfo.DeAC();
            this.InitializeComponent();
        }
        ObservableCollection<Achievements> AC = new ObservableCollection<Achievements>(AchievementInfo.Achinves);
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty. 
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty. 
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
          
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            dilParent.Visibility = Visibility.Visible;
            AchDital dil = new AchDital() { Local = (Achievements)e.ClickedItem };
            dilParent.Children.Insert(0,dil);
            EnterStoryboard.Begin();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            ExitStoryboard.Begin();
            ExitStoryboard.Completed += ExitStoryboard_Completed;
     
        }

        private void ExitStoryboard_Completed(object sender, object e)
        {
            foreach (var item in dilParent.Children)
            {
                if (item.GetType() == typeof(AchDital)){
                    dilParent.Children.Remove(item);
                }
            }     
            dilParent.Visibility = Visibility.Collapsed;
        }
    }
    
}
