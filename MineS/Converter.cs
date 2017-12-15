using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MineS
{
   public class ConverterTOIMG:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
       
            return new BitmapImage(new Uri(((List<string>)value)[0]));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class ConverterTOResouce : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ResourceDictionary re = new ResourceDictionary();
            re.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("ms-appx:///Resouse/Dictionary.xaml") });
            
            return re[value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
