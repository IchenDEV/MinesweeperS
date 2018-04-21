using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private static Themes _local;
        public static Themes Local
        {
            get
            {
                Initialize();
                return _local;
            }
            set
            {
                _local = value;
            }
        }

        public async static Task<Themes> Initialize()
        {

            var file = await localFolder.TryGetItemAsync("localTheme.json");
            try
            {
                string result = String.Empty;
                using (var ms = await (file as StorageFile).OpenStreamForReadAsync())
                {
                    using (StreamReader reader = new StreamReader(ms))
                    {

                        result = reader.ReadToEnd();
                    }
                }

                var themes = DeData<Themes>(result);
                _local = themes;
                return themes;
            }
            catch (Exception)
            {
                try
                {
                  await  file.DeleteAsync();
                }
                catch (Exception)
                {

                    //   throw;
                }

                if (file == null)
                {
                    file = await localFolder.CreateFileAsync("localTheme.json", CreationCollisionOption.OpenIfExists);

                    int bulid = OperatingSystemVersion.Build;
                    if (bulid >= 16299)
                    {
                        var Theme = new Themes()
                        {
                            ThemeName = "Snow",
                            ClickMusic = "ms-appx:///Assets//Theme//1709//M.mp3",
                            BackIMage = new List<string>(),
                            IsHighMode = true,
                            BoomMusic = "ms-appx:///Assets//Theme//1709//MP.mp3",
                            KilIM = "",
                            MineIM = "",
                            MarkMusic = "ms-appx:///Assets//Theme//1709//win.mp3",
                            SuccessMusic = "ms-appx:///Assets//Theme//1709//win.mp3",
                            MarkMineResouceName = "ButtonRevealMarkedStyle",
                            NomalButtonResouceName = "MyButtonStyle",
                            ShowNumResouceName = "",
                            mineColor = "#800078D7"
                            ,
                            AppButtonResouceName = "AppBarButtonRevealStyle"
                        };
                        Theme.BackIMage.Add("ms-appx:///Assets//trans.png");
                        _local = Theme;
                        string js = TojsonData(Theme);
                        await write(js);
                        return Theme;
                    }
                    else
                    {
                        var Theme = new Themes()
                        {

                            ThemeName = "Low",
                            ClickMusic = "ms-appx:///Assets//Theme//1709//M.mp3",
                            BackIMage = new List<string>(),
                            IsAcrlic = false,
                            IsHighMode = false,
                            BoomMusic = "ms-appx:///Assets//Theme//1709//MP.mp3",
                            KilIM = "",
                            MineIM = "",
                            MarkMusic = "ms-appx:///Assets//Theme//1709//win.mp3",
                            SuccessMusic = "ms-appx:///Assets//Theme//1709//win.mp3",
                            AppButtonResouceName = "AccentButtonStyle",
                            NomalButtonResouceName = "AccentButtonStyle",
                            MarkMineResouceName = "ButtonMarkedStyle",
                            ShowNumResouceName = "",
                            mineColor = "#800078D7"
                        };
                        Theme.BackIMage.Add("ms-appx:///Assets//Theme//1709//01_hiking_1920x1200.png");
                        _local = Theme;
                        string js = TojsonData(Theme);
                        await write(js);
                        return Theme;
                    }

                }
            }
            return new Themes();





        }

        public static async Task write(string js)
        {
            var file
             = await localFolder.CreateFileAsync("localTheme.json", CreationCollisionOption.ReplaceExisting);
            using (var stream = await (file as StorageFile).OpenStreamForWriteAsync())
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(js);
                    await sw.FlushAsync();

                }
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
        public bool AutoBackChange { get; set; }
        public List<String> BackIMage { get; set; }
        public String MineIM { get; set; }
        public String KilIM { get; set; }
        public String ClickMusic { get; set; }
        public String BoomMusic { get; set; }
        public String MarkMusic { get; set; }
        public String SuccessMusic { get; set; }
        public bool IsHighMode { get; set; }
        public bool IsAcrlic { get; set; }
        public String MarkMineResouceName { get; set; }
        public String BackgroundResouceName { get; set; }
        public String SysELAcrResouceName { get; set; }
        public String SupELAcrResouceName { get; set; }
        public String SupELAcrLostResouceName { get; set; }
        public String NomalButtonResouceName { get; set; }
        public String AppButtonResouceName { get; set; }

        public String ShowNumResouceName { get; set; }
        public bool isAutoColor { get; set; }
        public string mineColor { get; set; }
        private int MinSystemVer { get; set; }


    }
}
