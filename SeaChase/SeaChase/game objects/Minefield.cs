using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SeaChase.game_objects
{
    /// <summary>
    /// Minefield
    /// </summary>
    class MineField
    {
        Texture2D sprite;
        List<Mine> mineList;
        public List<Mine> MineList
        {
            get { return mineList; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">Contentmanager</param>
        public MineField(ContentManager content)
        {
            sprite = content.Load<Texture2D>(AssetsConstantNames.MINE_IMAGE);
            CreateMineField(content);            
        }

        /// <summary>
        /// Creates whole minefield
        /// </summary>
        /// <param name="content">Contentmanager</param>
        void CreateMineField(ContentManager content)
        {
            mineList = new List<Mine>();

            int xStep = 135;
            int yStep = 160;
            for (int test = 0; test < 3; test++)
            {
                yStep += 100;
                for (int i = 1; i < 6; i++)
                {
                    int x = xStep * i;
                    Mine mine = new Mine(content, new Rectangle(x - sprite.Width / 2, yStep - sprite.Height / 2, sprite.Width, sprite.Height));
                    mineList.Add(mine);
                }
            }
        }

        /// <summary>
        /// Draws minefield
        /// </summary>
        /// <param name="spritebatch">Spritebatch</param>
        /// <param name="gameTime">Gametime</param>
        public void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            foreach (var mine in mineList)
            {
                if (mine.IsActive)
                    mine.Draw(spritebatch, gameTime);
            }            
        }
    }
}