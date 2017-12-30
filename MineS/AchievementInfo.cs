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


        private static List<Achievements> _Achinves;
        public static List<Achievements> Achinves
        {
            get
            {
                return local.Read<List<Achievements>>("Achinves", new List<Achievements>()); ;
            }
            set
            {
                local.Save("Achinves", value);
                _Achinves = value;


            }
        }

        public static void DeAC()
        {
            Achinves.Add(new Achievements() { name="Insider",caption="Taster"});

        }
     

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
                _gameoverTime = value;

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
                _winTime = value;

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
                _crashTime = value;

            }
        }

        private static int _maxSocore;
        public static int MaxSocore
        {
            get
            {
                return local.Read("MaxSocore", 0); ;
            }
            set
            {
                local.Save("MaxSocore", value);
                _maxSocore = value;

            }
        }

        private static int _allSocore;
        public static int AllSocore
        {
            get
            {
                return local.Read("AllSocore", 0); ;
            }
            set
            {
                local.Save("AllSocore", value);
                _allSocore = value;

            }
        }

        private static DateTimeOffset _singlePlayTime;
        public static DateTimeOffset SinglePlayTime
        {
            get
            {
                return local.Read<DateTimeOffset>("SinglePlayTime", new DateTimeOffset (0,TimeSpan.Zero)); ;
            }
            set
            {
                local.Save<DateTimeOffset>("SinglePlayTime", value);
                _singlePlayTime = value;

            }
        }
        private static DateTimeOffset _allPlayTime;
        public static DateTimeOffset AllPlayTime
        {
            get
            {
                return local.Read<DateTimeOffset>("AllPlayTime", new DateTimeOffset(0, TimeSpan.Zero)); ;
            }
            set
            {
                local.Save<DateTimeOffset>("AllPlayTime", value);
                _allPlayTime = value;

            }
        }



    }
}
