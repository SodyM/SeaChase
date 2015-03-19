using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeaChase.game_objects
{
    /// <summary>
    /// AssetsConstantNames
    /// It's the one and only place where all assets names are defined
    /// </summary>
    class AssetsConstantNames
    {
        // graphics
        public static string BACKGROUND_IMAGE                   = "background";
        public static string GAME_OVER_FONT                     = "GameOverFont";
        public static string DEPTHCHARGE_IMAGE                  = "depthcharge";
        public static string DESTROYER_IMAGE                    = "destroyer";
        public static string DEPTH_EXPLOSION_IMAGE              = "deptExplosion";
        public static string LIFE_IMAGE                         = "life";
        public static string MINE_IMAGE                         = "mine";
        public static string LIFEMETER_IMAGE                    = "Lifemeter";
        public static string SUBMARINE_IMAGE                    = "submarine";
        public static string WAVE_IMAGE                         = "background_line";
        public static string TORPEDO_IMAGE                      = "torpedo";
        public static string TREASURE_STAR_IMAGE                = "star";

        // sounds
        public static string AUDIO_ENGINE                       = @"Content\SeaChase.xgs";
        public static string WAVE_BANK                          = @"Content\Wave Bank.xwb";
        public static string SOUND_BANK                         = @"Content\Sound Bank.xsb";

        // sound effects
        public static string AMBIENT_SOUND                      = "ambient";
        public static string EXPLOSION_SOUND                    = "explosion";
        public static string DEPTHCHARGE_SOUND                  = "deptcharge";
        public static string FIRE_SOUND                         = "fire";

        // little helpers
        public static int DROP_CONTENT_X1 = 50;
        public static int DROP_CONTENT_X2 = 195;
        public static int DROP_CONTENT_X3 = 330;
        public static int DROP_CONTENT_X4 = 470;
        public static int DROP_CONTENT_X5 = 600;
        public static int DROP_CONTENT_X6 = 730;
    }
}
