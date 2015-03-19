using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SeaChase.game_objects
{   
    /// <summary>
    /// Oxymeter - small oxygene bar
    /// </summary>
    class Oxymeter : UiObject
    {
        int countdownTimer = 0;        

        // inner part of lifemeter - simple color
        Texture2D life;
        int actualValue = 0;

        // frame of lifemeter
        Vector2 frameVector;
        Texture2D frame;
        ContentManager content;

        /// <summary>
        /// Creates lifetimer with frame and color
        /// </summary>
        /// <param name="content">Contentmanager</param>
        /// <param name="graphicsDevice">Graphicsdevice</param>
        public Oxymeter(ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.content = content;
            frame = content.Load<Texture2D>(AssetsConstantNames.LIFEMETER_IMAGE);
            frameVector = new Vector2(475, 575);

            life = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            life.SetData<Color>(new Color[] { Color.Red });
            actualValue = GameConstants.MAXVALUE;
        }

        /// <summary>
        /// Updates lifetemer
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        /// <returns>false = no more oxygene...die</returns>
        public new bool Update(GameTime gameTime)
        {
            countdownTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (countdownTimer >= GameConstants.COUNTDOWNINTERVAL)
            {
                countdownTimer = 0;
                actualValue--;
            }

            if (actualValue > 0)
            {
                return true;
            }
            else
            {
                actualValue = GameConstants.MAXVALUE;
                return false;               // no more oxygene      
            }
        }

        /// <summary>
        /// Adds extra time after player finds treasure
        /// </summary>
        public void AddTime()
        {
            int maxAddTime = 10;
            if (actualValue + maxAddTime <= GameConstants.MAXVALUE)
            {
                actualValue += maxAddTime;
            }
            else
            {
                actualValue = GameConstants.MAXVALUE;
            }            
        }

        /// <summary>
        /// Draw lifemeter
        /// </summary>
        /// <param name="spriteBatch">Spritebatch</param>
        /// <param name="gameTime">Gametime</param>
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(life, new Rectangle((int)frameVector.X, (int)frameVector.Y, actualValue, GameConstants.HEIGHT), Color.White);
            spriteBatch.Draw(frame, frameVector, Color.White);
        }
    }
}