using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SeaChase.game_objects.lib;

namespace SeaChase.game_objects
{
    /// <summary>
    /// Explosion
    /// </summary>
    class Explosion : AnimatedUiObject
    {                
        /// <summary>
        /// Explosion - simple animation
        /// </summary>
        /// <param name="content">Content manager</param>
        /// <param name="x">x location for the center of the explosion</param>
        /// <param name="y">y location for the center of the explosion</param>
        public Explosion(ContentManager content, int x, int y)
            :base(10, 96, 64, 60)
        {
            sprite = content.Load<Texture2D>(AssetsConstantNames.DEPTH_EXPLOSION_IMAGE);
            drawRectangle.X = x - Width / 2;
            drawRectangle.Y = y - Height / 2;
            drawRectangle.Width = Width;
            drawRectangle.Height = Height;

            // reset tracking values
            ElapsedFrameTime = 0;
            CurrentFrame = 0;

            SourceRectangle = new Rectangle(0, 0, Width, Height);
        }

        /// <summary>
        /// Updates the explosion. This only has an effect if the explosion animation is active
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (IsActive)
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
                        SetSourceRectangleLocation(CurrentFrame);
                    }
                    else
                    {
                        // reached the end of the animation
                        IsActive = false;
                    }
                }
            }
        }

        /// <summary>
        /// Sets correct frame for animation
        /// </summary>
        /// <param name="frameNumber">Framenumber</param>
        private void SetSourceRectangleLocation(int frameNumber)
        {
            SourceRectangle.X = frameNumber * Width;
        }
    }
}