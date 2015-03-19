using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SeaChase.game_objects.lib;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace SeaChase.game_objects
{
    /// <summary>
    /// Torpedoborec
    /// Moznosti:
    /// 1. pohybuje se leva -> prava a zpatky
    /// 2. shazuje hlubinne pumy
    /// </summary>
    class Destroyer : MovingUiObject
    {        
        // konstanty
        const int START_X = 50;
        const int START_Y = 148;
                       
        public float XPosition
        {
            get { return drawVector.X; }
        }
        
        public bool IsMovingRight 
        {
            get { return moveToLeft; }
        }

        ContentManager content;
        SoundBank soundbank;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">Contentmanager</param>
        public Destroyer(ContentManager content, SoundBank soundbank)
        {
            this.content = content;
            this.soundbank = soundbank;

            sprite = content.Load<Texture2D>(AssetsConstantNames.DESTROYER_IMAGE);
            drawVector = new Vector2(START_X - sprite.Width / 2, START_Y);
            Speed = 1.0f;

            collisionRectangle = new Rectangle((int)drawVector.X, (int)drawVector.Y, sprite.Width, sprite.Height);
        }

        public void DropDepthCharge(ref Dictionary<int, bool> dropContextStatusIsInUse, ref List<DepthCharge> depthCharges)
        {
            // drop depth charge
            int destroyerXPosition = (int)XPosition;
            if (IsMovingRight)
            {
                if (destroyerXPosition == GameConstants.RIGHT0)
                {
                    DropBomb(destroyerXPosition, 0, ref dropContextStatusIsInUse, ref depthCharges);
                }
                else if (destroyerXPosition == GameConstants.RIGHT1)
                {
                    DropBomb(destroyerXPosition, 1, ref dropContextStatusIsInUse, ref depthCharges);
                }
                else if (destroyerXPosition == GameConstants.RIGHT2)
                {
                    DropBomb(destroyerXPosition, 2, ref dropContextStatusIsInUse, ref depthCharges);
                }
                else if (destroyerXPosition == GameConstants.RIGHT3)
                {
                    DropBomb(destroyerXPosition, 3, ref dropContextStatusIsInUse, ref depthCharges);
                }
                else if (destroyerXPosition == GameConstants.RIGHT4)
                {
                    DropBomb(destroyerXPosition, 4, ref dropContextStatusIsInUse, ref depthCharges);
                }
            }
            else
            {
                if (destroyerXPosition == GameConstants.LEFT4)
                {
                    DropBomb(destroyerXPosition + sprite.Width, 5, ref dropContextStatusIsInUse, ref depthCharges);
                }
                else if (destroyerXPosition == GameConstants.LEFT3)
                {
                    DropBomb(destroyerXPosition + sprite.Width, 4, ref dropContextStatusIsInUse, ref depthCharges);
                }
                else if (destroyerXPosition == GameConstants.LEFT2)
                {
                    DropBomb(destroyerXPosition + sprite.Width, 3, ref dropContextStatusIsInUse, ref depthCharges);
                }
                else if (destroyerXPosition == GameConstants.LEFT1)
                {
                    DropBomb(destroyerXPosition + sprite.Width, 2, ref dropContextStatusIsInUse, ref depthCharges);
                }
                else if (destroyerXPosition == GameConstants.LEFT0)
                {
                    DropBomb(destroyerXPosition + sprite.Width, 1, ref dropContextStatusIsInUse, ref depthCharges);
                }
            }
        }

        void DropBomb(int xPosition, int dropColumnIndex, ref Dictionary<int, bool> dropContextStatusIsInUse, ref List<DepthCharge> depthCharges)
        {
            if (!dropContextStatusIsInUse[dropColumnIndex])
            {
                depthCharges.Add(new DepthCharge(content, xPosition, GameConstants.DROPBOMB_Y, soundbank, dropColumnIndex));
                dropContextStatusIsInUse[dropColumnIndex] = true;
            }
        }
    }
}