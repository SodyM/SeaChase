using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeaChase.game_objects.lib
{
    class MovingUiObject : UiObject
    {
        public Vector2 drawVector;
        public Vector2 velocity;

        public bool moveToLeft;
        public float Speed;
        
        /// <summary>
        /// Updates destroyer
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Update(GameTime gameTime)
        {
            // prehod smer pohybu lodi
            if (drawVector.X > GameConstants.WINDOW_WIDTH - sprite.Width)
            {
                moveToLeft = false;
            }
            else if (drawVector.X < 0)
            {
                moveToLeft = true;
            }

            if (drawVector.X > GameConstants.WINDOW_WIDTH - sprite.Width)
            {
                moveToLeft = false;
            }
            else if (drawVector.X < 0)
            {
                moveToLeft = true;
            }


            if (moveToLeft)
            {
                drawVector.X += Speed;
            }
            else
            {
                drawVector.X += -1 * Speed;
            }

            collisionRectangle.X = (int)drawVector.X;
            collisionRectangle.Y = (int)drawVector.Y;
        }        

        /// <summary>
        /// Draws destroyer
        /// </summary>
        /// <param name="spritebatch">SpriteBatch</param>
        /// <param name="gameTime">GameTime</param>
        public override void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            SpriteEffects effect = SpriteEffects.None;
            if (moveToLeft)
                effect = SpriteEffects.None;
            else
                effect = SpriteEffects.FlipHorizontally;
            
            spritebatch.Draw(sprite, drawVector, null, Color.White, 0.0f, Vector2.Zero, 1.0f, effect, 0.0f);
        }
    }
}
