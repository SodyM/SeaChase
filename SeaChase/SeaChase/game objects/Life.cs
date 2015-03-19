using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SeaChase.game_objects
{
    /// <summary>
    /// Lifelist
    /// </summary>
    class LifeList : UiObject
    {        
        List<Rectangle> drawRectangles;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">Content manager</param>
        public LifeList(ContentManager content)
        {
            sprite = content.Load<Texture2D>(AssetsConstantNames.LIFE_IMAGE);
            drawRectangles = new List<Rectangle>(GameConstants.LIFE_COUNT);
            for (int i = 1; i <= GameConstants.LIFE_COUNT; i++)
            {
                Rectangle rect = new Rectangle(50 * i - sprite.Width / 2,
                                          GameConstants.WINDOW_HEIGHT - 18 - sprite.Height / 2,
                                          sprite.Width - 7, sprite.Height - 7);                
                drawRectangles.Add(rect);
            }            
        }

        /// <summary>
        /// Update lifelist
        /// </summary>
        /// <param name="gameTime">Gametime</param>
        /// <returns>false = no more lifes left</returns>
        public new bool Update(GameTime gameTime)
        {
            if (drawRectangles.Count > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Draws lifelist
        /// </summary>
        /// <param name="spritebatch">Spritebatch</param>
        /// <param name="gameTime">Gametime</param>
        public override void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            for (int i = 0; i < drawRectangles.Count; i++)
            {
                drawRectangle = drawRectangles[i];
                base.Draw(spritebatch, gameTime);
            }
        }

        /// <summary>
        /// Removes life from list
        /// </summary>
        public void RemoveLife()
        {
            if (drawRectangles.Count > 0)
                drawRectangles.RemoveAt(0);
        }
    }
}