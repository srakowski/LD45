using Microsoft.Xna.Framework.Graphics;

namespace Coldsteel
{
    public interface ISprite
    {
        string SpriteLayerName { get; }
        bool Enabled { get; }
        void Draw(SpriteBatch spriteBatch);
    }
}
