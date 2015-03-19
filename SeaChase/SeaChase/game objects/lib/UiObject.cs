using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SeaChase.game_objects
{
    /// <summary>
    /// Base object for all ui elements
    /// </summary>
    public class UiObject
    {
        public int Collision_Offset = 0;

        public int Width;
        public int Height;

        bool isActive = true;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        protected Rectangle collisionRectangle;
        public Rectangle CollisionRectangle
        {
            get { return collisionRectangle; }
        }

        protected Texture2D sprite;
        protected Rectangle drawRectangle;


        /// <summary>
        /// Basic Updatehandler
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Basic Draw method
        /// </summary>
        /// <param name="spritebatch">SpriteBatch</param>
        /// <param name="gameTime">GameTime</param>
        public virtual void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            spritebatch.Draw(sprite, drawRectangle, Color.White);
        }        
    }
}