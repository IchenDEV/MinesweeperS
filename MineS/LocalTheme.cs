using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace MineS
{
    public static class LocalTheme
    {

        public static Themes Local { get; set; }
        public static void Initialize()
        {

        }

    }
    public class Themes
    {
        public Themes() { }
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
        private int MinSystemVer { get; set; }


    }
}
