using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Media.Imaging;

namespace MineS
{
    public static class AchievementInfo
    {
        static LocalObjectStorageHelper local = new LocalObjectStorageHelper();


        private static List<Achievements> _Achinves = local.Read<List<Achievements>>("Achinves", new List<Achievements>());
        public static List<Achievements> Achinves
        {
            get
            {
                try
                {
                    return _Achinves;
                }
                catch
                {
                    return new List<Achievements>();
                }
            }
            set
            {
                local.Save("Achinves", value);
                _Achinves = value;


            }
        }

        public static void DeAC()
        {
            GenerateToastContentAnPop("Insider","Insider.png", "Taster" );
        }


        public static void GenerateToastContentAnPop(string Name, string Img, string Caption)
        {
            foreach (var item in Achinves)
            {
                if (item.name == Name)
                {
                    return;
                }
            }

            _Achinves.Add(new Achievements() { GetTime = DateTime.Now, name = Name, caption = Caption, img = "ms-appx:///Assets/Achievent/"+Img });
            local.Save("Achinves", _Achinves);

            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BaseUri = new Uri("ms-appx:///Assets/Achievent/"),
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
            {
                new AdaptiveText()
                {
                    Text = "Wow,You get "+Name+"!!!"
                },

                new AdaptiveText()
                {
            Text = Caption
                },

                new AdaptiveGroup()
                {
                    Children =
                    {
                        new AdaptiveSubgroup()
                        {
                            Children =
                            {
                                new AdaptiveImage()
                                {
                                    HintRemoveMargin = true,
                                    Source = Img
                                }
                            }
                        }
                    }
                }
            }
                    }
                }
            };

            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);

            var tileContent = new TileContent()
            {
                Visual = new TileVisual()
                {
                    Branding = TileBranding.NameAndLogo,
                    TileMedium = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            TextStacking = TileTextStacking.Center,
                            Children =
                {
                    new AdaptiveText()
                    {
                        Text = "Hi,",
                        HintStyle = AdaptiveTextStyle.Base,
                        HintAlign = AdaptiveTextAlign.Center
                    },
                    new AdaptiveText()
                    {
                        Text = Name,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                        HintAlign = AdaptiveTextAlign.Center
                    }
                },
                            PeekImage = new TilePeekImage()
                            {
                                Source = "ms-appx:///Assets/Achievent/"+Img,
                                HintCrop = TilePeekImageCrop.Circle
                            }
                        }
                    },
                    TileWide = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                {
                    new AdaptiveGroup()
                    {
                        Children =
                        {
                            new AdaptiveSubgroup()
                            {
                                HintWeight = 33,
                                Children =
                                {
                                    new AdaptiveImage()
                                    {
                                        HintCrop = AdaptiveImageCrop.Circle,
                                        Source = "ms-appx:///Assets/Achievent/"+Img,
                                    }
                                }
                            },
                            new AdaptiveSubgroup()
                            {
                                Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = "Hi,",
                                        HintStyle = AdaptiveTextStyle.Title
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = Name,
                                        HintStyle = AdaptiveTextStyle.SubtitleSubtle
                                    }
                                },
                                HintTextStacking = AdaptiveSubgroupTextStacking.Center
                            }
                        }
                    }
                }
                        }
                    },
                    TileLarge = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            TextStacking = TileTextStacking.Center,
                            Children =
                {
                    new AdaptiveGroup()
                    {
                        Children =
                        {
                            new AdaptiveSubgroup()
                            {
                                HintWeight = 1
                            },
                            new AdaptiveSubgroup()
                            {
                                HintWeight = 2,
                                Children =
                                {
                                    new AdaptiveImage()
                                    {
                                        HintCrop = AdaptiveImageCrop.Circle,
                                        Source = "ms-appx:///Assets/Achievent/"+Img,
                                    }
                                }
                            },
                            new AdaptiveSubgroup()
                            {
                                HintWeight = 1
                            }
                        }
                    },
                    new AdaptiveText()
                    {
                        Text = localLang.GetString("App"),
                        HintStyle = AdaptiveTextStyle.Title,
                        HintAlign = AdaptiveTextAlign.Center
                    },
                    new AdaptiveText()
                    {
                        Text = Name,
                        HintStyle = AdaptiveTextStyle.SubtitleSubtle,
                        HintAlign = AdaptiveTextAlign.Center
                    }
                }
                        }
                    }
                }
            };

            // Create the tile notification
            var tileNotif = new TileNotification(tileContent.GetXml());
            
            // And send the notification to the primary tile
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotif);

        }
        static   ResourceLoader localLang=ResourceLoader.GetForCurrentView();


        private static int _clickTime;
        public static int ClickTime
        {
            get
            {
                return local.Read("ClickTime", 0); ;
            }
            set
            {
                local.Save("ClickTime", value);
                _clickTime = value;
                if (value > 10)
                {
                    GenerateToastContentAnPop("Click (Green Hand)", "IClickB.png", "Click more than 10 times");

                }
                else if (value > 100)
                {
                    GenerateToastContentAnPop("Click (Expert)", "IClickL.png", "Click more than 100 times");
                }
                else if (value > 1000)
                {
                    GenerateToastContentAnPop("Click (Master)", "IClickG.png", "Click more than 1000 times");
                }

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
                if (value > 10)
                {
                    var ss = localLang.GetString("LostB");
                    var sc = localLang.GetString("LostBc");
                    if (ss == String.Empty)
                    {
                        ss = "Die more than 10 times";
                        sc = "Bad Luck(Green Hand)";
                    }
                    GenerateToastContentAnPop(sc, "LostB.png", ss);

                }
                else if (value > 100)
                {
                    var ss = localLang.GetString("LostL");
                    var sc = localLang.GetString("LostLc");
                    if (ss == String.Empty)
                    {
                        ss = "Die more than 100 times";
                        sc = "Super Bad Luck(Expert)";
                    }
                    GenerateToastContentAnPop(sc, "LostL.png", ss);
                }
                else if (value > 1000)
                {
                    var ss = localLang.GetString("LostG");
                    var sc = localLang.GetString("LostGc");
                    if (ss == String.Empty)
                    {
                        ss = "Die more than 1000 times";
                        sc = "Bad Luck..........(Master)";
                    }
                    GenerateToastContentAnPop(sc, "LostG.png", ss);
                }
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
                if (value > 10)
                {
                    GenerateToastContentAnPop("Winner (Green Hand)", "WinnerB.png", "Win more than 10 times");

                }
                else if (value > 100)
                {
                    GenerateToastContentAnPop("Winner (Expert)", "WinnerL.png", "Win more than 100 times");
                }
                else if (value > 1000)
                {
                    GenerateToastContentAnPop("Winner (Master)", "WinnerG.png", "Win more than 1000 times");
                }
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
                //if (value > 10)
                //{
                //    GenerateToastContentAnPop("(Green Hand)", "WinnerB.png", "Win more than 10 times");

                //}
                //else if (value > 100)
                //{
                //    GenerateToastContentAnPop("Winner (Expert)", "WinnerL.png", "Win more than 100 times");
                //}
                //else if (value > 1000)
                //{
                //    GenerateToastContentAnPop("Winner (Master)", "WinnerG.png", "Win more than 1000 times");
                //}
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
                return local.Read<DateTimeOffset>("SinglePlayTime", new DateTimeOffset(0, TimeSpan.Zero)); ;
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
                if (value.Offset.TotalMinutes > 10)
                {
                    GenerateToastContentAnPop("Play(Green Hand)", "PlayB.png", "Play more than 10 Minutes");

                }
                else if (value.Offset.TotalMinutes > 100)
                {
                    GenerateToastContentAnPop("Play(Expert)", "PlayL.png", "Play more than 100 Minutes");
                }
                else if (value.Offset.TotalMinutes > 1000)
                {
                    GenerateToastContentAnPop("Play(Master)", "PlayG.png", "Play more than 1000 Minutes");
                }
            }
        }



    }
}
