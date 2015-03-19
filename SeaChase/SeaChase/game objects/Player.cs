using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using SeaChase.game_objects.lib;

namespace SeaChase.game_objects
{
    /// <summary>
    /// Player
    /// </summary>
    class Player : MovingUiObject
    {
        bool spacePressed = false;
        bool spaceReleased = true;        
        int fireTorpedoTimer = 0;
        bool torpedoReady = true;
        
        ContentManager content;         // kvuli propojeni na torpedo
        SoundBank soundBank;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">Contentmanager</param>
        /// <param name="soundBank">Soundbank</param>
        public Player(ContentManager content, SoundBank soundBank)
        {
            this.content = content;
            this.soundBank = soundBank;

            sprite = content.Load<Texture2D>(AssetsConstantNames.SUBMARINE_IMAGE);
            SetPlayerToStartPosition();
        }

        /// <summary>
        /// Sets player start position
        /// </summary>
        public void SetPlayerToStartPosition()
        {
            drawRectangle = new Rectangle(GameConstants.WINDOW_WIDTH / 2 - sprite.Width / 2,
                                            GameConstants.WINDOW_HEIGHT - 100,
                                            sprite.Width,
                                            sprite.Height);

            drawVector = new Vector2(drawRectangle.X, drawRectangle.Y);
            collisionRectangle = new Rectangle(drawRectangle.X, drawRectangle.Y, drawRectangle.Width, drawRectangle.Height);
        }

        /// <summary>
        /// Updates player
        /// </summary>
        /// <param name="gameTime">Gametime</param>
        /// <param name="keyboardState">Keyboardstate</param>
        public bool Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (!torpedoReady)
            {
                fireTorpedoTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (fireTorpedoTimer > GameConstants.FIRE_DELAY)
                {
                    torpedoReady = true;
                    fireTorpedoTimer = 0;
                }
            }            

            if (keyboardState.IsKeyDown(Keys.Up))
                Up();

            if (keyboardState.IsKeyDown(Keys.Down))
                Down();

            if (keyboardState.IsKeyDown(Keys.Left))
            {               
                Left();
            }
            if (keyboardState.IsKeyDown(Keys.Right))
                Right();

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                spacePressed = true;
                spaceReleased = false;
            }
            else if (keyboardState.IsKeyUp(Keys.Space))
            {
                spaceReleased = true;
                if (spacePressed && spaceReleased)
                {
                    FireTorpedo();
                    spaceReleased = false;
                    spacePressed = false;
                }
            }

            drawVector.Y += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            drawVector.X += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            velocity *= GameConstants.VELOCITY_SPEED;

            // koriguj pohyb na ose x
            if (drawVector.X < -1 * sprite.Width)
            {
                drawVector.X = GameConstants.WINDOW_WIDTH;
            }
            else if (drawVector.X > GameConstants.WINDOW_WIDTH)
            {
                drawVector.X = -1 * sprite.Width;
            }

            // koriguj pohyb na ose y
            if (drawVector.Y < GameConstants.SURFACE_SEA_LINE)
            {
                drawVector.Y = GameConstants.SURFACE_SEA_LINE;
            }
            else if (drawVector.Y > GameConstants.BOTTOM_SEA_LINE)
            {
                drawVector.Y = GameConstants.BOTTOM_SEA_LINE;
                return true;
            }

            collisionRectangle.X = (int)drawVector.X;
            collisionRectangle.Y = (int)drawVector.Y;
            return false;
        }

        
        #region movement + basic fyzic
        void Up()
        {
            velocity.Y -= GameConstants.VELOCITY_STEP;
        }

        void Down()
        {
            velocity.Y += GameConstants.VELOCITY_STEP;
        }

        void Left()
        {
            moveToLeft = true;
            velocity.X -= GameConstants.VELOCITY_STEP;
        }

        void Right()
        {
            moveToLeft = false;
            velocity.X += GameConstants.VELOCITY_STEP;
        }
        #endregion

        /// <summary>
        /// Fires torpedo
        /// </summary>
        void FireTorpedo()
        {
            if (torpedoReady)
            {
                Vector2 torpedoVector = new Vector2();
                if (moveToLeft)
                    torpedoVector.X = drawVector.X;
                else
                    torpedoVector.X = drawVector.X + sprite.Width / 2;

                torpedoVector.Y = drawVector.Y + sprite.Height / 2 + 4;

                SeaChaseGame.AddTorpedo(new Torpedo(content, torpedoVector, moveToLeft));

                soundBank.PlayCue(AssetsConstantNames.FIRE_SOUND);
                torpedoReady = false;
            }
        }
    }
}