using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SeaChase.game_objects.lib;

namespace SeaChase.game_objects
{
    /// <summary>
    /// Seavawe
    /// </summary>
    class SeaWave : AnimatedUiObject
    {               
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">Contentmanager</param>
        public SeaWave(ContentManager content)
            :base(5, 1153, 22, 75)
        {
            sprite = content.Load<Texture2D>(AssetsConstantNames.WAVE_IMAGE);
            drawRectangle = new Rectangle(0, 170, Width, Height);
            SourceRectangle.X = 0;
            SourceRectangle.Y = 0;
            SourceRectangle.Width = Width;
            SourceRectangle.Height = Height;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="gameTime">Gametime</param>
        public override void Update(GameTime gameTime)
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
                }
                else
                {
                    // reached the end of the animation
                    CurrentFrame = 0;
                }

                SetSourceRectangleLocation(CurrentFrame);
            }
        }
        
        /// <summary>
        /// Sets next frame
        /// </summary>
        /// <param name="frameNumber">frameNumber</param>
        private void SetSourceRectangleLocation(int frameNumber)
        {
            SourceRectangle.Y = frameNumber * Height;
        }
    }
}
