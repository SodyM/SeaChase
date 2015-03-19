using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using SeaChase.game_objects.lib;

namespace SeaChase.game_objects
{
    /// <summary>
    /// Depthcharge
    /// </summary>
    class DepthCharge : AnimatedUiObject
    {        
        int minDetonationDepth;
        int detonationDepth;
        int maxDetonationDepth;
        int dropContext;
        public int DropContext 
        {
            get { return dropContext; }
        }
                
        Vector2 velocity;
        Random rnd = new Random();       
        SoundBank soundBank;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">Contentmanager</param>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <param name="soundBank">Soundbank</param>
        /// <param name="dropContext">Index of dropcontext (just a simple index of column)</param>
        public DepthCharge(ContentManager content, int x, int y, SoundBank soundBank, int dropContext)
            :base(5, 29, 49, 210)
        {
            sprite = content.Load<Texture2D>(AssetsConstantNames.DEPTHCHARGE_IMAGE);
            drawRectangle.X = x - Width / 2;
            drawRectangle.Y = y - Height / 2;
            drawRectangle.Width = Width;
            drawRectangle.Height = Height;

            collisionRectangle = drawRectangle;

            this.soundBank = soundBank;
            this.dropContext = dropContext;

            velocity.Y = 1f;

            minDetonationDepth = y; 
            maxDetonationDepth = GameConstants.WINDOW_HEIGHT - sprite.Height / 2 - 70;
            detonationDepth = rnd.Next(minDetonationDepth, maxDetonationDepth);

            // reset tracking values
            ElapsedFrameTime = 0;
            CurrentFrame = 0;

            SourceRectangle = new Rectangle(0, 0, Width, Height);

            soundBank.PlayCue(AssetsConstantNames.DEPTHCHARGE_SOUND);
        }

        /// <summary>
        /// Updates depthcharge
        /// </summary>
        /// <param name="gameTime">Gametime</param>
        /// <returns>true = depthcharge exploded</returns>
        public new bool Update(GameTime gameTime)
        {
            if (IsActive)
            {
                drawRectangle.Y += (int)velocity.Y;
                collisionRectangle = drawRectangle;

                if (drawRectangle.Y >= detonationDepth || drawRectangle.Y >= maxDetonationDepth)
                {
                    CurrentFrame = 0;
                    ElapsedFrameTime = 0;
                    IsActive = false;
                    return true;
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
                            SetSourceRectangleLocation(CurrentFrame);
                        }
                        else
                        {
                            // reached the end of the animation                            
                            CurrentFrame = 0;
                            ElapsedFrameTime = 0;
                        }
                    }
                }
            }
            return false;
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