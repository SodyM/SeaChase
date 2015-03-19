using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SeaChase.game_objects.lib;

namespace SeaChase.game_objects
{
    /// <summary>
    /// Torpedo
    /// </summary>
    public class Torpedo : UiObject
    {
        Vector2 vector;
        const float SPEED = 4.0f;
        bool faceLeft;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">ContentManager</param>
        /// <param name="vector">Startposition as Vector2</param>
        /// <param name="faceLeft">Facing of torpedo. True = torpedo is facing left</param>
        public Torpedo(ContentManager content, Vector2 vector, bool faceLeft)
        {
            sprite = content.Load<Texture2D>(AssetsConstantNames.TORPEDO_IMAGE);
            base.IsActive = true;
            this.vector = vector;
            this.faceLeft = faceLeft;

            collisionRectangle = new Rectangle((int)vector.X, (int)vector.Y, sprite.Width, sprite.Height);
        }

        /// <summary>
        /// Update torpedo
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Update(GameTime gameTime)
        {
            // move fired torpedo
            if (faceLeft)
                vector.X -= SPEED;
            else
                vector.X += SPEED;

            collisionRectangle.X = (int)vector.X;

            // deactivate torpedo if it leaves screen
            if (vector.X < -1 * sprite.Width || vector.X > GameConstants.WINDOW_WIDTH + sprite.Width)
            {
                base.IsActive = false;
            }
        }

        /// <summary>
        /// Draw torpedo
        /// </summary>
        /// <param name="spritebatch">SpriteBatch</param>
        /// <param name="gameTime">GameTime</param>
        public override void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            SpriteEffects facing = SpriteEffects.FlipHorizontally;
            if (faceLeft)
                facing = SpriteEffects.None;

            spritebatch.Draw(sprite, vector, null, Color.White, 0, Vector2.Zero, 1.3f, facing, 0);
        }
    }
}