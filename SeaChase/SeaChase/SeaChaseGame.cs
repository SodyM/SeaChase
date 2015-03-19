using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SeaChase.game_objects;
using System;

namespace SeaChase
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SeaChaseGame : Microsoft.Xna.Framework.Game
    {
        // game objects
        SeaWave seaWave;
        Oxymeter lifemeter;
        Treasure treasure;
        Player player;
        Destroyer destroyer;
        MineField mineField;
        LifeList lifeList;
        List<Explosion> explosions;
        List<Color> explosionColors = new List<Color>();
        List<Color> backgroundColors = new List<Color>();
        List<DepthCharge> depthCharges;
        static List<Torpedo> torpedos;
        int score = 0;

        // technical objects & helper
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Color mainColorTheme = Color.White;
        Color backupColor = Color.White;
        GameStates gameState = GameStates.PLAY;
        Texture2D backgroudSprite;
        Rectangle backroundRectangle;        
        List<Rectangle> dropContextRectangles = new List<Rectangle>();

        // audio support
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;

        // text support
        SpriteFont font;

        bool playExplosion = false;        
        int DELAY = 18;
        int MAXREPEATCOUNT = 20;
        int repearCounter = 0;
        int actualTicks = 0;
        Color actualColor = Color.Pink;
        int colorIndex = 0;
             
        Dictionary<int, bool> dropContextStatusIsInUse = new Dictionary<int, bool>();

        /// <summary>
        /// Constructor
        /// </summary>
        public SeaChaseGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GameConstants.WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = GameConstants.WINDOW_HEIGHT;

            GenerateDropContexts();
            InitExplosionColors();

            Content.RootDirectory = "Content";           
        }

        /// <summary>
        /// Initialiye explosion spectrum colors
        /// </summary>
        void InitExplosionColors()
        {
            explosionColors.Add(Color.Red);
            explosionColors.Add(Color.OrangeRed);
            explosionColors.Add(Color.Orange);
            explosionColors.Add(Color.Yellow);
            explosionColors.Add(Color.LightYellow);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroudSprite = Content.Load<Texture2D>(AssetsConstantNames.BACKGROUND_IMAGE);
            backroundRectangle = new Rectangle(0, 0, backgroudSprite.Width, backgroudSprite.Height);
            GenerateBackgroundColor();

            // sound support
            audioEngine = new AudioEngine(AssetsConstantNames.AUDIO_ENGINE);
            waveBank = new WaveBank(audioEngine, AssetsConstantNames.WAVE_BANK);
            soundBank = new SoundBank(audioEngine, AssetsConstantNames.SOUND_BANK);
            soundBank.PlayCue(AssetsConstantNames.AMBIENT_SOUND);

            // load font
            font = Content.Load<SpriteFont>(AssetsConstantNames.GAME_OVER_FONT);

            // create game objects
            torpedos = new List<Torpedo>();
            depthCharges = new List<DepthCharge>();
            explosions = new List<Explosion>();
            player = new Player(Content, soundBank);
            destroyer = new Destroyer(Content, soundBank);
            mineField = new MineField(Content);
            lifeList = new LifeList(Content);
            treasure = new Treasure(Content);
            lifemeter = new Oxymeter(Content, GraphicsDevice);
            seaWave = new SeaWave(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }
      
        #region Update methods
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (gameState == GameStates.PLAY)
            {   
                // ?????
                if (!lifemeter.Update(gameTime))
                {
                    playExplosion = true;
                    lifeList.RemoveLife();
                }

                UpdateSeaWave(gameTime);
                UpdatePlayer(gameTime, keyboardState);
                UpdateDestroyer(gameTime);                
                UpdateTorpedoCollisions(gameTime);
                UpdatePlayerCollisions();
                UpdateMineCollisions();
                UpdateTreasure(gameTime);
                UpdateExplosions(gameTime);
                UpdateDepthcharges(gameTime);
                UpdateLifelist(gameTime);
                UpdateExplosionScreen(gameTime);
                

                // cleanup
                RemoveInactiveObjects();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Update player submarine
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        /// <param name="keyboardState">KeyboardState</param>
        /// <returns></returns>
        private void UpdatePlayer(GameTime gameTime, KeyboardState keyboardState)
        {
            bool killed = player.Update(gameTime, keyboardState);
            if (killed)
            {
                playExplosion = true;
                lifeList.RemoveLife();
                player.SetPlayerToStartPosition();
            }
        }

        /// <summary>
        /// Update upper seawave
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        private void UpdateSeaWave(GameTime gameTime)
        {
            seaWave.Update(gameTime);
        }

        /// <summary>
        /// Update explosion screen - blinking with few colors
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        private void UpdateExplosionScreen(GameTime gameTime)
        {
            if (playExplosion)
            {
                actualTicks += gameTime.ElapsedGameTime.Milliseconds;
                if (actualTicks > DELAY)
                {
                    if (repearCounter <= MAXREPEATCOUNT)
                    {
                        mainColorTheme = explosionColors[colorIndex];
                        colorIndex++;
                        if (colorIndex + 1 > explosionColors.Count)
                        {
                            colorIndex = 0;
                        }
                        actualTicks = 0;
                        repearCounter++;
                    }
                    else
                    {
                        mainColorTheme = backupColor;
                        repearCounter = 0;
                        playExplosion = false;
                    }
                }
            }
        }

        /// <summary>
        /// Update lifelist indicator
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        private void UpdateLifelist(GameTime gameTime)
        {
            if (!lifeList.Update(gameTime))
            {
                gameState = GameStates.GAME_OVER;
                Cue backgroundCue = soundBank.GetCue(AssetsConstantNames.AMBIENT_SOUND);
                backgroundCue.Stop(AudioStopOptions.Immediate);
            }
        }

        /// <summary>
        /// Update existing depth charges
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        private void UpdateDepthcharges(GameTime gameTime)
        {
            // depth charges
            foreach (var depthCharge in depthCharges)
            {
                bool explode = depthCharge.Update(gameTime);
                if (explode)
                {
                    dropContextStatusIsInUse[depthCharge.DropContext] = false;
                    explosions.Add(new Explosion(Content, depthCharge.CollisionRectangle.Center.X, depthCharge.CollisionRectangle.Center.Y));
                    soundBank.PlayCue(AssetsConstantNames.EXPLOSION_SOUND);
                }

                if (depthCharge.CollisionRectangle.Intersects(player.CollisionRectangle))
                {
                    dropContextStatusIsInUse[depthCharge.DropContext] = false;
                    explosions.Add(new Explosion(Content, depthCharge.CollisionRectangle.Center.X, depthCharge.CollisionRectangle.Center.Y));
                    soundBank.PlayCue(AssetsConstantNames.EXPLOSION_SOUND);

                    playExplosion = true;
                    lifeList.RemoveLife();
                    player.SetPlayerToStartPosition();
                }
            }
        }

        /// <summary>
        /// Update existing explosions
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        private void UpdateExplosions(GameTime gameTime)
        {
            foreach (var explosion in explosions)
            {
                explosion.Update(gameTime);
                if (explosion.CollisionRectangle.Intersects(player.CollisionRectangle))
                {
                    playExplosion = true;
                    lifeList.RemoveLife();
                }
            }
        }

        /// <summary>
        /// Update treasure object
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        private void UpdateTreasure(GameTime gameTime)
        {
            treasure.Update(gameTime);
        }

        /// <summary>
        /// Update destroyer
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateDestroyer(GameTime gameTime)
        {
            destroyer.Update(gameTime);
            destroyer.DropDepthCharge(ref dropContextStatusIsInUse, ref depthCharges);
        }

        /// <summary>
        /// Update collision with mine
        /// </summary>
        void UpdateMineCollisions()
        {
            // check for collision submarine vs mine
            foreach (var mine in mineField.MineList)
            {
                if (mine.IsActive)
                {
                    if (player.CollisionRectangle.Intersects(mine.CollisionRectangle))
                    {
                        playExplosion = true;
                        mine.IsActive = false;
                        soundBank.PlayCue(AssetsConstantNames.EXPLOSION_SOUND);
                        lifeList.RemoveLife();
                        explosions.Add(new Explosion(Content, mine.CollisionRectangle.Center.X, mine.CollisionRectangle.Center.Y));
                    }
                }
            }

        }

        /// <summary>
        /// Update player collisions
        /// </summary>
        void UpdatePlayerCollisions()
        {
            // check for collisions player vs destroyer
            if (player.CollisionRectangle.Intersects(destroyer.CollisionRectangle))
            {
                soundBank.PlayCue(AssetsConstantNames.EXPLOSION_SOUND);
                lifeList.RemoveLife();
                explosions.Add(new Explosion(Content, player.CollisionRectangle.Center.X, player.CollisionRectangle.Center.Y));
            }

            if (player.CollisionRectangle.Intersects(treasure.CollisionRectangle))
            {
                if (treasure.IsActive)
                {
                    lifemeter.AddTime();
                    score += 100;
                    treasure.IsActive = false;
                }
            }
        }

        /// <summary>
        /// Update torpedo collisions
        /// </summary>
        /// <param name="gameTime"></param>
        void UpdateTorpedoCollisions(GameTime gameTime)
        {
            foreach (var torpedo in torpedos)
            {
                // check for collisions torpedo vs destroyer
                if (torpedo.IsActive)
                {
                    if (destroyer.CollisionRectangle.Intersects(torpedo.CollisionRectangle))
                    {
                        soundBank.PlayCue(AssetsConstantNames.EXPLOSION_SOUND);
                        torpedo.IsActive = false;

                        explosions.Add(new Explosion(Content, destroyer.CollisionRectangle.Center.X, destroyer.CollisionRectangle.Center.Y));
                    }

                    # region player can destroy mines with torpedo ?
                    foreach (var mine in mineField.MineList)
                    {
                        if (torpedo.CollisionRectangle.Intersects(mine.CollisionRectangle))
                        {
                            torpedo.IsActive = false;
                            mine.IsActive = false;
                            soundBank.PlayCue(AssetsConstantNames.EXPLOSION_SOUND);
                            explosions.Add(new Explosion(Content, mine.CollisionRectangle.Center.X, mine.CollisionRectangle.Center.Y));
                        }
                    }
                    #endregion

                    foreach (var charge in depthCharges)
                    {
                        if (torpedo.CollisionRectangle.Intersects(charge.CollisionRectangle))
                        {
                            torpedo.IsActive = false;
                            charge.IsActive = false;
                            soundBank.PlayCue(AssetsConstantNames.EXPLOSION_SOUND);
                            explosions.Add(new Explosion(Content, charge.CollisionRectangle.Center.X, charge.CollisionRectangle.Center.Y));
                        }
                    }

                    torpedo.Update(gameTime);
                }
            }
        }

        #endregion
                
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(backgroudSprite, backroundRectangle, mainColorTheme);
            treasure.Draw(spriteBatch, gameTime);

            // draw score infos
            spriteBatch.DrawString(font, "SCORE :", new Vector2(100, GameConstants.TOP_LINE), GameConstants.TEXT_COLOR);
            string scoreText = String.Format("{0:000000}", score);

            spriteBatch.DrawString(font, scoreText, new Vector2(100, GameConstants.BOTTOM_LINE), GameConstants.TEXT_COLOR);
            spriteBatch.DrawString(font, "BONUS :", new Vector2(520, GameConstants.TOP_LINE), GameConstants.TEXT_COLOR);
            spriteBatch.DrawString(font, "000000", new Vector2(520, GameConstants.BOTTOM_LINE), GameConstants.TEXT_COLOR);

            if (gameState == GameStates.PLAY)
            {
                lifemeter.Draw(spriteBatch, gameTime);
                destroyer.Draw(spriteBatch, gameTime);
                seaWave.Draw(spriteBatch, gameTime);
                player.Draw(spriteBatch, gameTime);
                
                foreach (var torpedo in torpedos)
                {
                    torpedo.Draw(spriteBatch, gameTime);
                }
                
                // explosions
                foreach (var explosion in explosions)
                {
                    explosion.Draw(spriteBatch, gameTime);
                }

                // depth charges
                foreach (var depthCharge in depthCharges)
                {
                    depthCharge.Draw(spriteBatch, gameTime);
                }

                
                mineField.Draw(spriteBatch, gameTime);
                lifeList.Draw(spriteBatch, gameTime);                
            }
            else if (gameState == GameStates.GAME_OVER)
            {                
                spriteBatch.DrawString(font,  "G A M E   O V E R", 
                                        new Vector2(GameConstants.WINDOW_WIDTH / 2 - 170, 
                                        GameConstants.WINDOW_HEIGHT / 2 + 50), Color.Red);
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Generate drop context for depth charges
        /// </summary>
        void GenerateDropContexts()
        {
            const int DROP_CONTENT_Y = 180;
            const int size = 10;
            dropContextRectangles.Add(new Rectangle(AssetsConstantNames.DROP_CONTENT_X1, DROP_CONTENT_Y, size, size));
            dropContextRectangles.Add(new Rectangle(AssetsConstantNames.DROP_CONTENT_X2, DROP_CONTENT_Y, size, size));
            dropContextRectangles.Add(new Rectangle(AssetsConstantNames.DROP_CONTENT_X3, DROP_CONTENT_Y, size, size));
            dropContextRectangles.Add(new Rectangle(AssetsConstantNames.DROP_CONTENT_X4, DROP_CONTENT_Y, size, size));
            dropContextRectangles.Add(new Rectangle(AssetsConstantNames.DROP_CONTENT_X5, DROP_CONTENT_Y, size, size));
            dropContextRectangles.Add(new Rectangle(AssetsConstantNames.DROP_CONTENT_X6, DROP_CONTENT_Y, size, size));

            for (int i = 0; i < dropContextRectangles.Count; i++)
            {
                dropContextStatusIsInUse.Add(i, false);
            }
        }

        /// <summary>
        /// Removes all inactive objects - just a simple cleanup process
        /// </summary>
        void RemoveInactiveObjects()
        {
            RemoveInactiveTorpedos();
            RemoveInActiveMines();
            RemoveInActiveExplisions();
            RemoveInactiveDepthCharges();
        }

        /// <summary>
        /// Removes inactive explosions
        /// </summary>
        void RemoveInActiveExplisions()
        {
            for (int i = explosions.Count - 1; i >= 0; i--)
            {
                if (!explosions[i].IsActive)
                    explosions.RemoveAt(i);
            }
        }

        /// <summary>
        /// Remove inactive torpedos
        /// </summary>
        void RemoveInactiveTorpedos()
        {
            for (int i = SeaChaseGame.torpedos.Count - 1; i >= 0; i--)
            {
                if (!SeaChaseGame.torpedos[i].IsActive)
                    SeaChaseGame.torpedos.RemoveAt(i);
            }
        }

        /// <summary>
        /// Removes inactive depth charges
        /// </summary>
        void RemoveInactiveDepthCharges()
        {
            for (int i = depthCharges.Count - 1; i >= 0; i--)
            {
                if (!depthCharges[i].IsActive)
                    depthCharges.RemoveAt(i);
            }
        }

        /// <summary>
        /// Remove inactive mines
        /// </summary>
        void RemoveInActiveMines()
        {
            for (int i = mineField.MineList.Count - 1; i >= 0; i--)
            {
                if (!mineField.MineList[i].IsActive)
                    mineField.MineList.RemoveAt(i);
            }
        }

        /// <summary>
        /// Creates new torpedo
        /// </summary>
        /// <param name="torpedo"></param>
        public static void AddTorpedo(Torpedo torpedo)
        {
            torpedos.Add(torpedo);
        }

        /// <summary>
        /// Generates random background color
        /// </summary>
        void GenerateBackgroundColor()
        {            
            backgroundColors.Add(Color.WhiteSmoke);
            backgroundColors.Add(Color.Wheat);
            backgroundColors.Add(Color.Violet);
            backgroundColors.Add(Color.Turquoise);
            backgroundColors.Add(Color.Thistle);

            Random rnd = new Random();
            int colorIndex = rnd.Next(0, backgroundColors.Count);
            mainColorTheme = backgroundColors[colorIndex];
            backupColor = mainColorTheme;
        }
    }
}