using System;
using System.Collections.Generic;

namespace SnapToFloor
{
    public static class SnapToFloorSettings
    {
        public static List<string> GetLanguageScript(KLanguage kLanguage)
        {
            switch (kLanguage)
            {
                case KLanguage.English:
                    return new List<string>()
                    {
                        "Always",
                        "Never",
                    };
                case KLanguage.한국어:
                    return new List<string>()
                    {
                        "항상",
                        "끄기",
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(kLanguage), kLanguage, null);
            }
        }

        public enum KSnapMode
        {
            Mode3D,
            Mode2D
        }

        public enum KLanguage
        {
            English,
            한국어,
        }

        public enum KStartAtShow
        {
            Always,
            Never,
        }
    }
}