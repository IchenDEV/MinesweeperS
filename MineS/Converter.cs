﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MineS
{
    public class ConverterTOIMG : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return new BitmapImage(new Uri(((List<string>)value)[0]));
            }
            catch
            {
                try
                {
                    return new BitmapImage(new Uri(value.ToString()));
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class ConverterTOResouce : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {ResourceDictionary re = new ResourceDictionary();
            try
            {

            re.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("ms-appx:///Resouse/Dictionary.xaml") });

            return re[value];
            }
            catch (Exception)
            {

                return re["ApplicationPageBackgroundThemeBrush"];
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class ObjectConverterTOable : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class InputNumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {
                int a = System.Convert.ToInt16(value);
                if (a > 3 && a < 35)
                {
                    return a;
                }
                else return 3;
            }
            catch
            {
                return 3;
            }


        }
    }
    public class ConverterTObrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            SolidColorBrush solidColorBrush = new SolidColorBrush((Color)value);

            return solidColorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
