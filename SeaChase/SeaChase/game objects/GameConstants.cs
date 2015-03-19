using Microsoft.Xna.Framework;

namespace SeaChase.game_objects
{
    /// <summary>
    /// All game constants
    /// </summary>
    class GameConstants
    {
        public static int WINDOW_WIDTH = 800;
        public static int WINDOW_HEIGHT = 600;
        public static int LIFE_COUNT = 4;

        public GameStates gameState = GameStates.PLAY;

        // score text
        public static int TOP_LINE = 5;
        public static int BOTTOM_LINE = 40;
        public static Color TEXT_COLOR              = Color.DarkRed;

        // x coordinates of drop columns - destroyer moving right
        public static int RIGHT0                    = 47;
        public static int RIGHT1                    = 200;
        public static int RIGHT2                    = 335;
        public static int RIGHT3                    = 472;
        public static int RIGHT4                    = 610;

        // x coordinates of drop columns - destroyer moving left
        public static int LEFT0                     = 89;
        public static int LEFT1                     = 220;
        public static int LEFT2                     = 357;
        public static int LEFT3                     = 494;
        public static int LEFT4                     = 624;
        
        // start y position for dropped depthcharge               
        public static int DROPBOMB_Y                = 200;

        // oxymeter
        public static int MINVALUE                  = 1;                // minimal value in oxymeter
        public static int MAXVALUE                  = 300;              // maximal value in oxymeter
        public static int HEIGHT                    = 15;               // height of oxymeter
        public static int COUNTDOWNINTERVAL         = 100;              // speed of oxygene lost

        // ui helper for game screen
        public static int SURFACE_SEA_LINE          = 195;              // position of surface line, our submarine can't surface
        public static int BOTTOM_SEA_LINE           = 527;              // bottom line

        public static int FIRE_DELAY                = 1500;

        // velocity
        public static float VELOCITY_STEP = 5.0f;                       // velocity step for acceleration
        public static float VELOCITY_SPEED = 0.98f;                     // simple simulation for physics - losing speed
    }
}