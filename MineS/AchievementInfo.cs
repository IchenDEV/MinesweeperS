using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineS
{
    public static class AchievementInfo
    {
      static  LocalObjectStorageHelper local = new LocalObjectStorageHelper();
        private static int _clickTime;
        public static int ClickTime
        {
            get
            {
                return local.Read("ClickTime",0);;
            }
            set
            {
                local.Save("ClickTime", value);
                _clickTime = value;

            }
        }

        private static int _gameoverTime;
        public static int GameoverTime
        {
            get
            {
                return local.Read("GameoverTime", 0); ;
            }
            set
            {
                local.Save("GameoverTime", value);
                _clickTime = value;

            }
        }

        private static int _winTime;
        public static int WinTime
        {
            get
            {
                return local.Read("WinTime", 0); ;
            }
            set
            {
                local.Save("WinTime", value);
                _clickTime = value;

            }
        }

        private static int _crashTime;
        public static int CrashTime
        {
            get
            {
                return local.Read("CrashTime", 0); ;
            }
            set
            {
                local.Save("CrashTime", value);
                _clickTime = value;

            }
        }
    }
}
