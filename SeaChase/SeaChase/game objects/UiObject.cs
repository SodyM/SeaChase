using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SeaChase.game_objects
{
    /// <summary>
    /// Base object for all ui elements
    /// </summary>
    class UiObject
    {
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


        public virtual void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            spritebatch.Draw(sprite, drawRectangle, Color.White);
        }
    }
}
