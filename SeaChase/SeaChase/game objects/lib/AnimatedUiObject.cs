using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SeaChase.game_objects.lib
{
    /// <summary>
    /// Base class for all animated objects
    /// </summary>
    class AnimatedUiObject : UiObject
    {
        public int NumFrames;        
        public int FrameTime;
        public int CurrentFrame;
        public int ElapsedFrameTime = 0;
        public Rectangle SourceRectangle;        

        /// <summary>
        /// Standard constructor
        /// </summary>
        public AnimatedUiObject()
        {
        }

        /// <summary>
        /// Constructor with extra parameters for animation
        /// </summary>
        /// <param name="numFrames">Count of all frames</param>
        /// <param name="width">Width of one frame</param>
        /// <param name="height">Height of one frame</param>
        /// <param name="frameTime">Framerate time - speed of animation play</param>
        public AnimatedUiObject(int numFrames, int width, int height, int frameTime)
        {
            base.Width = width;
            base.Height = height;

            NumFrames = numFrames;            
            FrameTime = frameTime;
        }

        /// <summary>
        /// Sets correct frame for animation
        /// </summary>
        /// <param name="frameNumber">Framenumber</param>
        public void SetSourceRectangleLocation(int frameNumber, int width)
        {
            SourceRectangle.X = frameNumber * width;
        }

        /// <summary>
        /// Draw animated object
        /// </summary>
        /// <param name="spritebatch">Spritebatch</param>
        /// <param name="gameTime">Gametime</param>
        public override void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            if (IsActive)
            {
                spritebatch.Draw(sprite, drawRectangle, SourceRectangle, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0);
            }            
        }
    }
}