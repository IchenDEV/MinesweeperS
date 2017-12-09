using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media;

namespace MineS
{
    public static class LocalTheme
    {
        static StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        // To get operating system version
        public static OSVersion OperatingSystemVersion => SystemInformation.OperatingSystemVersion;
        public static Themes Local { get; set; }
        public async static void Initialize()
        {

            var file = await localFolder.TryGetItemAsync("localTheme.json");
            if (file == null)
            {
                file = await localFolder.CreateFileAsync("localTheme.json", CreationCollisionOption.OpenIfExists);

                int bulid = OperatingSystemVersion.Build;
                if (bulid >= 16299)
                {
                    var Theme = new Themes()
                    {
                        ThemeName = "1709",
                        BackgroundResouceName = "SystemControlAcrylicWindowMediumHighBrush",
                        ClickMusic = "ms-appx:///Assets//Theme//1709//M.mp3",
                        BackIMage = "ms-appx:///Assets//Theme//1709//01_hiking_1920x1200.png",
                        IsAcrlic = true,
                        IsHighMode = true,
                        BoomMusic = "",
                        KilIM = "",
                        MineIM = "",
                        SuccessMusic = "",
                        MarkMineResouceName = "",
                        NomalMineResouceName = "",
                        ShowNumResouceName = "",
                        mineColor="#"
                    };
                    Local = Theme;
                    string js = TojsonData(Theme);
                    using (var stream = await (file as StorageFile).OpenStreamForWriteAsync())
                    {
                        using (StreamWriter sw = new StreamWriter(stream))
                        {
                            sw.Write(js);
                            await sw.FlushAsync();

                        }
                    }
                }




            }
            else
            {
                string result = String.Empty;
                using (var ms = await (file as StorageFile).OpenStreamForReadAsync())
                {
                    using (StreamReader reader = new StreamReader(ms))
                    {

                        result = reader.ReadToEnd();
                    }
                }
                Local = DeData<Themes>(result);
            }




        }
        public static string TojsonData(object item)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(item.GetType());
            string result = String.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, item);
                ms.Position = 0;
                using (StreamReader reader = new StreamReader(ms))
                {

                    result = reader.ReadToEnd();
                }

            }
            return result;
        }
        public static T DeData<T>(string jsonData)
        {
            var ds = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonData));
            T obj = (T)ds.ReadObject(ms);
            ms.Dispose();
            return obj;

        }


    }
    public class Themes
    {
        public Themes() { }
        public String ThemeName { get; set; }
        public String BackIMage { get; set; }
        public String MineIM { get; set; }
        public String KilIM { get; set; }
        public String ClickMusic { get; set; }
        public String BoomMusic { get; set; }
        public String SuccessMusic { get; set; }
        public bool IsHighMode { get; set; }
        public bool IsAcrlic { get; set; }
        public String MarkMineResouceName { get; set; }
        public String BackgroundResouceName { get; set; }
        public String NomalMineResouceName { get; set; }
        public String ShowNumResouceName { get; set; }
        public string mineColor { get; set; }
        private int MinSystemVer { get; set; }


    }
}
