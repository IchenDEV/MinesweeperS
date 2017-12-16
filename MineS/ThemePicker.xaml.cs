using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class ThemePicker : UserControl
    {
        public ThemePicker()
        {
            this.InitializeComponent();
        }
        ObservableCollection<Themes> them = new ObservableCollection<Themes>();
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {


            var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Theme");

            List<StorageFile> SL = (await folder.GetFilesAsync()).ToList();
            foreach (var item in SL)
            {
                string result = "";
                using (Stream str = await item.OpenStreamForReadAsync())
                {
                    using (StreamReader streamReader = new StreamReader(str))
                    {
                        result = await streamReader.ReadToEndAsync();
                    }
                }

                Themes t = LocalTheme.DeData<Themes>(result);
                them.Add(t);
            }
        }

        private async void Gv_ItemClick(object sender, ItemClickEventArgs e)
        {
            LocalTheme.Local = (e.ClickedItem as Themes);
            await LocalTheme.write(LocalTheme.TojsonData(e.ClickedItem));
        }
    }
}
