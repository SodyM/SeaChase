using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SeaChase.game_objects.lib;

namespace SeaChase.game_objects
{
    /// <summary>
    /// Treasure
    /// </summary>
    class Treasure : AnimatedUiObject
    {
        Random rndY = new Random();
        Random rndX = new Random();
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">Content manager</param>
        public Treasure(ContentManager content)
            :base(4, 50, 20, 500)
        {
            sprite = content.Load<Texture2D>(AssetsConstantNames.TREASURE_STAR_IMAGE);
            Collision_Offset = 5;
            PutTreasureOnNewPosition();
        }

        /// <summary>
        /// Updates treasure
        /// </summary>
        /// <param name="gameTime">Gametime</param>
        public override void Update(GameTime gameTime)
        {
            if (!IsActive)
            {
                PutTreasureOnNewPosition();
            }
            else
            {
                // check for advancing animation frame
                ElapsedFrameTime += gameTime.ElapsedGameTime.Milliseconds;
                if (ElapsedFrameTime > FrameTime)
                {
                    // reset frame timer
                    ElapsedFrameTime = 0;

                    // advance the animation
                    if (CurrentFrame < NumFrames - 1)
                    {
                        CurrentFrame++;
                        SetSourceRectangleLocation(CurrentFrame, 50);
                    }
                    else
                    {
                        CurrentFrame = 0;
                        SetSourceRectangleLocation(CurrentFrame, 50);
                    }
                }
            }
        }
        
        /// <summary>
        /// Puts treasure on new dynamicaly generated position
        /// </summary>
        void PutTreasureOnNewPosition()
        {            
            List<int> columnYCoordrinates = new List<int>()
            {
                308,
                408,
                515
            };

            List<int> lineXCoordinates = new List<int>()
            {
                135,
                270,
                405,
                540
            };

            // randomly generate new position
            int randomXIndex = this.rndX.Next(lineXCoordinates.Count);
            int randomYIndex = this.rndY.Next(columnYCoordrinates.Count);            

            drawRectangle.X = lineXCoordinates[randomXIndex] - Width / 2;
            drawRectangle.Y = columnYCoordrinates[randomYIndex] - Height / 2;
            drawRectangle.Width = Width;
            drawRectangle.Height = Height;

            // reset animation
            // reset tracking values
            ElapsedFrameTime = 0;
            CurrentFrame = 0;

            Rectangle x = SourceRectangle;
            x.Width = 50;
            x.Height = 25;

            SourceRectangle = x;
            
            // set new collision rectangle
            collisionRectangle = new Rectangle((int)drawRectangle.X - Collision_Offset,
                                                (int)drawRectangle.Y - Collision_Offset,
                                                sprite.Width - Collision_Offset,
                                                sprite.Height - Collision_Offset);
            // and activate treasure
            IsActive = true;
        }
    }
}
