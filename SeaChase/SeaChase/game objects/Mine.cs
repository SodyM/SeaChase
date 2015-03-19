using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SeaChase.game_objects.lib;

namespace SeaChase.game_objects
{
    /// <summary>
    /// Mine
    /// </summary>
    class Mine : UiObject
    {               
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">Contentmanager</param>
        /// <param name="drawRectangle">Rectangle</param>
        public Mine(ContentManager content, Rectangle drawRectangle)
        {
            sprite = content.Load<Texture2D>(AssetsConstantNames.MINE_IMAGE);
            Collision_Offset = 4;

            this.drawRectangle = drawRectangle;

            collisionRectangle = new Rectangle(drawRectangle.X - Collision_Offset,
                                                drawRectangle.Y - Collision_Offset,
                                                drawRectangle.Width - Collision_Offset,
                                                drawRectangle.Height - Collision_Offset);
        }
    }
}